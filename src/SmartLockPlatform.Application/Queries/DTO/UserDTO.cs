namespace SmartLockPlatform.Application.Queries.DTO;

public class UserDTO
{
    public long Id { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
}