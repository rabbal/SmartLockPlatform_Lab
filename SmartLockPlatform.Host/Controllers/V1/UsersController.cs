// using MediatR;
// using Microsoft.AspNetCore.Mvc;
//
// namespace SmartLockPlatform.Host.Controllers.V1;
//
// [ApiController, Route("api/v{version:apiVersion}/users")]
// [ApiVersion("1.0")]
// public class UsersController : ControllerBase
// {
//     private readonly ISender _sender;
//
//     public UsersController(ISender sender)
//     {
//         _sender = sender;
//     }
//
//     [HttpGet("search")]
//     public Task<IReadOnlyList<UserDTO>> Search([FromQuery(Name = "search_term")] string searchTerm,
//         CancellationToken cancellationToken)
//     {
//         return _sender.Send(new SearchUsersRequest(searchTerm), cancellationToken);
//     }
// }