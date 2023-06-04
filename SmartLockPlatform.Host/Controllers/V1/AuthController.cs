using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Host.Authentication;
using SmartLockPlatform.Host.Controllers.V1.Models;

namespace SmartLockPlatform.Host.Controllers.V1;

[ApiController, Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IOptions<TokenOptions> _tokenOptions;

    public AuthController(IMediator mediator, IMapper mapper, IOptions<TokenOptions> tokenOptions)
    {
        _mediator = mediator;
        _mapper = mapper;
        _tokenOptions = tokenOptions;
    }

    [HttpPost("sign-up"), AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] RegisterUserDTO model, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(model);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.Failed) return Problem(result.Message, statusCode: StatusCodes.Status400BadRequest);

        return Ok();
    }

    [HttpPost("token"), AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] LoginDTO model, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginCommand>(model);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.Failed) return Problem(result.Message, statusCode: StatusCodes.Status400BadRequest);

        var claims = result.Data;
        var token = Token.Create(_tokenOptions.Value, claims);

        return Ok(token);
    }
}