using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SmartLockPlatform.Host.Authorization.PermissionBased;
using SmartLockPlatform.Host.Authorization.ResourceBased;

namespace SmartLockPlatform.Host.Authorization;

public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string name)
    {
        if (name.StartsWith(PermissionNames.Sites.Prefix, StringComparison.OrdinalIgnoreCase))
        {
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new SiteAuthorizationRequirement(name))
                .Build();
        }

        if (!name.StartsWith(PermissionConstants.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return await base.GetPolicyAsync(name);
        }

        var permissions = name[PermissionConstants.PolicyPrefix.Length..]
            .UnpackFromString(PermissionConstants.PolicyNameDelimiter);

        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionAuthorizationRequirement(permissions))
            .Build();
    }
}