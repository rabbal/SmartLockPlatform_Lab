using System.Text.Json;
using System.Text.RegularExpressions;

namespace SmartLockPlatform.Host.Base;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToSnakeCase();
}

public static partial class StringExtensions  
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }

        var underscores = StartUnderscoreRegex().Match(input);
        return underscores + SnakeCaseRegex().Replace(input, "$1_$2").ToLower();
    }

    [GeneratedRegex("^_+")]
    private static partial Regex StartUnderscoreRegex();
    [GeneratedRegex("([a-z0-9])([A-Z])")]
    private static partial Regex SnakeCaseRegex();
}