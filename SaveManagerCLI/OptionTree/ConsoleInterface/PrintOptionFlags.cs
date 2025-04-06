namespace SaveManagerCLI.OptionTree.ConsoleInterface;

[Flags]
public enum PrintOptionFlags
{
    None = 0,
    PreClearConsole = 1, // TODO: Implement more options
    Default = PreClearConsole
}