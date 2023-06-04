using System.Reflection;

namespace SmartLockPlatform.Domain.Base;

public abstract class Enumeration : ValueObject
{
    protected Enumeration(int value, string displayName)
    {
        Value = value;
        DisplayName = displayName;
    }

    public int Value { get; init; }
    public string DisplayName { get; init; }

    protected override IEnumerable<IComparable?> EqualityValues
    {
        get
        {
            yield return Value;
        }
    }

    public override string ToString()
    {
        return DisplayName;
    }
    public static IEnumerable<T> List<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<T>();
    
    public static int AbsoluteDifference(Enumeration left, Enumeration right)
    {
        var absoluteDifference = Math.Abs(left.Value - right.Value);
        return absoluteDifference;
    }

    public static T FromValue<T>(int value) where T : Enumeration
    {
        var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);
        return matchingItem;
    }

    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
        var matchingItem = Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
        return matchingItem;
    }
    
    private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = List<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

        return matchingItem;
    }
}