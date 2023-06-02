namespace SmartLockPlatform.Host.Controllers.V1.Models;

public class ManipulateMembersOfRoleDTO
{
    public IReadOnlyList<long> NewMemberIds { get; init; } = Array.Empty<long>();
    public IReadOnlyList<long> RemovedMemberIds { get; init; } = Array.Empty<long>();
}