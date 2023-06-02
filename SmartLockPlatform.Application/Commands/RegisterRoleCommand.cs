using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Commands;

public record RegisterRoleCommand : ICommand<Result<RoleDTO>>
{
    public long SiteId { get; init; }
    public string Name { get; init; } = default!;
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();
}