namespace SaveManagerCLI.OptionTree.ConsoleInterface;

internal class ConsoleOptionSelector(OptionSelector optionSelector)
{
    private OptionSelector OptionSelector => optionSelector;
    private readonly int ConsoleLine = Console.CursorTop;
    private int messageLine;

    /// <summary>
    /// Displays a list of LocalOptions and allows the user to select one by using the arrow keys or the number keys
    /// </summary>
    /// <param name="printOptionFlags">A set of flags that determine how the options will be printed</param>
    /// <param name="inputHandlingFlags">A set of flags that determines how the inputs will be handled</param>
    public static T PrintOptionSelector<T>(OptionSelector optionSelector,
                                           PrintOptionFlags printOptionFlags = PrintOptionFlags.Default,
                                           InputHandlingFlags inputHandlingFlags = InputHandlingFlags.Default)
    {
        return new ConsoleOptionSelector(optionSelector)
            .PrintOptionSelector<T>(printOptionFlags, inputHandlingFlags);
    }

    /// <summary>
    /// Displays a list of LocalOptions and allows the user to select one by using the arrow keys or the number keys
    /// </summary>
    /// <param name="printOptionFlags">A set of flags that determine how the options will be printed</param>
    /// <param name="inputHandlingFlags">A set of flags that determines how the inputs will be handled</param>
    public T PrintOptionSelector<T>(PrintOptionFlags printOptionFlags = PrintOptionFlags.Default,
                                    InputHandlingFlags inputHandlingFlags = InputHandlingFlags.Default)
    {
        bool leafSelected = false;
        T? selectedLeaf = default;
        (bool Previous, bool Current) movedBack = (false, false);
        do
        {
            PrintOptions(printOptionFlags);
            HandleKeyInputs(ref leafSelected, ref selectedLeaf, ref movedBack, inputHandlingFlags);
        } while (!leafSelected);
        return selectedLeaf!;
    }

    /// <summary>
    /// Processes console key inputs to navigate through the options and perform selection actions.
    /// </summary>
    /// <param name="useNumber">
    /// If set to true, number keys (D1-D9 and NumPad1-NumPad9) can be used to directly select an option.
    /// </param>
    /// <param name="leafSelected">
    /// A reference flag that will be set to true when a leaf option is chosen, i.e. hits the end of the tree
    /// </param>
    /// <param name="selectedLeaf">
    /// A reference to the action associated with the selected leaf option.
    /// </param>
    private void HandleKeyInputs<T>(ref bool leafSelected,
                                    ref T? selectedLeaf,
                                    ref (bool Previous, bool Current) failedMoveBack,
                                    InputHandlingFlags inputHandlingFlags)
    {
        bool useNumber = inputHandlingFlags.HasFlag(InputHandlingFlags.UseNumber);
        bool allowEscaping = inputHandlingFlags.HasFlag(InputHandlingFlags.AllowEscaping);

        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
        failedMoveBack.Previous = failedMoveBack.Current;
        failedMoveBack.Current = false;
        ConsoleUtils.ClearLine(messageLine);
        switch (keyInfo.Key)
        {
            case ConsoleKey.Backspace:
            case ConsoleKey.Escape:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.Enter when keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                failedMoveBack.Current = !OptionSelector.GoBack();

                if (!failedMoveBack.Current)
                {
                    // do nothing, it moved back successfully
                }
                else if (!allowEscaping)
                {
                    messageLine = Console.CursorTop;
                    ConsoleUtils.Error("Cannot go back from here.");
                    --Console.CursorTop;
                }
                else if (failedMoveBack.Previous)
                {
                    ++Console.CursorTop;
                    leafSelected = true;
                    selectedLeaf = default;
                }
                else
                {
                    ConsoleUtils.ClearLine();
                    messageLine = Console.CursorTop;
                    ConsoleUtils.Warn($"Press {keyInfo.Key} again to return to the previous menu.");
                    --Console.CursorTop;
                }

                break;

            case ConsoleKey.Enter:
            case ConsoleKey.RightArrow:
                ConsoleUtils.ClearLine(messageLine);
                if (OptionSelector.CurrentOption.Children.Count == 0)
                {
                    ConsoleUtils.Error("No options available");
                }
                PrintOptions(PrintOptionFlags.PreClearConsole, selected: true);
                leafSelected = OptionSelector.Select(out selectedLeaf);
                break;

            case ConsoleKey.UpArrow:
            case ConsoleKey.PageUp:
            case ConsoleKey.Tab when keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                OptionSelector.MoveUp();
                ConsoleUtils.ClearLine(messageLine);
                break;

            case ConsoleKey.DownArrow:
            case ConsoleKey.PageDown:
            case ConsoleKey.Tab when !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                OptionSelector.MoveDown();
                ConsoleUtils.ClearLine(messageLine);
                break;

            case ConsoleKey key when useNumber && (
            ConsoleKey.NumPad1 <= key && key <= ConsoleKey.NumPad9 ||
            ConsoleKey.D1 <= key && key <= ConsoleKey.D9):
                int index;
                index = key >= ConsoleKey.NumPad1 && keyInfo.Key <= ConsoleKey.NumPad9
                    ? keyInfo.Key - ConsoleKey.NumPad1
                    : keyInfo.Key - ConsoleKey.D1;
                try
                {
                    OptionSelector.GoToOption(index);
                }
                catch (ArgumentOutOfRangeException)
                {
                    ConsoleUtils.Error($"{keyInfo.Key} Key out of range");
                }
                break;
        }
    }

    private void PrintOptions(PrintOptionFlags optionFlags, bool selected = false)
    {
        if (optionFlags.HasFlag(PrintOptionFlags.PreClearConsole))
        {
            Console.Clear();
        }
        ConsoleUtils.ClearLines(ConsoleLine, messageLine - 1);
        Console.CursorTop = ConsoleLine;
        Console.CursorLeft = 0;

        int visualIndex = 1;
        Range range = MathUtils.BoundedRange(OptionSelector.LocalOptions.Length, OptionSelector.LocalCurrentIndex, 4);
        int startHideCount = range.Start.Value;
        int endHideCount = OptionSelector.LocalOptions.Length - range.End.Value;

        foreach (var (visibleOption, depth) in OptionSelector.VisibleOptions)
        {
            var index = Array.IndexOf(OptionSelector.LocalOptions, visibleOption);
            if (visibleOption == OptionSelector.CurrentOption)
            {
                ConsoleUtils.ClearLine();
                PrintSelectedOption(selected, ref visualIndex, visibleOption);
            }
            else if (index == -1)
            {
                ConsoleUtils.ClearLine();
                if (visibleOption.IsVisible)
                    PrintUpperOptions(visibleOption);
            }
            else if (range.Contains(index))
            {
                ConsoleUtils.ClearLine();
                Console.WriteLine($"  {visualIndex++}┃{visibleOption}");
            }
            else if (index == range.Start.Value - 1)
            {
                Console.WriteLine($"Collapsed {startHideCount} options");
            }
            else if (index == range.End.Value)
            {
                Console.WriteLine($"Collapsed {endHideCount} options");
            }
            // else dont print
        }
        messageLine = Console.CursorTop;

        static void PrintSelectedOption(bool selected, ref int visualIndex, Option visibleOption)
        {
            Console.BackgroundColor = selected ? ConsoleColor.Green : ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            string ansiColor = selected ? ConsoleUtils.CustomColor("ff" + "0A5A0A") : "";

            Console.WriteLine($"{ansiColor}> {visualIndex++}┃{visibleOption}");
            Console.ResetColor();
        }

        static void PrintUpperOptions(Option visibleOption)
        {
            Console.Write("   ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("│");
            Console.ResetColor();
            Console.WriteLine(visibleOption);
        }
    }
}