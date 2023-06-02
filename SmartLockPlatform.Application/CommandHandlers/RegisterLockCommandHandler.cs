using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

internal class RegisterLockCommandHandler : ICommandHandler<RegisterLockCommand, Result<LockDTO>>
{
    private readonly IAppDbContext _dbContext;

    public RegisterLockCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<LockDTO>> Handle(RegisterLockCommand command, CancellationToken cancellationToken)
    {
        var site = await _dbContext.Set<Site>()
            .Include(site => site.Locks)
            .FindById(command.SiteId, cancellationToken);
        if (site is null) return Fail<LockDTO>($"There is no site with given siteId [{command.SiteId}].");

        if (await IsDuplicateLockWithUuid(command.Uuid, cancellationToken))
        {
            return Fail<LockDTO>("A lock with given uuid already exists.");
        }
        
        var result = site.AddLock(command.Name, command.Uuid);
        if (result.Failed) return Fail<LockDTO>(result.Message);

        await _dbContext.Complete(cancellationToken);

        return new LockDTO
        {
            Id = result.Data.Id,
            Uuid = result.Data.Uuid,
            Name = result.Data.Name
        };
    }
    
    private Task<bool> IsDuplicateLockWithUuid(string uuid, CancellationToken cancellationToken)
    {
        return _dbContext.Set<Lock>().AnyAsync(i => i.Uuid == uuid, cancellationToken);
    }
}