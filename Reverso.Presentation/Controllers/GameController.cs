using MediatR;
using Application;

using Microsoft.AspNetCore.Mvc;

namespace Reverso.Presentation.Controllers;

[ApiController]
public class GameController: ControllerBase
{
    private readonly IMediator mediator;

    public GameController(IMediator _mediator)
    {
        this.mediator = _mediator;
    }

    [HttpPost("play")]
    public async Task<IActionResult> Play(PlayCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.GameStarted)
        {
            if (!result.NewLobbyCreated)
            {
                return Conflict(result.Message);
            }
        }
        return Ok(result);
    }
}