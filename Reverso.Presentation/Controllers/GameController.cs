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

    [HttpPost("makeMove")]
    public async Task<IActionResult> MakeMove(MakeMoveCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("lobbies")]
    public async Task<IActionResult> GetAllLobbies()
    {
        var result = await mediator.Send(new GetAllLobbiesCommand());
        if (result.LobbiesEmpty) return BadRequest("There are no lobbies");
        return Ok(result.Lobbies);
    }

    [HttpGet("waitingForPlayer/{lobbyId}")]
    public async Task<IActionResult> WaitingForPlayer([FromRoute]Guid lobbyId)
    {
        var result = await mediator.Send(new WaitingForPlayerCommand{LobbyId = lobbyId});
        if (!result.GameStarted) return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpGet("getGameInfo/{lobbyId}")]
    public async Task<IActionResult> GetGameInfo([FromRoute] Guid lobbyId)
    {
        var result = await mediator.Send(new GetGameInfoCommand{LobbyId = lobbyId});
        if (!result.LobbyExists) return BadRequest("Such lobby does not exist");
        if (!result.LobbyStarted) return BadRequest("Lobby is not started");
        return Ok(result);
    }
}