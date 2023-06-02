using System.Text.RegularExpressions;

namespace SmartLockPlatform.Application.Base;

public class SortExpression
{
    private const string ExpressionPattern =
        @"(?<direction>desc|asc)\((?<field>[a-zA-Z_][a-zA-Z0-9_\.]*)\)|(?<field>[a-zA-Z_][a-zA-Z0-9_\.]*)[_.:\s]{1}?(?=(?<direction>desc|asc))|(?<=(?<direction>[-+]?))(?<field>[a-zA-Z_][a-zA-Z0-9_\.]*)";
    private static readonly Regex SortingRegex = new(ExpressionPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromSeconds(2));

    public SortExpression(string field, bool descending)
    {
        if (string.IsNullOrEmpty(field)) throw new ArgumentNullException(nameof(field));

        Field = field;
        Descending = descending;
    }

    /// <summary>
    /// Gets or sets the name of the sorted field (property).
    /// </summary>
    public string Field { get; }

    /// <summary>
    /// Gets or sets the sort direction. Should be "true" for descending and "false" for "ascending".
    /// </summary>
    public bool Descending { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not SortExpression sort) return false;

        if (ReferenceEquals(this, obj)) return true;

        return Field.Equals(sort.Field) && Descending == sort.Descending;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (((Field?.GetHashCode()) ?? 0) * 397) ^ Descending.GetHashCode();
        }
    }

    public override string ToString()
    {
        return $"{Field} {(Descending ? "desc" : "asc")}";
    }

    public static SortExpression FromString(string pattern)
    {
        if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException(nameof(pattern));

        var result = SortingRegex.Match(pattern);
        if (!result.Success)
        {
            throw new ArgumentException(
                "Invalid sorting pattern. The supported patterns involve field.desc|field_desc|field:desc|field desc|-field|desc(field)");
        }

        var field = result.Groups["field"].Value;
        var direction = result.Groups["direction"].Value;

        return new SortExpression(field, direction == "-" || direction == "desc");
    }
}