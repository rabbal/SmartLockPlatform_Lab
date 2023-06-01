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
}