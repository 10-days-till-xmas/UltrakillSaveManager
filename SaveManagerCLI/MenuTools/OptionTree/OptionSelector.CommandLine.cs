namespace SaveManagerCLI.MenuTools.OptionTree;

public partial class OptionSelector
{
    /// <summary>
    /// This is the line where the Options will be displayed
    /// </summary>
    readonly int ConsoleLine = Console.CursorTop;

    /// <summary>
    /// Displays a list of Options and allows the user to select one by using the arrow keys or the number keys
    /// </summary>
    /// <param name="useNumber">If true, the user can select an option by pressing the number key corresponding to the option</param>
    /// <param name="option">The index of the selected option</param>
    public void PrintOptionSelector(bool useNumber = true)
    {
        bool leafSelected = false;
        Action? selectedLeaf = null;
        do
        {
            Console.CursorTop = ConsoleLine;
            Console.CursorLeft = 0;
            PrintOptions();
            HandleKeyInputs(useNumber, ref leafSelected, ref selectedLeaf);
        } while (!leafSelected);

        ConsoleUtils.ClearLine();
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
    private void HandleKeyInputs(bool useNumber, ref bool leafSelected, ref Action? selectedLeaf)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

        switch (keyInfo.Key)
        {
            case ConsoleKey.Backspace:
            case ConsoleKey.Escape:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.Enter when keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                bool movedBack = GoBack();
                if (!movedBack)
                {
                    ConsoleUtils.Warn($"Root is not accessible!");
                }
                break;

            case ConsoleKey.Enter:
            case ConsoleKey.RightArrow:
                leafSelected = Select(out selectedLeaf);
                break;

            case ConsoleKey.UpArrow:
            case ConsoleKey.PageUp:
            case ConsoleKey.Tab when keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                MoveUp();
                break;

            case ConsoleKey.DownArrow:
            case ConsoleKey.PageDown:
            case ConsoleKey.Tab when !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                MoveDown();
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
                    GoTo(index);
                }
                catch (ArgumentOutOfRangeException)
                {
                    ConsoleUtils.Error($"{keyInfo.Key} Key out of range");
                }
                break;
        }
    }

    private void PrintOptions()
    {
        for (int i = 0; i < Options.Length; i++)
        {
            if (Options[i] == CurrentOption)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"> {i + 1}. {Options[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"  {i + 1}. {Options[i]}");
            }
        }
    }
}
