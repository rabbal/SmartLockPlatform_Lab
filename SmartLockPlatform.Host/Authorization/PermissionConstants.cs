using System.Security.Claims;
using SmartLockPlatform.Infrastructure.Identity;

namespace SmartLockPlatform.Host.Authorization;

public static class PermissionConstants
{
    public const string PolicyPrefix = "Permission:";
    public const string PolicyNameDelimiter = ":";
    public const string PackingSymbol = ":";
    
    public static string PackToString(this IEnumerable<string> values, string packingSymbol)
    {
        return string.Join(packingSymbol, values);
    }
        
    public static IEnumerable<string> UnpackFromString(this string packedValues,string packingSymbol)
    {
        if (packedValues == null) throw new ArgumentNullException(nameof(packedValues));

        return packedValues.Split(new[] {packingSymbol}, StringSplitOptions.None);
    }
    
    public static IEnumerable<string> FindPermissions(this ClaimsPrincipal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        var permissions = principal.Claims
            .Where(c => c.Type.Equals(UserClaimTypes.Permission, StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Value)
            .ToList();

        var packedPermissions = principal.Claims.Where(c =>
                c.Type.Equals(UserClaimTypes.PackedPermission, StringComparison.OrdinalIgnoreCase))
            .SelectMany(c => c.Value.UnpackFromString(PackingSymbol));

        permissions.AddRange(packedPermissions);

        return permissions;
    }

    public static bool HasPermission(this ClaimsPrincipal principal, string permission)
    {
        return principal.FindPermissions().Any(p => p.Equals(permission, StringComparison.OrdinalIgnoreCase));
    }
}