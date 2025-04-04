using SaveManagerCLI.OptionTree;
using UltrakillSaveManager.SaveFile;

namespace SaveManagerCLI;

internal class ProgramMenu(string saveFolderDir = @"C:/Users/10_days_till_xmas/Documents/coding/C#/UltraModding/ultrakill+bepinex/Saves")
{
    public string saveFolderDir = saveFolderDir;

    public Branch SlotSaves(int index)
    {
        string[] difficulties = ["Harmless", "Lenient", "Standard", "Violent", "Brutal", "UKMD"];
        Slot slot = new(index);

        Leaf DifficultyProgress(string name, int index)
        {
            return new Leaf(name, () => TypedSaveFile.GetSaveFile(saveFolderDir + slot.DifficultyProgress(index)));
        }
        Leaf LevelProgress(int num)
        {
            return new Leaf($"Level {num}", () => TypedSaveFile.GetSaveFile(saveFolderDir + slot.LvlProgress(num)));
        }

        return new Branch($"Slot {index}",
            new Leaf("General Progress", () => TypedSaveFile.GetSaveFile(saveFolderDir + slot.GeneralProgress)),
            new Leaf("Cyber Grind High Score", () => TypedSaveFile.GetSaveFile(saveFolderDir + slot.CyberGrindHiScore)),
            new Branch("Difficulty Progress", [.. difficulties.Select(DifficultyProgress)]),
            new Branch("Level Progress", [.. Slot.ValidLevelNums.Select(LevelProgress)])
        );
    }

    public Branch OptionsTree => new("LocalOptions",
        SlotSaves(1),
        SlotSaves(2),
        SlotSaves(3),
        SlotSaves(4),
        SlotSaves(5)
    );

    public static void DisplayOptions(Node node, int indent = 0)
    {
        string indentation = new(' ', indent * 4);

        if (node is Branch branch)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{indentation}{node.Name}:");
            Console.ResetColor();
            foreach (var child in branch.Children)
            {
                DisplayOptions(child, indent + 1);
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{indentation}{node.Name}");
            Console.ResetColor();
        }
    }
}