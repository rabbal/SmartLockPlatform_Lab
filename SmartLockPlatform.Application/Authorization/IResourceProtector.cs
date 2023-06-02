using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Application.Authorization;

public interface IResourceProtector
{
    Task<bool> IsOwnerOf(long userId, long siteId, CancellationToken cancellationToken = default);
    Task<bool> IsGranted(long userId, long siteId, string permission, CancellationToken cancellationToken = default);
}

internal class ResourceProtector : IResourceProtector
{
    private readonly IAppDbContext _dbContext;

    public ResourceProtector(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> IsOwnerOf(long userId, long siteId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Site>()
            .AnyAsync(site => site.Id == siteId && EF.Property<long>(site, "OwnerId") == userId, cancellationToken);
    }

    public async Task<bool> IsGranted(long userId, long siteId, string permission, CancellationToken cancellationToken)
    {
        var isOwner = await _dbContext.Set<Site>()
            .AnyAsync(site => site.Id == siteId && EF.Property<long>(site, "OwnerId") == userId, cancellationToken);

        return isOwner || await _dbContext.Set<Member>()
            .Where(member => !member.IsBlocked &&
                             EF.Property<long>(member, "UserId") == userId &&
                             EF.Property<long>(member, "SiteId") == siteId)
            .SelectMany(member => member.Roles)
            .AnyAsync(role => role.Permissions.Any(p => p.Name == permission), cancellationToken);
    }
}