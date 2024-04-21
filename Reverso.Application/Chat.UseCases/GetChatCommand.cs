using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class GetChatCommand: IRequest<GetChatResult>
{
    public Guid LobbyId { get; set; }
}

public class GetChatResult
{
    public bool LobbyExists { get; set; }
    public List<Message> Chat { get; set; }
}

public class GetChatUseCase : IRequestHandler<GetChatCommand, GetChatResult>
{
    private readonly ILobbyStorage lobbyStorage;

    public GetChatUseCase(ILobbyStorage _lobbyStorage)
    {
        lobbyStorage = _lobbyStorage;
    }

    public async Task<GetChatResult> Handle(GetChatCommand request, CancellationToken cancellationToken)
    {
        if (!await lobbyStorage.LobbyExists(request.LobbyId))
        {
            return new GetChatResult(){LobbyExists = false};
        }
        return new GetChatResult() {LobbyExists = true, Chat = await lobbyStorage.GetChatByLobbyId(request.LobbyId)};
    }
}