using System.Drawing;

namespace SaveManagerCLI;

public static class ConsoleUtils
{
    public static string CustomColor(string colorName)
    {
        return CustomColor(Color.FromName(colorName));
    }

    public static string CustomColor(Color color)
    {
        return $"\u001b[38;2;{color.R};{color.G};{color.B}m";
    }

    public static string CustomColor(byte red, byte green, byte blue)
    {
        Color color = Color.FromArgb(red, green, blue);
        return CustomColor(color);
    }

    public static string ColoredString(string text, Color color)
    {
        return $"{CustomColor(color)}{text}{ResetCustomColor()}";
    }

    public static string ResetCustomColor()
    {
        return "\u001b[0m";
    }

    public static void WaitForEnterPress()
    {
        Console.WriteLine("Press Enter to continue...");
        do
        {
        } while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter);
    }

    public static void WaitForKeyPress()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(intercept: true);
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
        int oldTop = Console.CursorTop;
        Console.SetCursorPosition(0, line ?? oldTop);
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