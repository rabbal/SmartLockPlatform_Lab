namespace SmartLockPlatform.Domain.Identity;

public class LockoutOptions
{
    public int MaxFailedLoginCount { get; set; } = 5;
    public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1);

    public static LockoutOptions CreateUnrestraint()
    {
        return new LockoutOptions
        {
            MaxFailedLoginCount = 999,
            Duration = TimeSpan.Zero
        };
    }
}