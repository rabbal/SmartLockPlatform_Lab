using System.Security.Claims;
using SmartLockPlatform.Application.Identity;

namespace SmartLockPlatform.Infrastructure.Identity;

public static class PrincipalExtensions
{
    /// <summary>
    /// Returns the value for the first claim of the specified type, otherwise null if the claim is not present.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance this method extends.</param>
    /// <param name="claimType">The claim type whose first value should be returned.</param>
    /// <returns>The value of the first instance of the specified claim type, or null if the claim is not present.</returns>
    public static string? FindFirstValue(this ClaimsPrincipal principal, string claimType)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst(claimType);
        return claim?.Value;
    }

    public static long GetUserId(this ClaimsPrincipal principal)
    {
        var id = principal.FindFirstValue(UserClaimTypes.UserId);
        return id is not null ? long.Parse(id) : default;
    }
}