using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;

namespace SmartLockPlatform.Application.Queries;

public interface ISiteQueries
{
    Task<PaginatedList<SiteDTO>> ListSites(ListSitesRequest request, CancellationToken cancellationToken = default);

    Task<PaginatedList<MemberDTO>> ListMembers(ListMembersRequest request,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<LockDTO>> ListLocks(ListLocksRequest request, CancellationToken cancellationToken = default);
}

public record ListSitesRequest : PaginatedRequest
{
    public long? OwnerId { get; init; }
}

public record ListMembersRequest : PaginatedRequest
{
    public long SiteId { get; set; }
}

public record ListLocksRequest : PaginatedRequest
{
    public long SiteId { get; set; }
}