using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

internal class RegisterRoleCommandHandler : ICommandHandler<RegisterRoleCommand, Result<RoleDTO>>
{
    private readonly IAppDbContext _dbContext;

    public RegisterRoleCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<RoleDTO>> Handle(RegisterRoleCommand command, CancellationToken cancellationToken)
    {
        var site = await _dbContext.Set<Site>()
            .Include(s => s.Roles)
            .FindById(command.SiteId, cancellationToken);
        if (site is null) return Fail<RoleDTO>($"There is no site with given siteId [{command.SiteId}]");

        var result = site.RegisterRole(command.Name, command.Permissions);
        if (result.Failed) return Fail<RoleDTO>(result.Message);

        await _dbContext.Complete(cancellationToken);

        return new RoleDTO
        {
            Id = result.Data.Id,
            Name = command.Name,
            Permissions = command.Permissions,
        };
    }
}