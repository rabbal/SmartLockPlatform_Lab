using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Commands;

public record RegisterMemberCommand : ICommand<Result<MemberDTO>>
{
    public long SiteId { get; init; }
    public long UserId { get; init; }
    public string? Alias { get; init; }
    public IEnumerable<long> RoleIds { get; init; } = Array.Empty<long>();
}