using System.ComponentModel.DataAnnotations;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Identity;

public class Email : ValueObject
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    protected override IEnumerable<IComparable> EqualityValues
    {
        get { yield return Value; }
    }

    public override string ToString()
    {
        return Value;
    }
    
    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
        {
            return Fail<Email>("The email address is invalid.");
        }

        return new Email(email);
    }
}

public class FirstName : ValueObject
{
    public FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    protected override IEnumerable<IComparable> EqualityValues
    {
        get { yield return Value; }
    }

    public override string ToString()
    {
        return Value;
    }
}

public class LastName : ValueObject
{
    public LastName(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    protected override IEnumerable<IComparable> EqualityValues
    {
        get { yield return Value; }
    }

    public override string ToString()
    {
        return Value;
    }
}