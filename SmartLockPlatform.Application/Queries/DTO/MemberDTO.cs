namespace SmartLockPlatform.Application.Queries.DTO;

public class MemberDTO
{
    public long Id { get; init; }
    public string? Alias { get; init; }
    public bool IsBlocked { get; init; }
    public IReadOnlyList<RoleDTO> Roles { get; init; } = Array.Empty<RoleDTO>();
    public UserDTO User { get; init; } = default!;
}

public class RoleDTO
{
    public long Id { get; init; }
    public string Name { get; init; } = default!;
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();
}