namespace SmartLockPlatform.Domain.Identity;

public class IdentityOptions
{
    public PasswordOptions Password { get; set; } = new();
    public LockoutOptions Lockout { get; set; } = new();

    public static IdentityOptions CreateUnrestraint()
    {
        return new IdentityOptions
        {
            Password = PasswordOptions.CreateUnrestraint(),
            Lockout = LockoutOptions.CreateUnrestraint()
        };
    }
}