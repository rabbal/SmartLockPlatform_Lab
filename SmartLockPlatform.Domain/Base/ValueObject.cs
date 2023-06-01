
namespace SmartLockPlatform.Domain.Base;

[Serializable]
public abstract class ValueObject : IComparable, IComparable<ValueObject>
{
    private int? _hash;
    protected abstract IEnumerable<IComparable?> EqualityValues { get; }

    public int CompareTo(ValueObject? obj)
    {
        if (obj is null)
            return 1;

        if (ReferenceEquals(this, obj))
            return 0;

        var thisType = GetUnproxiedType(this);
        var otherType = GetUnproxiedType(obj);
        if (thisType != otherType)
            return string.Compare($"{thisType}", $"{otherType}", StringComparison.Ordinal);

        return
            EqualityValues.Zip(
                    obj.EqualityValues,
                    (left, right) =>
                        left?.CompareTo(right) ?? (right is null ? 0 : -1))
                .FirstOrDefault(cmp => cmp != 0);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (GetUnproxiedType(this) != GetUnproxiedType(obj))
            return false;

        var valueObject = (ValueObject)obj;

        return EqualityValues.SequenceEqual(valueObject.EqualityValues);
    }

    public override int GetHashCode()
    {
        if (!_hash.HasValue)
        {
            _hash = EqualityValues
                .Aggregate(17, (current, next) =>
                {
                    unchecked
                    {
                        return current * 23 + (next?.GetHashCode() ?? 0);
                    }
                });
        }

        return _hash.Value;
    }

    public int CompareTo(object? obj)
    {
        return CompareTo(obj as ValueObject);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            return true;

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }

    public ValueObject? Copy()
    {
        return MemberwiseClone() as ValueObject;
    }

    public static Type GetUnproxiedType(object obj)
    {
        var type = obj.GetType();
        if (type.ToString().Contains("Castle.Proxies."))
        {
            return type.BaseType!;
        }

        return type;
    }
}