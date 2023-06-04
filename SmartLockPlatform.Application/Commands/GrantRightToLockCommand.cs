using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Application.Commands;

public record GrantRightToLockCommand : ICommand
{
    public long SiteId { get; init; }
    public long GroupId { get; init; }
    public long LockId { get; init; }
}