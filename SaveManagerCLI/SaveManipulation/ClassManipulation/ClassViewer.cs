using SaveManagerCLI.OptionTree;
using SaveManagerCLI.SaveManipulation.ClassManipulation.ValueWrapper;

namespace SaveManagerCLI.SaveManipulation.ClassManipulation;

internal class ClassViewer
{
    internal readonly object instance;
    internal readonly Type _type;
    internal readonly string rootName;

    internal ClassViewer(string rootName, object classInstance, Type type)
    {
        instance = classInstance;
        _type = type;
        this.rootName = rootName;
    }

    internal object PrintOptions()
    {
        Node node = MakeBranchFromClass(instance, rootName);
        Option option = new(node);
        OptionSelector selector = new(option);
        Console.ResetColor();
        Console.WriteLine(rootName);
        // TODO: find a way to patch the names, so that it can display its type and value (if primitive), or array size

        return selector.PrintOptionSelector<object>(true, true);
    }

    internal static Branch<object> MakeBranchFromClass(object obj, string? name = null)
    {
        var type = obj.GetType();
        var fields = type.GetFields();
        var children = fields.Select(field => ValueNode.CreateNodeFromField(obj, field.Name));
        return new Branch<object>(name ?? type.Name, obj, children.ToArray());
    }
}