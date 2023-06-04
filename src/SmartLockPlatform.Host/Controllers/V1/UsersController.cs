using Microsoft.AspNetCore.Mvc;
using SmartLockPlatform.Application.Queries;
using SmartLockPlatform.Application.Queries.DTO;

namespace SmartLockPlatform.Host.Controllers.V1;

[ApiController, Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly IUserQueries _queries;

    public UsersController(IUserQueries queries)
    {
        _queries = queries;
    }

    [HttpGet("search")]
    public Task<IReadOnlyList<UserDTO>> Search([FromQuery(Name = "search_term")] string term,
        CancellationToken cancellationToken)
    {
        return _queries.Search(term, cancellationToken);
    }
}