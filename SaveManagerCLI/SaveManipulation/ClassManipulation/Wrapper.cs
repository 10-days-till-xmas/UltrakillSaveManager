using System.Collections;
using System.Reflection;

namespace SaveManagerCLI.SaveManipulation.ClassManipulation;

/// <summary>
/// Wrapper class to wrap fields, properties, and some collection types
/// </summary>
internal class Wrapper
{
    internal enum WrappedValueCategory
    {
        Field,
        CollectionItem,
        ArrayItem = CollectionItem,
        DictionaryItem = CollectionItem
    }

    public string Name { get; init; } = string.Empty;

    public Action<object?> Setter { get; init; }
    public Func<object?> Getter { get; init; }

    /// <summary>
    /// Whether the wrapped object is a primitive type (or a string)
    /// </summary>
    internal bool IsPrimitive => WrappedType.IsPrimitive || WrappedType == typeof(string);

    /// <summary>
    /// The type of the wrapped object
    /// </summary>
    internal Type WrappedType { get; init; }

    protected Wrapper(Func<object?> getter, Action<object?> setter)
    {
        Setter = setter;
        Getter = getter;
        WrappedType = getter()!.GetType();
    }

    protected Wrapper(string name, Func<object?> getter, Action<object?> setter)
    {
        Name = name;
        Setter = setter;
        Getter = getter;
        WrappedType = getter()!.GetType();
    }

    /// <summary>
    /// Create a new <see cref="Wrapper"/> for each item in the wrapped object
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<Wrapper> CreateItems()
    {
        object instance = Getter() ?? throw new Exception("Wrapped item is null");

        if ((instance is ValueType value && value.GetType().IsPrimitive) || instance is string)
            throw new Exception("Cannot wrap primitive types");

        return instance switch
        {
            Array array => CreateItemsFromArray(array, Name),
            IDictionary dictionary => CreateItemsFromDictionary(dictionary),
            _ => CreateItemsFromObject(instance),
        };
    }

    /// <summary>
    /// Create a ValueWrapper for a field
    /// </summary>
    /// <param name="field">The <see cref="FieldInfo"/> for the field</param>
    /// <param name="instance">The instance of an object where the <paramref name="field"/> is defined</param>
    /// <returns>A new <see cref="Wrapper"/> of the specified field and instance</returns>
    public static FieldWrapper CreateItem(FieldInfo field, object instance)
    {
        return FieldWrapper.CreateFieldWrapper(field, instance);
    }

    /// <summary>
    /// Create a ValueWrapper for a property
    /// </summary>
    /// <param name="property">The <see cref="PropertyInfo"/> for the property</param>
    /// <param name="instance">The instance of an object where the <paramref name="property"/> is defined</param>
    /// <returns>A new <see cref="Wrapper"/> of the specified property and instance</returns>
    public static Wrapper CreateItem(PropertyInfo property, object instance)
    {
        object? getter() => property.GetValue(instance);
        void setter(object? val) => property.SetValue(instance, val);
        return new Wrapper(getter, setter);
    }

    /// <summary>
    /// Create a ValueWrapper for an item in an <see cref="Array"/>
    /// </summary>
    /// <param name="array"></param>
    /// <param name="index">The index of the item in <paramref name="array"/> to wrap</param>
    /// <returns>A new <see cref="Wrapper"/> corresponding to <paramref name="array"/>[<paramref name="index"/>]</returns>
    public static ArrayItemWrapper CreateItem(string name, Array array, int index)
    {
        return ArrayItemWrapper.CreateArrayItemWrapper($"{name}[{index}]", array, index);
    }

    /// <summary>
    /// Create a ValueWrapper for an item in an <see cref="IDictionary"/>
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key">The index of the item in <paramref name="dictionary"/> to wrap</param>
    /// <returns>A new <see cref="Wrapper"/> corresponding to <paramref name="dictionary"/>[<paramref name="key"/>]</returns>
    public static Wrapper CreateItem(IDictionary dictionary, object key)
    {
        object? getter() => dictionary[key];
        void setter(object? val) => dictionary[key] = val;
        return new Wrapper(getter, setter);
    }

    private static IEnumerable<ArrayItemWrapper> CreateItemsFromArray(Array array, string arrayName)
    {
        return Enumerable.Range(0, array.Length).Select(i => CreateItem(arrayName, array, i));
    }

    private static IEnumerable<Wrapper> CreateItemsFromDictionary(IDictionary dictionary) => dictionary.Keys.Cast<object>().Select(i => CreateItem(dictionary, i));

    private static IEnumerable<Wrapper> CreateItemsFromObject(object instance)
    {
        var type = instance.GetType();
        var fields = type.GetFields().Select(field => CreateItem(field, instance));
        var properties = type.GetProperties().Select(property => CreateItem(property, instance));
        return [.. fields, .. properties];
    }
}