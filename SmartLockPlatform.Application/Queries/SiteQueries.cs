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

    public Task<PaginatedList<SiteDTO>> ListSites(long ownerId, IPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Site>()
            .Where(site => EF.Property<long>(site, "OwnerId") == ownerId)
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

    public Task<PaginatedList<MemberDTO>> ListMembers(long siteId, IPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Member>()
            .Where(member => EF.Property<long>(member, "SiteId") == siteId)
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
            .AsSingleQuery()
            .ToPaginatedList(request, cancellationToken);
    }

    public Task<PaginatedList<LockDTO>> ListLocks(long siteId, IPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Lock>()
            .Where(item => EF.Property<long>(item, "SiteId") == siteId)
            .Select(item => new LockDTO
            {
                Id = item.Id,
                Uuid = item.Uuid,
                Name = item.Name
            })
            .ToPaginatedList(request, cancellationToken);
    }

    public Task<PaginatedList<RoleDTO>> ListRoles(long siteId, IPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Role>()
            .Where(role => EF.Property<long>(role, "SiteId") == siteId)
            .Select(role => new RoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.Permissions.Select(p => p.Name).ToList()
            })
            .ToPaginatedList(request, cancellationToken);
    }

    public Task<PaginatedList<MemberGroupDTO>> ListMemberGroups(long siteId, IPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<MemberGroup>()
            .Where(group => EF.Property<long>(group, "SiteId") == siteId)
            .Select(group => new MemberGroupDTO
            {
                Id = group.Id,
                Name = group.Name,
            })
            .ToPaginatedList(request, cancellationToken);
    }

    public Task<PaginatedList<MemberDTO>> ListMembersInGroup(long siteId, long groupId, IPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<MemberGroup>()
            .Where(group => EF.Property<long>(group, "SiteId") == siteId)
            .SelectMany(group => group.Members)
            .Select(member => new MemberDTO
            {
                Id = member.Id,
                Alias = member.Alias,
                IsBlocked = member.IsBlocked,
                Roles = member.Roles.Select(role => new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = role.Permissions.Select(p => p.Name).ToList()
                }).ToList(),
                User = new UserDTO
                {
                    Id = member.User.Id,
                    FirstName = member.User.FirstName.Value,
                    LastName = member.User.LastName.Value,
                    Email = member.User.Email.Value
                }
            })
            .AsSingleQuery()
            .ToPaginatedList(request, cancellationToken);
    }
}