using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Commands;

public record RegisterMemberCommand
    (long SiteId, long UserId, string? Alias, IEnumerable<long> RoleIds) : ICommand<Result<MemberDTO>>;