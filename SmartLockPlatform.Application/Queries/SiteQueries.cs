using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.Queries;

internal class SiteQueries : ISiteQueries
{
    private readonly IAppDbContext _dbContext;

    public SiteQueries(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PaginatedList<SiteDTO>> ListSites(ListSitesRequest request,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Site>()
            .WhereIf(request.OwnerId.HasValue,
                site => EF.Property<long>(site, nameof(request.OwnerId)) == request.OwnerId)
            .Select(site => new SiteDTO
            {
                Id = site.Id,
                Name = site.Name,
                Owner = new UserDTO
                {
                    Id = site.Owner.Id,
                    FirstName = site.Owner.FirstName.Value,
                    LastName = site.Owner.LastName.Value,
                    Email = site.Owner.Email.Value
                }
            })
            .ToPaginatedList(request, cancellationToken);
    }

    public Task<PaginatedList<MemberDTO>> ListMembers(ListMembersRequest request,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Member>()
            .Where(member => EF.Property<long>(member, nameof(request.SiteId)) == request.SiteId)
            .Select(member => new MemberDTO
            {
                Id = member.Id,
                Alias = member.Alias,
                User = new UserDTO
                {
                    Id = member.User.Id,
                    FirstName = member.User.FirstName.Value,
                    LastName = member.User.LastName.Value,
                    Email = member.User.Email.Value
                },
                Roles = member.Roles.Select(role => new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToList()
            })
            .ToPaginatedList(request, cancellationToken);
    }
}