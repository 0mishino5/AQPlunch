using System.Security.Claims;
using AQPlunch.Identity.Application.Security;
using AQPlunch.Identity.Application.UseCases.Auth.Queries;
using AQPlunch.Identity.Application.UseCases.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.Identity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [Authorize]
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        return Ok(await _mediator.Send(new GetUserByIdQuery { UserId = Guid.Parse(User.FindFirstValue(NombreClaim.UserId)!) }));
    }
}
