using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Application.Commands;

public record UnLockCommand : ICommand
{
    public long SiteId { get; init; }
    public long LockId { get; init; }
    public long UserId { get; set; }
    public string Otp { get; init; } = default!;
}