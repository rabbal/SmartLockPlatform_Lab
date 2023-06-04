using SmartLockPlatform.Application.Queries.DTO;

namespace SmartLockPlatform.Application.Queries;

public interface IUserQueries
{
    Task<IReadOnlyList<UserDTO>> Search(string term, CancellationToken cancellationToken = default);
}