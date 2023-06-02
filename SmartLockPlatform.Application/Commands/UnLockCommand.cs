using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Application.Commands;

public class UnLockCommand : ICommand
{
    public long SiteId { get; init; }
    public long LockId { get; init; }
    public string Otp { get; init; } = default!;
}