using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Host.Authorization;
using SmartLockPlatform.Host.Controllers.V1.Models;

namespace SmartLockPlatform.Host.Controllers.V1;

[ApiController, Route("api/v{version:apiVersion}/sites")]
[ApiVersion("1.0")]
public class SitesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ISiteQueries _queries;
    private readonly IUserIdentitySession _identitySession;

    public SitesController(
        IMediator mediator,
        IMapper mapper,
        ISiteQueries queries,
        IUserIdentitySession identitySession)
    {
        _mediator = mediator;
        _mapper = mapper;
        _queries = queries;
        _identitySession = identitySession;
    }

    // [HttpGet, PermissionAuthorize(PermissionNames.Sites_View_List_Permission)]
    // public Task<PaginatedList<SiteDTO>> GetSites([FromQuery] PaginatedListQueryParams parameters)
    // {
    //     return _mediator.Send(new ListSitesQuery
    //     {
    //         Filtering = parameters.Filtering,
    //         Sorting = parameters.Sorting,
    //         Skip = parameters.Skip,
    //         Top = parameters.Top,
    //         IncludeCount = parameters.IncludeCount,
    //         OwnerId = _identitySession.UserId
    //     });
    // }
    [HttpGet, PermissionAuthorize(PermissionNames.Sites_View_List_Permission)]
    public Task<PaginatedList<SiteDTO>> GetSites([FromQuery] PaginatedListQueryParams parameters)
    {
        return _queries.ListSites(new ListSitesRequest
        {
            Filtering = parameters.Filtering,
            Sorting = parameters.Sorting,
            Skip = parameters.Skip,
            Top = parameters.Top,
            IncludeCount = parameters.IncludeCount,
            OwnerId = _identitySession.UserId
        });
    }

    [HttpPost]
    //[PermissionAuthorize(PermissionNames.Sites_Register_Permission)]
    public async Task<IActionResult> RegisterSite([FromBody] RegisterSiteDTO model, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterSiteCommand>(model);

        var result = await _mediator.Send(command, cancellationToken);
        if (result.Failed) return BadRequest(result.Message);

        return Ok(result.Value);
    }

    // [HttpGet("{site_id:long}/members")]
    // public Task<PaginatedList<SiteMemberDTO>> GetMembers(
    //     [FromRoute(Name = "site_id")] long siteId,
    //     [FromQuery] PaginatedListQueryParams parameters)
    // {
    //     return _sender.Send(new ListSiteMembersRequest
    //     {
    //         Filtering = parameters.Filtering,
    //         Sorting = parameters.Sorting,
    //         Skip = parameters.Skip,
    //         Top = parameters.Top,
    //         IncludeCount = parameters.IncludeCount,
    //         SiteId = siteId
    //     });
    // }
    //
    // [HttpPost("{site_id:long}")]
    // public async Task<ActionResult> RegisterMember(
    //     [FromRoute(Name = "site_id")] long siteId,
    //     [FromBody] RegisterMemberCommand command,
    //     CancellationToken cancellationToken)
    // {
    //     //command.SiteId = siteId;
    //     
    //     var result = await _sender.Send(command, cancellationToken);
    //     if (result.Failed) return BadRequest(result);
    //
    //     return Ok();
    // }

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