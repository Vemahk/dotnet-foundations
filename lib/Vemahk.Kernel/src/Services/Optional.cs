using System.Text.Json.Serialization;

namespace Vemahk.Kernel.Services;

/// <summary>
/// Because Nullable<> is too picky.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Optional<T> : IEquatable<Optional<T>>
    where T : notnull // we're basically lying sometimes.
{
    public static Optional<T> None = default;

    public bool HasValue { get; }
    public T Value { get; }

    [JsonConstructor]
    public Optional(bool hasValue, T value)
    {
        HasValue = hasValue;
        Value = value;
    }

    public bool Equals(Optional<T> other)
    {
        if (!HasValue) return !other.HasValue;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        int hashCode = 1816676634;
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Value);
        hashCode = hashCode * -1521134295 + HasValue.GetHashCode();
        return hashCode;
    }

    public override bool Equals(object obj)
    {
        return obj is Optional<T> other ? Equals(other) : false;
    }

    public static implicit operator Optional<T>(T data) => new(true, data);
}