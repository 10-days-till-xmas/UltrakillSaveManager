using SaveManagerCLI.OptionTree;

namespace SaveManagerCLI.SaveManipulation.ClassManipulation.ValueWrapper;

internal abstract class ValueNode(string name, Wrapper value) : Node<Wrapper>(name, value)
{
    public object? NodeValue
    {
        get => Value.Getter();
        set => Value.Setter(value);
    }

    internal static ValueNode CreateNodeFromField(object obj, string name)
    {
        var field = obj.GetType().GetField(name) ?? throw new InvalidOperationException($"Field {name} not found in {obj.GetType().Name}");

        Wrapper wrapper = Wrapper.CreateItem(field, obj);
        return CreateNodeFromWrapper(wrapper);
    }

    internal static ValueLeaf CreateLeafFromWrappedPrimitive(Wrapper wrapper)
    {
        if (!wrapper.IsPrimitive)
        {
            throw new InvalidOperationException("Only primitives can be leaves");
        }

        string name = wrapper switch
        {
            ArrayItemWrapper arrayItemWrapper => arrayItemWrapper.ParentArray.GetType().Name + $"[{arrayItemWrapper.Index}]",
            FieldWrapper fieldWrapper => fieldWrapper.FieldInfo.Name,
            _ => throw new NotImplementedException("Only Arrays and Fields are supported"),
        };
        return new ValueLeaf(name, wrapper);
    }

    internal static ValueBranch CreateBranchFromWrapper(Wrapper wrapper, params ValueNode[] children)
    {
        if (wrapper.IsPrimitive)
        {
            throw new InvalidOperationException("Wrapped primitives cannot be Branches");
        }

        string name = wrapper switch
        {
            ArrayItemWrapper arrayItemWrapper => arrayItemWrapper.ParentArray.GetType().Name + $"[{arrayItemWrapper.Index}]",
            FieldWrapper fieldWrapper => fieldWrapper.FieldInfo.Name,
            _ => throw new NotImplementedException("Only Arrays and Fields are supported"),
        };
        return new ValueBranch(name, wrapper, children);
    }

    internal static ValueNode CreateNodeFromWrapper(Wrapper wrapper)
    {
        // if primitive, create leaf, else create branch recursively
        if (wrapper.IsPrimitive)
        {
            return CreateLeafFromWrappedPrimitive(wrapper);
        }

        var childWrappers = wrapper.CreateItems();
        List<ValueNode> childrenNodes = [];
        foreach (var childWrapper in childWrappers)
        {
            childrenNodes.Add(CreateNodeFromWrapper(childWrapper));
        }
        return CreateBranchFromWrapper(wrapper, [.. childrenNodes]);
    }
}