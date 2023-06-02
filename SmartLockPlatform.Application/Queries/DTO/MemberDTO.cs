namespace SmartLockPlatform.Application.Queries.DTO;

public class MemberDTO
{
    public long Id { get; set; }
    public string? Alias { get; set; }
    public bool IsBlocked { get; set; }
    public IReadOnlyList<RoleDTO> Roles { get; set; }
    public UserDTO User { get; set; }
}

public class RoleDTO
{
    public long Id { get; init; }
    public string Name { get; init; } = default!;
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();
}