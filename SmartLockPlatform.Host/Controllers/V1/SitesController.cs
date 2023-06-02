using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Host.Authorization;
using SmartLockPlatform.Host.Controllers.V1.Models;
using SmartLockPlatform.Infrastructure.Identity;

namespace SmartLockPlatform.Host.Controllers.V1;

[ApiController, Route("api/v{version:apiVersion}/sites")]
[ApiVersion("1.0")]
public class SitesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ISiteQueries _queries;

    public SitesController(
        IMediator mediator,
        IMapper mapper,
        ISiteQueries queries)
    {
        _mediator = mediator;
        _mapper = mapper;
        _queries = queries;
    }

    [HttpGet, Authorize(PermissionNames.Sites.View_List)]
    public Task<PaginatedList<SiteDTO>> GetSites([FromQuery] PaginatedListQueryParams parameters)
    {
        var request = _mapper.Map<ListSitesRequest>(parameters) with { OwnerId = User.GetUserId() };
        return _queries.ListSites(request);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterSite([FromBody] RegisterSiteDTO model, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterSiteCommand>(model) with { OwnerId = User.GetUserId() };

        var result = await _mediator.Send(command, cancellationToken);
        if (result.Failed) return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpGet("{site_id:long}/members")]
    [Authorize(PermissionNames.Sites.View_Members)]
    public Task<PaginatedList<MemberDTO>> GetMembers(
        [FromRoute(Name = "site_id")] long siteId,
        [FromQuery] PaginatedListQueryParams parameters
    )
    {
        var request = _mapper.Map<ListMembersRequest>(parameters) with { SiteId = siteId };
        return _queries.ListMembers(request);
    }

    [HttpPost("{site_id:long}/members")]
    [Authorize(PermissionNames.Sites.Register_Members)]
    public async Task<ActionResult> RegisterMember(
        [FromRoute(Name = "site_id")] long siteId,
        [FromBody] RegisterMemberDTO model,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterMemberCommand>(model) with
        {
            SiteId = siteId
        };

        var result = await _mediator.Send(command, cancellationToken);
        if (result.Failed) return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpGet("{site_id:long}/members/{member_id:long}")]
    [Authorize(PermissionNames.Sites.View_Members)]
    public Task<ActionResult> GetMember(
        [FromRoute(Name = "site_id")] long siteId,
        [FromRoute(Name = "member_id")] long memberId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // [HttpGet("{site_id}/entries"), PermissionAuthorize(PermissionNames.Sites_View_Entries_Permission)]
    // public IActionResult GetEntries([FromRoute(Name = "site_id")] long siteId)
    // {
    //     return Ok();
    // }
    //
    // [HttpGet("{site_id}/incidents"), PermissionAuthorize(PermissionNames.Sites_View_Incidents_Permission)]
    // public IActionResult GetIncidents([FromRoute(Name = "site_id")] long siteId)
    // {
    //     return Ok();
    // }
}