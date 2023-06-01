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
    private readonly IUserIdentitySession _identitySession;

    public RegisterSiteCommandHandler(IAppDbContext dbContext, IUserIdentitySession identitySession)
    {
        _dbContext = dbContext;
        _identitySession = identitySession;
    }

    public async Task<Result<SiteDTO>> Handle(RegisterSiteCommand request, CancellationToken cancellationToken)
    {
        _identitySession.ThrowIfUnauthenticated(); //TODO: move into decorator

        var user = await _dbContext.Set<User>().FindById(_identitySession.UserId.Value, cancellationToken);
        if (user is null) return Fail<SiteDTO>("The current user hasn't been found in database.");

        var site = new Site(request.Name, user);

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
}