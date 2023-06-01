using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;

namespace SmartLockPlatform.Application.Queries;

public interface ISiteQueries
{
    Task<PaginatedList<SiteDTO>> ListSites(ListSitesRequest request, CancellationToken cancellationToken = default);
}

public class ListSitesRequest : PaginatedRequest
{
    public long? OwnerId { get; set; }
}