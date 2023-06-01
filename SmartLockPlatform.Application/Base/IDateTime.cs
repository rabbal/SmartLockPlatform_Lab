namespace SmartLockPlatform.Application.Base;

public interface IDateTime
{
    /// <summary>
    /// Retrieves the current system time in UTC.
    /// </summary>
    DateTime Now { get; }
    DateTime Normalize(DateTime dateTime);
}

internal sealed class SystemDateTime : IDateTime
{
    //<inherit>
    public DateTime Now => DateTime.UtcNow;

    public DateTime Normalize(DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}