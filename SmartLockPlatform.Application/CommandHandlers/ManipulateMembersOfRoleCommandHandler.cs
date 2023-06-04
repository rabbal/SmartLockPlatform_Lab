using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

internal class ManipulateMembersOfRoleCommandHandler : ICommandHandler<ManipulateMembersOfRoleCommand>
{
    private readonly IAppDbContext _dbContext;

    public ManipulateMembersOfRoleCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(ManipulateMembersOfRoleCommand command, CancellationToken cancellationToken)
    {
        var site = await _dbContext.Set<Site>()
            .Include(s => s.Members)
            .ThenInclude(m => m.Roles)
            .Include(s => s.Roles.Where(r => r.Id == command.RoleId))
            .ThenInclude(r => r.Members)
            .FindById(command.SiteId, cancellationToken);

        if (site is null) return Fail($"There is no site with given siteId [{command.SiteId}]");

        foreach (var memberId in command.NewMemberIds)
        {
            var result = site.AddMemberToRole(command.RoleId, memberId);
            if (result.Failed) return result;
        }

        foreach (var memberId in command.RemovedMemberIds)
        {
            var result = site.RemoveMemberFromRole(command.RoleId, memberId);
            if (result.Failed) return result;
        }

        await _dbContext.Complete(cancellationToken);

        return Ok();
    }
}