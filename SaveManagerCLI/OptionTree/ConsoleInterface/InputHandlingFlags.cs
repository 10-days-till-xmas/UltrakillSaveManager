namespace SaveManagerCLI.OptionTree.ConsoleInterface;

[Flags]
public enum InputHandlingFlags
{
    None = 0,
    AllowEscaping = 1,
    UseNumber = 2,
    Default = UseNumber
}