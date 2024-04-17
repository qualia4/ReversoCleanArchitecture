using MediatR;
using Application;
using Microsoft.AspNetCore.Mvc;

namespace Reverso.Presentation.Controllers;

[ApiController]
public class AuthController: ControllerBase
{
    private Mediator mediator;
    [HttpPost("user")]
    public async Task<IActionResult> RegisterUser(string name)
    {
        return Ok("Not implemented");
    }
}