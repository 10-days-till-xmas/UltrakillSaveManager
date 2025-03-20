using SaveManagerCLI.MenuTools.OptionTree;

namespace SaveManagerCLI.MenuTools
{
    internal class OptionSelector(string[] options)
    {
        readonly string[] options = options;
        readonly Branch Root;

        int selectedIndex = 0;
        readonly int ConsoleLine = Console.CursorTop; // This is the line where the options will be displayed
        ConsoleKeyInfo keyInfo;

        /// <summary>
        /// Displays a list of options and allows the user to select one by using the arrow keys or the number keys
        /// </summary>
        /// <param name="useNumber">If true, the user can select an option by pressing the number key corresponding to the option</param>
        /// <param name="option">The index of the selected option</param>
        public void PrintOptionSelector(bool useNumber, out int option)
        {
            do
            {
                Console.CursorTop = ConsoleLine;
                Console.CursorLeft = 0;

                for (int i = 0; i < options.Length; i++)
                {
                    if (selectedIndex == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"> {i + 1}. {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {i + 1}. {options[i]}");
                    }
                }

                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.PageUp:
                    case ConsoleKey.Tab when keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                        selectedIndex = selectedIndex == 0 ? options.Length - 1 : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.PageDown:
                    case ConsoleKey.Tab when !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift):
                        selectedIndex = selectedIndex == options.Length - 1 ? 0 : selectedIndex + 1;
                        break;

                    case ConsoleKey n when useNumber && n >= ConsoleKey.D1 && n <= ConsoleKey.D9:
                        if ((int)keyInfo.Key - (int)ConsoleKey.D1 >= options.Length)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{keyInfo.Key} Key out of range");
                            Console.ResetColor();
                            break;
                        }
                        else
                        {
                            selectedIndex = (int)keyInfo.Key - (int)ConsoleKey.D1;
                            break;
                        }

                    case ConsoleKey n when useNumber && n >= ConsoleKey.NumPad1 && n <= ConsoleKey.NumPad9:
                        if ((int)keyInfo.Key - (int)ConsoleKey.NumPad1 >= options.Length)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{keyInfo.Key} Key out of range");
                            Console.ResetColor();
                            break;
                        }
                        else
                        {
                            selectedIndex = (int)keyInfo.Key - (int)ConsoleKey.NumPad1;
                            break;
                        }

                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.CursorLeft = 0;
            Console.Write(" ".PadRight(Console.WindowWidth - 1)); // Clear the line
            Console.CursorLeft = 0;
            option = selectedIndex;
        }

        public void PrintOptionSelector(bool useNumber, out string option)
        {
            PrintOptionSelector(useNumber, out int selectedIndex);
            option = options[selectedIndex];
        }
    }
}
