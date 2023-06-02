using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.External;
using SmartLockPlatform.Domain.Base;
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
        var site = await _dbContext.Set<Site>()
            .Include(s => s.Locks)
            .ThenInclude(l => l.Rights)
            .ThenInclude(r => r.Group)
            .FindById(command.SiteId, cancellationToken);

        return Ok();
    }
}