using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

internal class RegisterMemberCommandHandler : ICommandHandler<RegisterMemberCommand, Result<MemberDTO>>
{
    private readonly IAppDbContext _dbContext;

    public RegisterMemberCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<MemberDTO>> Handle(RegisterMemberCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().FindById(command.UserId, cancellationToken);
        if (user is null) return Fail<MemberDTO>($"There is no user with given userId [{command.UserId}]");

        var site = await _dbContext.Set<Site>()
            .Include(s => s.Members)
            .Include(s => s.Roles)
            .FindById(command.SiteId, cancellationToken);
        if (site is null) return Fail<MemberDTO>($"There is no site with given siteId [{command.SiteId}]");

        var result = site.RegisterMember(user, command.Alias, command.RoleIds);
        if (result.Failed) return Fail<MemberDTO>(result.Message);

        await _dbContext.Complete(cancellationToken);

        var member = result.Data;
        return new MemberDTO
        {
            Id = member.Id,
            Alias = member.Alias,
            IsBlocked = member.IsBlocked,
            Roles = member.Roles.Select(role => new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            }).ToList(),
            User = new UserDTO
            {
                Id = member.User.Id,
                FirstName = member.User.FirstName.Value,
                LastName = member.User.LastName.Value,
                Email = member.User.Email.Value
            }
        };
    }
}