using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;

namespace SmartLockPlatform.Application.Queries;

public interface ISiteQueries
{
    Task<PaginatedList<SiteDTO>> ListSites(long ownerId, IPaginatedRequest request,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<MemberDTO>> ListMembers(long siteId, IPaginatedRequest request,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<LockDTO>> ListLocks(long siteId, IPaginatedRequest request,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<RoleDTO>> ListRoles(long siteId, IPaginatedRequest request,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<MemberGroupDTO>> ListMemberGroups(long siteId, IPaginatedRequest request,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<MemberDTO>> ListMembersInGroup(long siteId, long groupId, IPaginatedRequest request,
        CancellationToken cancellationToken = default);
}