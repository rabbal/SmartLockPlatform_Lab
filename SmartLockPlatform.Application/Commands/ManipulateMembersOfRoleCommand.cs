using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Application.Commands;

public record ManipulateMembersOfRoleCommand : ICommand
{
    public long SiteId { get; init; }
    public long RoleId { get; init; }
    public IReadOnlyList<long> NewMemberIds { get; init; } = Array.Empty<long>();
    public IReadOnlyList<long> RemovedMemberIds { get; init; } = Array.Empty<long>();
}