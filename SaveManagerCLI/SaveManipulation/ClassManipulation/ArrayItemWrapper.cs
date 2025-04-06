namespace SaveManagerCLI.SaveManipulation.ClassManipulation;

internal class ArrayItemWrapper : Wrapper
{
    internal Array ParentArray { get; }
    internal int Index { get; }

    private ArrayItemWrapper(Array parentArray, int index)
        : base(getter: () => parentArray.GetValue(index),
               setter: val => parentArray.SetValue(val, index))
    {
        ParentArray = parentArray;
        Index = index;
    }

    internal static ArrayItemWrapper CreateArrayItemWrapper(string name, Array parentArray, int index)
    {
        return new ArrayItemWrapper(parentArray, index) { Name = name };
    }
}