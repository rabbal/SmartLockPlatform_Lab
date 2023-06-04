namespace SmartLockPlatform.Application.Queries.DTO;

public class SiteDTO
{
    public long Id { get; init; }
    public string Name { get; init; } = default!;
    public UserDTO Owner { get; init; } = default!;
}