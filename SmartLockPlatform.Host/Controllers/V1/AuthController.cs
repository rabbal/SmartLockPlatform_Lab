using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartLockPlatform.Application.Commands;

namespace SmartLockPlatform.Host.Controllers.V1;

[ApiController, Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("sign-up")]
    public async Task<ActionResult> Register([FromBody] RegisterUserDTO model, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(model);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.Failed) return BadRequest(result.Message);

        return Ok();
    }
}