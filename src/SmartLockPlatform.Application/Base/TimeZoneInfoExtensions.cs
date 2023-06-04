namespace SmartLockPlatform.Application.Base;

public static class TimeZone
{
    private static readonly Dictionary<string, TimeZoneInfo> Timezones = TimeZoneInfo.GetSystemTimeZones()
        .ToDictionary(tz => tz.Id);

    public static TimeZoneInfo? Find(string timezoneId)
    {
        return Timezones.TryGetValue(timezoneId, out var timezone) ? timezone : null;
    }
}