namespace SmartLockPlatform.Application.Queries.DTO;

public class LockDTO
{
    public long Id { get; init; }
    public string Uuid { get; init; } = default!;
    public string Name { get; init; } = default!;
}