namespace SmartLockPlatform.Host.Controllers.V1.Models;

public class RegisterRoleDTO
{
    public string Name { get; init; } = default!;
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();
}