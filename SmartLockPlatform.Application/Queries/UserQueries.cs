using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Identity;

namespace SmartLockPlatform.Application.Queries;

internal class UserQueries : IUserQueries
{
    private readonly IAppDbContext _dbContext;

    public UserQueries(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<UserDTO>> Search(string term, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(term) || term.Length < 3)
        {
            return ArraySegment<UserDTO>.Empty;
        }

        return await _dbContext.Set<User>()
            .Where(u => u.IsActive)
            .Where(u => u.Email.Value.StartsWith(term)) //it uses gin indices
            .OrderBy(u => u.Email.Value)
            .Take(20)
            .Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName.Value,
                LastName = u.LastName.Value,
                Email = u.Email.Value
            }).ToListAsync(cancellationToken);
    }
}