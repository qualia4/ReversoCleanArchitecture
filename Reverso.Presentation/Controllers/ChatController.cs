using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;

namespace Reverso.Presentation.Controllers;

[ApiController]
public class ChatController: ControllerBase
{
    private readonly IMediator mediator;

    public ChatController(IMediator _mediator)
    {
        this.mediator = _mediator;
    }

    [HttpGet("getChat/{lobbyId}")]
    public async Task<IActionResult> GetGameInfo([FromRoute] Guid lobbyId)
    {
        var result = await mediator.Send(new GetChatCommand{LobbyId = lobbyId});
        if (!result.LobbyExists) return BadRequest("Such lobby does not exist");
        return Ok(result.Chat);
    }

    [HttpPost("writeMessageToChat/{lobbyId}")]
    public async Task<IActionResult> WriteMessage([FromRoute] Guid lobbyId, [FromBody] SendMessageRequest request)
    {
        WriteMessageCommand command = new WriteMessageCommand()
            {LobbyId = lobbyId, Username = request.Username, Text = request.Text};
        var result = await mediator.Send(command);
        if(!result.LobbyExists) return BadRequest("Such lobby does not exist");
        if(!result.UserExists) return BadRequest("There is no such user in the lobby");
        return Ok("Success");
    }
}

public class SendMessageRequest
{
    public string Username { get; set; }
    public string Text { get; set; }
}