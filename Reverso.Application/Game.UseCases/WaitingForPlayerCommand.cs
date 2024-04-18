using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class WaitingForPlayerCommand: IRequest<WaitingForPlayerResult>
{
    public Guid LobbyId { get; set; }
}

public class WaitingForPlayerResult
{
    public bool GameStarted { get; set; }
    public string Message { get; set; }
}

public class WaitingForPlayerUseCase : IRequestHandler<WaitingForPlayerCommand, WaitingForPlayerResult>
{
    private readonly ILobbyStorage lobbyStorage;

    public WaitingForPlayerUseCase(ILobbyStorage _lobbyStorage)
    {
        lobbyStorage = _lobbyStorage;
    }

    public async Task<WaitingForPlayerResult> Handle(WaitingForPlayerCommand request, CancellationToken cancellationToken)
    {
        if (!lobbyStorage.LobbyExists(request.LobbyId).Result)
        {
            return new WaitingForPlayerResult() {GameStarted = false, Message = "Invalid lobbyID"};
        }
        while(!lobbyStorage.IsStarted(request.LobbyId).Result){}
        return new WaitingForPlayerResult() {GameStarted = true, Message = "Success"};
    }
}