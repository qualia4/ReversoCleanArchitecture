using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class GetAllLobbiesCommand: IRequest<GetAllLobbiesResult>
{

}

public class GetAllLobbiesResult
{
    public bool LobbiesEmpty { get; set; }
    public List<Lobby> Lobbies { get; set; }
}

public class GetAllLobbiesUseCase : IRequestHandler<GetAllLobbiesCommand, GetAllLobbiesResult>
{
    private readonly ILobbyStorage lobbyStorage;

    public GetAllLobbiesUseCase(ILobbyStorage _lobbyStorage)
    {
        lobbyStorage = _lobbyStorage;
    }

    public async Task<GetAllLobbiesResult> Handle(GetAllLobbiesCommand request, CancellationToken cancellationToken)
    {
        if (await lobbyStorage.IsLobbiesEmpty())
        {
            return new GetAllLobbiesResult() {LobbiesEmpty = true};
        }
        return new GetAllLobbiesResult() {LobbiesEmpty = false, Lobbies = lobbyStorage.GetAllLobbies().Result};
    }
}