using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

internal class RegisterSiteCommandHandler : ICommandHandler<RegisterSiteCommand, Result<SiteDTO>>
{
    private readonly IAppDbContext _dbContext;

    public RegisterSiteCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SiteDTO>> Handle(RegisterSiteCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().FindById(command.OwnerId, cancellationToken);
        if (user is null) return Fail<SiteDTO>("The current user hasn't been found in database.");

        if (await IsDuplicateSiteWithName(command.Name, cancellationToken))
        {
            return Fail<SiteDTO>("A site with given name is already exists.");
        }

        var site = new Site(command.Name, user);

        _dbContext.Set<Site>().Add(site);
        await _dbContext.Complete(cancellationToken);

        //TODO: it seems we are violating CQS
        return new SiteDTO
        {
            Id = site.Id,
            Name = site.Name,
            Owner = new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName.Value,
                LastName = user.LastName.Value,
                Email = user.Email.Value
            }
        };
    }

    private Task<bool> IsDuplicateSiteWithName(string name, CancellationToken cancellationToken)
    {
        return _dbContext.Set<Site>().AnyAsync(i => i.Name == name, cancellationToken);
    }
}