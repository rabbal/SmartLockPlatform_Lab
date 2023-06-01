using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace SmartLockPlatform.Host.Authorization;

public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string name)
    {
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