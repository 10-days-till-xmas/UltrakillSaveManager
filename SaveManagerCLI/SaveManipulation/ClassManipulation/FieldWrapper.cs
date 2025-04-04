using System.Reflection;

namespace SaveManagerCLI.SaveManipulation.ClassManipulation;

internal class FieldWrapper : Wrapper
{
    internal object Instance { get; }
    internal FieldInfo FieldInfo { get; }

    private FieldWrapper(FieldInfo field, object instance)
        : base(
            getter: () => field.GetValue(instance),
            setter: (val) => field.SetValue(instance, val))

    {
        Instance = instance;
        FieldInfo = field;
    }

    internal static FieldWrapper CreateFieldWrapper(FieldInfo field, object instance)
    {
        return new FieldWrapper(field, instance);
    }
}