using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Identity;

namespace SmartLockPlatform.Infrastructure.Identity;

internal class UserIdentitySession : IUserIdentitySession
{
    private static readonly AsyncLocal<ClaimsPrincipal?> BackgroundPrincipal = new();
    private static readonly AsyncLocal<string?> BackgroundUserIP = new();

    private static readonly List<Claim> EmptyClaims = new();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdentitySession(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpContext? HttpContext => _httpContextAccessor.HttpContext;
    private ClaimsPrincipal? Principal => BackgroundPrincipal.Value ?? HttpContext?.User;

    public bool IsAdmin
    {
        get
        {
            var isAdmin = Principal?.FindFirstValue(UserClaimTypes.IsAdmin);
            return isAdmin is not null && bool.Parse(isAdmin);
        }
    }

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public long? UserId
    {
        get
        {
            var id = Principal?.FindFirstValue(UserClaimTypes.UserId);
            if (id is not null) return long.Parse(id);
            return null;
        }
    }

    public string? UserDisplayName => Principal?.FindFirstValue(UserClaimTypes.DisplayName);
    public string? UserBrowserName => HttpContext?.GetUserAgent();

    public string? UserIP => string.IsNullOrWhiteSpace(BackgroundUserIP.Value)
        ? HttpContext?.GetUserIP()
        : BackgroundUserIP.Value;

    public IReadOnlyList<Claim> Claims => Principal?.Claims.ToList() ?? EmptyClaims;

    [DoesNotReturn]
    public void ThrowIfUnauthenticated()
    {
        if (!IsAuthenticated)
        {
            throw new UserIsUnauthenticatedException("This operation need user authenticated");
        }
#pragma warning disable CS8763
    }
#pragma warning restore CS8763

    public IDisposable Use(IEnumerable<Claim> claims, string ip)
    {
        BackgroundUserIP.Value = ip;
        BackgroundPrincipal.Value = new ClaimsPrincipal(
            new ClaimsIdentity(
                claims,
                JwtBearerDefaults.AuthenticationScheme
            )
        );

        return new DisposableAction(() =>
        {
            BackgroundPrincipal.Value = null;
            BackgroundUserIP.Value = null;
        });
    }
}