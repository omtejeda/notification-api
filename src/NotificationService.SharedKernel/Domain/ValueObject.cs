namespace NotificationService.SharedKernel.Domain;

/// <summary>
/// Represents a base class for value objects that are defined by their values,
/// rather than by a unique identifier.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Compares two value objects for equality.
    /// </summary>
    /// <param name="left">The left value object.</param>
    /// <param name="right">The right value object.</param>
    /// <returns>True if both value objects are equal; otherwise, false.</returns>
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

    /// <summary>
    /// Compares two value objects for inequality.
    /// </summary>
    /// <param name="left">The left value object.</param>
    /// <param name="right">The right value object.</param>
    /// <returns>True if both value objects are not equal; otherwise, false.</returns>
    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Determines whether the specified value object is equal to the current value object.
    /// </summary>
    /// <param name="other">The other value object to compare.</param>
    /// <returns>True if the specified value object is equal to the current value object; otherwise, false.</returns>
    public virtual bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current value object.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns>True if the specified object is equal to the current value object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return obj is ValueObject valueObject && ValuesAreEqual(valueObject);
    }

    /// <summary>
    /// Returns a hash code for the current value object.
    /// </summary>
    /// <returns>A hash code for the current value object.</returns>
    public override int GetHashCode()
    {
        return GetAtomicValues().Aggregate(
            default(int),
            (hashCode, value) =>
                HashCode.Combine(hashCode, value.GetHashCode()));
    }

    /// <summary>
    /// Gets the atomic values that define this value object.
    /// </summary>
    /// <returns>A collection of atomic values.</returns>
    protected abstract IEnumerable<object> GetAtomicValues();

    private bool ValuesAreEqual(ValueObject valueObject)
    {
        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }
}