using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Commands;

public record RegisterMemberGroupCommand : ICommand<Result<MemberGroupDTO>>
{
    public long SiteId { get; init; }
    public string Name { get; init; } = default!;
}