using Microsoft.AspNetCore.Authorization;

namespace SmartLockPlatform.Host.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Creates a new instance of <see cref="AuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="permissions">A list of permissions to authorize</param>
    public PermissionAuthorizeAttribute(params string[] permissions)
    {
        Policy = $"{PermissionConstants.PolicyPrefix}{permissions.PackToString(PermissionConstants.PolicyNameDelimiter)}";
    }
}