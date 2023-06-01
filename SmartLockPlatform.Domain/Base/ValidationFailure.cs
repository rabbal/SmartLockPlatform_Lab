namespace SmartLockPlatform.Domain.Base;

public class ValidationFailure
{
    public ValidationFailure(string? memberName, string message)
    {
        MemberName = memberName ?? string.Empty;
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message));
        }

        Message = message;
    }

    public string MemberName { get; }
    public string Message { get; }

    public override string ToString()
    {
        return $"[{MemberName}: {Message}]";
    }
}