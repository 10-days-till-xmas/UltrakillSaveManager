namespace SaveManagerCLI.SaveManipulation.ClassManipulation;

internal class ConsoleValueEditor
{
    internal Wrapper ValueWrapper { get; init; }
    internal string ValueName { get; init; }
    internal Type ValueType => ValueWrapper.WrappedType;

    internal string ValueTypeName => ValueType.Name;

    internal ConsoleValueEditor(string name, Wrapper wrapper)
    {
        ValueName = name;
        ValueWrapper = wrapper;
    }

    public void PrintValueEditor()
    {
        Console.WriteLine($"Editing {ValueName}");
        Console.WriteLine($"Type: {ValueTypeName}");
        Console.WriteLine($"Current value: {ValueWrapper.Getter()}");
        Console.WriteLine("Enter new value:");
        bool repeat = true;
        do
        {
            string? newValue = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newValue))
            {
                Console.WriteLine("Invalid input");
                continue;
            }
            if (TryParseValue(newValue, out object? result))
            {
                ValueWrapper.Setter(result);
                Console.WriteLine("Value set");
                repeat = false;
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
        } while (repeat);
    }

    private bool TryParseValue(string value, out object? result)
    {
        try
        {
            result = Convert.ChangeType(value, ValueType);
            return true;
        }
        catch (Exception)
        {
            result = null;
            return false;
        }
    }
}