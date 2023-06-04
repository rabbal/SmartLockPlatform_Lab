using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Application.Commands;

public record ManipulateMembersOfGroupCommand : ICommand
{
    public long SiteId { get; init; }
    public long GroupId { get; init; }
    public IReadOnlyList<long> NewMemberIds { get; init; } = Array.Empty<long>();
    public IReadOnlyList<long> RemovedMemberIds { get; init; } = Array.Empty<long>();
}