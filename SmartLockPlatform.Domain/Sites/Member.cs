using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;

namespace SmartLockPlatform.Domain.Sites;

public sealed class Member : Entity
{
    private readonly List<Role> _roles = new();

#pragma warning disable CS8618
    private Member() //ORM
#pragma warning restore CS8618
    {
    }

    public Member(User user, string? alias)
    {
        User = user;
        Alias = alias;
    }

    public User User { get; init; }
    public bool IsBlocked { get; private set; }
    public string? BlockedReason { get; private set; }
    public string? Alias { get; init; }
    public bool TotpEnabled { get; set; }
    public string? TotpToken { get; set; }
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();

    public void Block(string reason)
    {
        IsBlocked = true;
        BlockedReason = reason;
    }

    public void UnBlock()
    {
        IsBlocked = false;
        BlockedReason = null;
    }

    public void EnableTotp()
    {
        
    }

    public void ResetTotpToken()
    {
        
    }
}