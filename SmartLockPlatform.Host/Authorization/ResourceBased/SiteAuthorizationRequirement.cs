using Microsoft.AspNetCore.Authorization;
using SmartLockPlatform.Application.Authorization;
using SmartLockPlatform.Infrastructure.Identity;

namespace SmartLockPlatform.Host.Authorization.ResourceBased;

public class SiteAuthorizationHandler : AuthorizationHandler<SiteAuthorizationRequirement>
{
    private readonly IResourceProtector _protector;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SiteAuthorizationHandler(
        IResourceProtector protector,
        IHttpContextAccessor httpContextAccessor)
    {
        _protector = protector;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SiteAuthorizationRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var routeValue = httpContext.GetRouteValue("site_id")?.ToString();
        if (string.IsNullOrWhiteSpace(routeValue))
        {
            throw new InvalidOperationException("The route param with name [site_id] hasn't been found.");
        }

        if (!long.TryParse(routeValue, out var siteId))
        {
            throw new InvalidOperationException("The provided site_id route param is invalid.");
        }

        if (await _protector.IsGranted(context.User.GetUserId(), siteId, requirement.Permission,
                httpContext.RequestAborted))
        {
            context.Succeed(requirement);
        }
    }
}

public class SiteAuthorizationRequirement : IAuthorizationRequirement
{
    public SiteAuthorizationRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; init; }
}

public class PolicyNames
{
    public const string Owner = nameof(Owner);
}