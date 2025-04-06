using SaveManagerCLI.OptionTree;
using SaveManagerCLI.OptionTree.ConsoleInterface;

namespace SaveManagerCLI.SaveManipulation;

internal class SimpleDirectoryExplorer
{
    internal enum DirectorySorting
    {
        None = 0,
        Alphabetical,
        Type
    }

    private readonly Branch<DirectoryInfo> rootBranch;

    public SimpleDirectoryExplorer(string rootDir)
    {
        DirectoryInfo directory = new(rootDir);
        Node[] children = CreateNodeTree(directory);
        rootBranch = new Branch<DirectoryInfo>(directory.Name, directory, children);
    }

    public static Node[] CreateNodeTree(DirectoryInfo directory)
    {
        List<Node> nodes = [];

        foreach (var fileDirs in directory.GetFileSystemInfos())
        {
            switch (fileDirs)
            {
                case FileInfo file:
                    nodes.Add(new Leaf<FileInfo>(file.Name, file));
                    break;

                case DirectoryInfo subDir:
                    Node[] dirChildren = CreateNodeTree(subDir);
                    nodes.Add(new Branch<DirectoryInfo>(subDir.Name, subDir, dirChildren));
                    break;

                default:
                    throw new NotSupportedException("Unsupported FileSystemInfo type");
            }
        }

        return nodes.ToArray();
    }

    public static void PrintDirectoryTree()
    {
        SimpleDirectoryExplorer explorer = new(ProgramSettings.SaveDirectory);
        Option root = new(explorer.rootBranch);
        OptionSelector selector = new(root);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(root.Name);
        Console.ResetColor();
        FileInfo selectedSave;
        Console.CursorTop = 0;
        do
        {
            selectedSave = ConsoleOptionSelector.PrintOptionSelector<FileInfo>(selector, PrintOptionFlags.Default, InputHandlingFlags.AllowEscaping | InputHandlingFlags.UseNumber);
            if (selectedSave is null)
            {
                return;
            }
            if (selectedSave.Extension is ".bepis")
            {
                break;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            ConsoleUtils.ClearLine();
            Console.WriteLine("Invalid File Type");
            --Console.CursorTop;
            Console.ResetColor();
        } while (true);

        SaveNavigator navigator = new(selectedSave);
        navigator.PrintSaveData();
    }
}