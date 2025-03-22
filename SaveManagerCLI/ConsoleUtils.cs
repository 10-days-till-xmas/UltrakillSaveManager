namespace SaveManagerCLI;

public static class ConsoleUtils
{
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
}
