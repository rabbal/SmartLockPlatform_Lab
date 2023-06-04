using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

public class GrantRightToLockCommandHandler : ICommandHandler<GrantRightToLockCommand>
{
    private readonly IAppDbContext _dbContext;

    public GrantRightToLockCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(GrantRightToLockCommand command, CancellationToken cancellationToken)
    {
        var site = await _dbContext.Set<Site>()
            .Include(s => s.Groups.Where(g => g.Id == command.GroupId))
            .Include(s => s.Locks.Where(l => l.Id == command.LockId))
            .ThenInclude(l => l.Rights)
            .ThenInclude(lr => lr.Group)
            .Include(s => s.Members)
            .AsSplitQuery()
            .FindById(command.SiteId, cancellationToken);
        if (site is null) return Fail<RoleDTO>($"There is no site with given siteId [{command.SiteId}]");

        var result = site.GrantRightToLock(command.LockId, command.GroupId, Timeframe.Always);
        if (result.Failed) return result;

        await _dbContext.Complete(cancellationToken);

        return Ok();
    }
}