namespace SmartLockPlatform.Host.Controllers.V1.Models;

public record RegisterMemberDTO(long UserId, string? Alias, IEnumerable<long> RoleIds);