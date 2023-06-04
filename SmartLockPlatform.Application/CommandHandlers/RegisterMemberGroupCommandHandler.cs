using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.CommandHandlers;

internal class RegisterMemberGroupCommandHandler : ICommandHandler<RegisterMemberGroupCommand, Result<MemberGroupDTO>>
{
    private readonly IAppDbContext _dbContext;

    public RegisterMemberGroupCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<MemberGroupDTO>> Handle(RegisterMemberGroupCommand command,
        CancellationToken cancellationToken)
    {
        var site = await _dbContext.Set<Site>()
            .Include(s => s.Groups)
            .FindById(command.SiteId, cancellationToken);
        if (site is null) return Fail<MemberGroupDTO>($"There is no site with given siteId [{command.SiteId}]");

        var result = site.AddMemberGroup(command.Name);
        if (result.Failed) return Fail<MemberGroupDTO>(result.Message);

        await _dbContext.Complete(cancellationToken);

        return new MemberGroupDTO
        {
            Id = result.Data.Id,
            Name = command.Name
        };
    }
}