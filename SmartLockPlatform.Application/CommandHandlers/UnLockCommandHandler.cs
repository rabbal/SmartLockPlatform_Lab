using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.External;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

public class UnLockCommandHandler : ICommandHandler<UnLockCommand>
{
    private readonly ISmartMqttClient _client;
    private readonly IAppDbContext _dbContext;

    public UnLockCommandHandler(ISmartMqttClient client, IAppDbContext dbContext)
    {
        _client = client;
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UnLockCommand command, CancellationToken cancellationToken)
    {
        //TODO: CHECK TOTP

        var site = await _dbContext.Set<Site>()
            .Include(s => s.Members)
            .ThenInclude(m => m.User)
            .Include(s => s.Owner)
            .Include(s => s.Locks.Where(l => l.Id == command.LockId))
            .ThenInclude(l => l.Rights)
            .ThenInclude(r => r.Group)
            .ThenInclude(g => g.Members)
            .AsSplitQuery()
            .FindById(command.SiteId, cancellationToken);

        if (site is null) return Fail($"There is no site with given siteId [{command.SiteId}]");

        var result = site.CanUnLock(command.LockId, command.UserId);
        if (result.Failed)
        {
            return result;
        }

        var @lock = site.Locks.Single(l => l.Id == command.LockId);

        await _client.UnLock(new UnLockDTO
        {
            SiteId = command.SiteId,
            LockId = command.LockId,
            LockUuid = @lock.Uuid,
            UserId = command.UserId,
            IdempotentId = Guid.NewGuid().ToString("N")
        }, cancellationToken);

        return Ok();
    }
}