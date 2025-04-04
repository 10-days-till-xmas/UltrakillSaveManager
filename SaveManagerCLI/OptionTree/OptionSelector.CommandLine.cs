namespace SaveManagerCLI.OptionTree;

public partial class OptionSelector
{
    /// <summary>
    /// This is the line where the LocalOptions will be displayed
    /// </summary>
    private readonly int ConsoleLine = Console.CursorTop;

    /// <summary>
    /// Displays a list of LocalOptions and allows the user to select one by using the arrow keys or the number keys
    /// </summary>
    /// <param name="useNumber">If true, the user can select an option by pressing the number key corresponding to the option</param>
    /// <param name="option">The index of the selected option</param>
    public T PrintOptionSelector<T>(bool useNumber = true, bool allowEscapingFromRoot = false,
        bool clearOnReset = true)
    {
        bool leafSelected = false;
        T? selectedLeaf = default;
        (bool Previous, bool Current) movedBack = (false, false);
        do
        {
            if (clearOnReset)
            {
                Console.Clear();
            }
            PrintOptions();
            HandleKeyInputs(useNumber, allowEscapingFromRoot, ref leafSelected, ref selectedLeaf, ref movedBack);
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
    private void HandleKeyInputs<T>(bool useNumber,
                                    bool allowEscaping,
                                    ref bool leafSelected,
                                    ref T? selectedLeaf,
                                    ref (bool Previous, bool Current) failedMoveBack)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
        failedMoveBack.Previous = failedMoveBack.Current;
        failedMoveBack.Current = false;
        switch (keyInfo.Key)
        {
            case ConsoleKey.Backspace:
            case ConsoleKey.Escape:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.Enter when keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                ConsoleUtils.ClearLine(messageLine);
                failedMoveBack.Current = !GoBack();

                if (!failedMoveBack.Current)
                {
                    // do nothing, it moved back successfully
                }
                else if (!allowEscaping)
                {
                    ConsoleUtils.ClearLine();
                    messageLine = Console.CursorTop;
                    ConsoleUtils.Error("Cannot go back from here.");
                    --Console.CursorTop;
                }
                else if (failedMoveBack.Current && !failedMoveBack.Previous)
                {
                    ConsoleUtils.Warn("Cannot go back from here.");
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
                if (CurrentOption.Children.Count == 0)
                {
                    ConsoleUtils.Error("No options available");
                }
                PrintOptions(selected: true);
                leafSelected = Select(out selectedLeaf);
                break;

            case ConsoleKey.UpArrow:
            case ConsoleKey.PageUp:
            case ConsoleKey.Tab when keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                MoveUp();
                ConsoleUtils.ClearLine(messageLine);
                break;

            case ConsoleKey.DownArrow:
            case ConsoleKey.PageDown:
            case ConsoleKey.Tab when !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                MoveDown();
                ConsoleUtils.ClearLine(messageLine);
                break;

            case ConsoleKey key when useNumber && (
            (ConsoleKey.NumPad1 <= key && key <= ConsoleKey.NumPad9) ||
            (ConsoleKey.D1 <= key && key <= ConsoleKey.D9)):
                int index;
                index = key >= ConsoleKey.NumPad1 && keyInfo.Key <= ConsoleKey.NumPad9
                    ? keyInfo.Key - ConsoleKey.NumPad1
                    : keyInfo.Key - ConsoleKey.D1;
                try
                {
                    GoToOption(index);
                }
                catch (ArgumentOutOfRangeException)
                {
                    ConsoleUtils.Error($"{keyInfo.Key} Key out of range");
                }
                break;
        }
    }

    private int messageLine;

    private void PrintOptions(bool selected = false)
    {
        ConsoleUtils.ClearLines(ConsoleLine, messageLine - 1);
        Console.CursorTop = ConsoleLine;
        Console.CursorLeft = 0;

        int visualIndex = 1;
        Range range = MathUtils.BoundedRange(LocalOptions.Length, LocalCurrentIndex, 4);
        int startHideCount = range.Start.Value;
        int endHideCount = LocalOptions.Length - range.End.Value;

        foreach (var (visibleOption, depth) in VisibleOptions)
        {
            var index = Array.IndexOf(LocalOptions, visibleOption);
            if (visibleOption == CurrentOption)
            {
                ConsoleUtils.ClearLine();
                PrintSelectedOption(selected, ref visualIndex, visibleOption);
            }
            else if (index == -1)
            {
                ConsoleUtils.ClearLine();
                if (visibleOption.IsVisible)
                    PrintUpperOptions(visibleOption, depth);
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

        static void PrintUpperOptions(Option visibleOption, int depth)
        {
            Console.Write("   ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("│");
            Console.ResetColor();
            Console.WriteLine(visibleOption);
        }
    }
}