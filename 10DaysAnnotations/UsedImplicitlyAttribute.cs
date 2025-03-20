namespace _10DaysAnnotations;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class UsedImplicitlyAttribute : Attribute
{

    public string Reason { get; } = "";

    public UsedImplicitlyAttribute() { }

    public UsedImplicitlyAttribute(string reason)
    {
        Reason = reason;
    }
}
