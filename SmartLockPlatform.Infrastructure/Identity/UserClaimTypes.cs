using System.Security.Claims;

namespace SmartLockPlatform.Infrastructure.Identity;

public static class UserClaimTypes
{
    public const string Username = ClaimTypes.Name;
    public const string Email = ClaimTypes.Email;
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string SecurityStampToken = nameof(SecurityStampToken);
    public const string Role = ClaimTypes.Role;
    public const string DisplayName = nameof(DisplayName);
    public const string IsAdmin = nameof(IsAdmin);
    public const string System = ClaimTypes.System;
    public const string Permission = nameof(Permission);    
    public const string PackedPermission = nameof(PackedPermission);
}