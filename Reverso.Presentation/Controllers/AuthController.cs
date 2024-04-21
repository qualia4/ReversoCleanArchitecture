using MediatR;
using Application;
using Microsoft.AspNetCore.Mvc;

namespace Reverso.Presentation.Controllers;

[ApiController]
public class AuthController: ControllerBase
{
    private readonly IMediator mediator;

    public AuthController(IMediator _mediator)
    {
        this.mediator = _mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.UserCreated) return Conflict(result.Message);
        else return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.UserExists) return Conflict(result.Message);
        if (!result.UserLogedIn) return Conflict(result.Message);
        else
        {
            return Ok(result);
        }
    }

    [HttpGet("getUserInfo/{username}")]
    public async Task<IActionResult> GetUser([FromRoute] string username)
    {
        var result = await mediator.Send(new GetUserInfoCommand{Username = username});
        if (!result.UserFound) return BadRequest("There is no such user in the database");
        else
        {
            return Ok(result.User);
        }
    }

    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await mediator.Send(new GetAllUsersCommand());
        if (result.UsersEmpty) return BadRequest("There are no users");
        else
        {
            return Ok(result.Users);
        }
    }
}