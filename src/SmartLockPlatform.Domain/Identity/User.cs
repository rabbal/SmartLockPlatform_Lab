using System.Diagnostics.CodeAnalysis;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Identity;

public class User : AggregateRoot
{
#pragma warning disable CS8618
    protected User() //ORM
#pragma warning restore CS8618
    {
    }

    public User(FirstName firstName, LastName lastName, Email email, bool isAdmin)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        IsAdmin = isAdmin;
        LockoutEnabled = true;
        IsActive = true; //TODO: must be false by default and activate through email confirmation
        SetStampToken();
    }
    public FirstName FirstName { get; init; }
    public LastName LastName { get; init; }
    public Email Email { get; init; }

    /// <summary>
    /// Gets or sets a salted and hashed representation of the password.
    /// </summary>
    public string? PasswordHash { get; private set; }

    public bool IsActive { get; private set; }
    public bool IsAdmin { get; private set; }
    public bool LockoutEnabled { get; init; }
    public DateTime? LockoutEnd { get; private set; }
    public int FailedLoginCount { get; private set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password, permissions, roles, ...)
    /// </summary>
    public string SecurityStampToken { get; private set; }

    public bool IsLocked(DateTime now)
    {
        if (LockoutEnabled == false) return false;

        return LockoutEnd > now;
    }

    public void Lock(LockoutOptions options, DateTime now)
    {
        if (!LockoutEnabled) return;

        if (FailedLoginCount + 1 > options.MaxFailedLoginCount)
        {
            LockoutEnd = now.Add(options.Duration);
            return;
        }

        FailedLoginCount++;
    }

    public void ResetLockout()
    {
        FailedLoginCount = 0;
        LockoutEnd = null;
    }


    public Result Deactivate()
    {
        if (IsAdmin) return Fail("Deactivating an admin account is not allowed.");

        IsActive = false;
        SetStampToken();

        return Ok();
    }

    public void Activate()
    {
        IsActive = true;
        SetStampToken();
    }

    public void SetPassword(Password password, IPasswordHashAlgorithm passwordHashAlgorithm)
    {
        PasswordHash = passwordHashAlgorithm.HashPassword(password.Value);
        SetStampToken();
    }

    public void GrantAdminRights()
    {
        IsAdmin = true;
        SetStampToken();
    }

    public void RevokeAdminRights()
    {
        IsAdmin = false;
        SetStampToken();
    }

    [MemberNotNull(nameof(SecurityStampToken))]
    private void SetStampToken()
    {
        SecurityStampToken = Guid.NewGuid().ToString("N");
    }

    public void LogoutEverywhere()
    {
        SetStampToken();
    }
}