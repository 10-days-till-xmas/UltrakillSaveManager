namespace SaveManagerCLI.OptionTree;

public partial class OptionSelector
{
    /// <summary>
    /// This is the line where the LocalOptions will be displayed
    /// </summary>
    readonly int ConsoleLine = Console.CursorTop;

    /// <summary>
    /// Displays a list of LocalOptions and allows the user to select one by using the arrow keys or the number keys
    /// </summary>
    /// <param name="useNumber">If true, the user can select an option by pressing the number key corresponding to the option</param>
    /// <param name="option">The index of the selected option</param>
    public T PrintOptionSelector<T>(bool useNumber = true, bool allowEscapingFromRoot = false)
    {
        bool leafSelected = false;
        T? selectedLeaf = default;
        (bool Previous, bool Current) movedBack = (false, false);
        do
        {
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
                if (!allowEscaping & failedMoveBack.Current)
                {
                    ConsoleUtils.Warn("Cannot go back from here.");
                    --Console.CursorTop;
                    break;
                }

                if (failedMoveBack.Current && !failedMoveBack.Previous)
                {
                    ConsoleUtils.ClearLine();
                    messageLine = Console.CursorTop;
                    ConsoleUtils.Warn($"Press {keyInfo.Key} again to return to the previous menu.");
                    --Console.CursorTop;
                }
                else if (failedMoveBack.Previous && failedMoveBack.Current)
                {
                    ++Console.CursorTop;
                    leafSelected = true;
                    selectedLeaf = default;
                }
                break;

            case ConsoleKey.Enter:
            case ConsoleKey.RightArrow:
                ConsoleUtils.ClearLine(messageLine);
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
        
        for (int i = 0; i < VisibleOptions.Length; i++)
        {
            var index = Array.IndexOf(LocalOptions, VisibleOptions[i]);
            if (VisibleOptions[i] == CurrentOption)
            {
                ConsoleUtils.ClearLine();
                Console.BackgroundColor = selected ? ConsoleColor.Green : ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                string ansiColor = selected ? ConsoleUtils.CustomColor(0x0a, 0x5a, 0x0a) : "";
                
                Console.WriteLine($"{ansiColor}> {index + 1}┃{VisibleOptions[i]}");
                Console.ResetColor();
            }
            else
            {
                if (index == -1)
                {
                    ConsoleUtils.ClearLine();
                    Console.Write("   ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("│");
                    Console.ResetColor();
                    Console.WriteLine(VisibleOptions[i]);
                }
                else
                {
                    ConsoleUtils.ClearLine();
                    Console.WriteLine($"  {index + 1}┃{VisibleOptions[i]}");
                }
            }
        }
        messageLine = Console.CursorTop;
    }
}
