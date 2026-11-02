using AQPlunch.Identity.Application.UseCases.Users.Commands;
using AQPlunch.Identity.Application.UseCases.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.Identity.Api.Controllers;

[ApiController]
[Authorize(Policy = "Administrador")]
[Route("api/usuarios")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetUsersQuery()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetUserByIdQuery { UserId = id }));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddUserCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
    {
        command.UserId = id;
        await _mediator.Send(command);

        return Ok(new { message = "Usuario actualizado" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteUserCommand { UserId = id });

        return Ok(new { message = "Usuario eliminado" });
    }

    [HttpPost("{id:guid}/roles/{roleId:guid}")]
    public async Task<IActionResult> AddRole(Guid id, Guid roleId)
    {
        await _mediator.Send(new AddUserRoleCommand { UserId = id, RoleId = roleId });

        return Ok(new { message = "Rol asignado al usuario" });
    }

    [HttpDelete("{id:guid}/roles/{roleId:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id, Guid roleId)
    {
        await _mediator.Send(new DeleteUserRoleCommand { UserId = id, RoleId = roleId });

        return Ok(new { message = "Rol quitado al usuario" });
    }
}
