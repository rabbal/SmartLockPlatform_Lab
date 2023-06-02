using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Commands;

public record RegisterLockCommand : ICommand<Result<LockDTO>>
{
    public long SiteId { get; init; }
    public string Uuid { get; init; } = default!;
    public string Name { get; init; } = default!;
}