using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Identity;

public class Password : ValueObject
{
    private Password(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    protected override IEnumerable<IComparable> EqualityValues
    {
        get { yield return Value; }
    }

    public static Result<Password> Create(string password, PasswordOptions options)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < options.RequiredLength)
        {
            return Fail<Password>($"The password is too short. minimum lenght is [{options.RequiredLength}].");
        }

        if (options.RequireNonAlphanumeric && password.All(IsLetterOrDigit))
        {
            return Fail<Password>("The password requires non alphanumeric.");
        }

        if (options.RequireDigit && !password.Any(IsDigit))
        {
            return Fail<Password>("The password requires digits.");
        }

        if (options.RequireLowercase && !password.Any(IsLower))
        {
            return Fail<Password>("The password requires lowercase letters.");
        }

        if (options.RequireUppercase && !password.Any(IsUpper))
        {
            return Fail<Password>("The password requires uppercase letters.");
        }

        if (options.RequiredUniqueChars >= 1 && password.Distinct().Count() < options.RequiredUniqueChars)
        {
            return Fail<Password>($"The password requires at least [{options.RequiredUniqueChars}] unique characters.");
        }

        return new Password(password);
    }

    private static bool IsDigit(char c)
    {
        return c is >= '0' and <= '9';
    }

    private static bool IsLower(char c)
    {
        return c is >= 'a' and <= 'z';
    }

    private static bool IsUpper(char c)
    {
        return c is >= 'A' and <= 'Z';
    }

    private static bool IsLetterOrDigit(char c)
    {
        return IsUpper(c) || IsLower(c) || IsDigit(c);
    }

    public override string ToString()
    {
        return Value;
    }
}