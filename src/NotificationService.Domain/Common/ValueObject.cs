namespace NotificationService.Domain.Common;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }

    public virtual bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject valueObject && ValuesAreEqual(valueObject);
    }

    public override int GetHashCode()
    {
        return GetAtomicValues().Aggregate(
            default(int),
            (hashCode, value) =>
            HashCode.Combine(hashCode, value.GetHashCode()));
    }

    protected abstract IEnumerable<object> GetAtomicValues();

    private bool ValuesAreEqual(ValueObject valueObject)
    {
        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }
}