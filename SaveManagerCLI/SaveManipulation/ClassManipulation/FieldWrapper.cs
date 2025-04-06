using System.Reflection;

namespace SaveManagerCLI.SaveManipulation.ClassManipulation;

internal class FieldWrapper : Wrapper
{
    internal object Instance { get; }
    internal FieldInfo FieldInfo { get; }

    private FieldWrapper(FieldInfo field, object instance)
        : base(
            name: field.DeclaringType?.Name + '.' + field.Name,
            getter: () => field.GetValue(instance),
            setter: (val) => field.SetValue(instance, val))
    {
        if (field.IsStatic)
            throw new Exception("Cannot wrap static fields"); // TODO: Handle static fields if necessary?
        if (field.IsLiteral)
            throw new Exception("Cannot wrap literal fields");
        if (field.Attributes.HasFlag(FieldAttributes.InitOnly))
            throw new Exception("Cannot wrap readonly fields");
        Instance = instance;
        FieldInfo = field;
    }

    internal static FieldWrapper CreateFieldWrapper(FieldInfo field, object instance)
    {
        return CreateFieldWrapper(field.Name, field, instance);
    }

    internal static FieldWrapper CreateFieldWrapper(string name, FieldInfo field, object instance)
    {
        return new FieldWrapper(field, instance) { Name = name };
    }
}