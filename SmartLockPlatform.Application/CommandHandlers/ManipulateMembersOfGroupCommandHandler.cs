using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

internal class ManipulateMembersOfGroupCommandHandler : ICommandHandler<ManipulateMembersOfGroupCommand>
{
    private readonly IAppDbContext _dbContext;

    public ManipulateMembersOfGroupCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(ManipulateMembersOfGroupCommand command, CancellationToken cancellationToken)
    {
        var site = await _dbContext.Set<Site>()
            .Include(s => s.Members)
            .Include(s => s.Groups.Where(r => r.Id == command.GroupId))
            .ThenInclude(r => r.Members)
            .FindById(command.SiteId, cancellationToken);

        if (site is null) return Fail($"There is no site with given siteId [{command.SiteId}]");

        foreach (var memberId in command.NewMemberIds)
        {
            var result = site.AddMemberToGroup(command.GroupId, memberId);
            if (result.Failed) return result;
        }

        foreach (var memberId in command.RemovedMemberIds)
        {
            var result = site.RemoveMemberFromGroup(command.GroupId, memberId);
            if (result.Failed) return result;
        }

        await _dbContext.Complete(cancellationToken);

        return Ok();
    }
}