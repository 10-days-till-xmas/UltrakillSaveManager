namespace SaveManagerCLI;

public static class ConsoleUtils
{

    public static string ColoredString(string text, byte red, byte green, byte blue)
    {
        return $"{CustomColor(red, green, blue)}{text}{ResetCustomColor()}";
    }
    public static string CustomColor(byte red, byte green, byte blue)
    {
        return $"\u001b[38;2;{red};{green};{blue}m";
    }
    public static string ResetCustomColor()
    {
        return "\u001b[0m";
    }

    public static void Warn(object? message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public static void Error(object? message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public static void Success(object? message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public static void Info(object? message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void ClearLine(int? line = null)
    {
        line ??= Console.CursorTop;
        int oldTop = Console.CursorTop;
        Console.SetCursorPosition(0, (int)line);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, oldTop);
    }
    public static void ClearLines(int end)
    {
        ClearLines(Console.CursorTop, end);
    }
    public static void ClearLines(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            ClearLine(i);
        }
    }
}
