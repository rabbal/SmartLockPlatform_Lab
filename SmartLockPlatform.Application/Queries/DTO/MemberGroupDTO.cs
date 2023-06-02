namespace SmartLockPlatform.Application.Queries.DTO;

public class MemberGroupDTO
{
    public string Name { get; init; } = default!;
    public IReadOnlyList<MemberDTO> Members { get; init; } = Array.Empty<MemberDTO>();
}