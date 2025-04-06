namespace SaveManagerCLI.SaveManipulation.ClassManipulation;

internal static class ConsoleValueEditor
{
    public static object? PrintValueEditor(string name, object? currentValue, Type type)
    {
        Console.WriteLine($"Editing {name}");
        Console.WriteLine($"Type: {type.Name}");
        Console.WriteLine($"Current value: {currentValue}");
        Console.WriteLine("Enter new value:");
        while (true)
        {
            var newValue = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ConsoleUtils.Error("Invalid input");
                Console.CursorTop -= 2;
                continue;
            }
            if (TryParseValue(newValue, type, out object? result))
            {
                return result;
            }
            else
            {
                ConsoleUtils.Error("Invalid input");
                Console.CursorTop -= 2;
            }
        } 
        throw new Exception("Failed to parse value");
    }

    private static bool TryParseValue(string value, Type type, out object? result)
    {
        try
        {
            result = Convert.ChangeType(value, type);
            return true;
        }
        catch (Exception)
        {
            result = null;
            return false;
        }
    }
}