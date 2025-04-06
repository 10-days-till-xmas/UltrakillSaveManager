using SaveManagerCLI.OptionTree;
using SaveManagerCLI.OptionTree.ConsoleInterface;

namespace SaveManagerCLI;

internal static class ProgramTest
{
    internal static Leaf<Action>[] MakeLeaves(string namePrefix, int count, int start)
    {
        Leaf<Action>[] leaves = new Leaf<Action>[count];

        for (int i = 0; i < count; i++)
        {
            leaves[i] = new Leaf<Action>($"{namePrefix}{start + i}", () => Console.WriteLine($"{namePrefix} {start + i}"));
        }
        return leaves;
    }

    internal static Branch MakeBranchOfLeaves(string namePrefix, int count)
    {
        return new Branch(namePrefix, MakeLeaves(namePrefix, count, 0));
    }

    // Over the top, but it covers many cases
    internal static readonly Branch TestBranch =
        new("Main Root Test", [
            ..MakeLeaves("L", 20, 0),
            new Branch("B20", [
                ..MakeLeaves("B21.L", 15, 0),
                new Branch("B21.B15", [
                    ..MakeLeaves("B21.B15.L", 15, 0),
                    new Branch("B21.B15.B15", [
                        ..MakeLeaves("B21.B15.B15.L", 15, 0),
                        MakeBranchOfLeaves("B21.B15.B15.B15", 15),
                        ..MakeLeaves("B21.B15.B15.L", 15, 0),
                        ])
                    ])
                ]),
            ..MakeLeaves("L", 15, 21)
            ]);

    internal static readonly OptionSelector testSelector = new(new Option(TestBranch));

    internal static void Test()
    {
        Console.Clear();
        //Action onExecute = baseSelector.PrintOptionSelector<Action>();
        Action onExecute = ConsoleOptionSelector.PrintOptionSelector<Action>(testSelector);
        onExecute();
    }
}