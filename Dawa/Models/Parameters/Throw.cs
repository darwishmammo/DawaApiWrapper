using System;

namespace Dawa.Models.Parameters;

internal static class Throw
{
    public static T IfNull<T>(T? value, string? paramName = null, string? message = null)
    {
        if (value is null)
            throw new ArgumentNullException(paramName ?? nameof(value), message);
        return value;
    }

    public static string IfNullOrWhitespace(string? value, string? paramName = null, string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(message ?? "Value cannot be null or whitespace.", paramName ?? nameof(value));
        return value;
    }

    public static T[] IfNullOrEmpty<T>(T[]? array, string? paramName = null, string? message = null)
    {
        if (array == null || array.Length == 0)
            throw new ArgumentException(message ?? "Array cannot be null or empty.", paramName ?? nameof(array));
        return array;
    }

    public static T IfOutOfRange<T>(T value, T min, T max, string? paramName = null, string? message = null) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            throw new ArgumentOutOfRangeException(paramName ?? nameof(value), value, message ?? $"Value must be between {min} and {max}.");
        return value;
    }

    public static bool IfFalse(bool condition, string? message = null)
    {
        if (!condition)
            throw new InvalidOperationException(message ?? "Condition must be true.");
        return condition;
    }

    public static bool IfTrue(bool condition, string? message = null)
    {
        if (condition)
            throw new InvalidOperationException(message ?? "Condition must be false.");
        return condition;
    }

    public static Guid IfInvalidGuid(Guid value, string? paramName = null, string? message = null)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(message ?? "GUID cannot be empty.", paramName ?? nameof(value));
        return value;
    }

    public static Guid IfInvalidGuid(string? value, string? paramName = null, string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value) || !Guid.TryParse(value, out var parsed) || parsed == Guid.Empty)
            throw new ArgumentException(message ?? "Value must be a valid non-empty GUID.", paramName ?? nameof(value));
        return parsed;
    }
}
