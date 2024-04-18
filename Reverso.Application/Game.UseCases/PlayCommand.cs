using MediatR;
using Reverso.Domain.Web;
using Domain.Players;

namespace Application;

public class PlayCommand: IRequest<PlayResult>
{
    public string Username { get; set; }
    public string GameType { get; set; }
}

public class PlayResult
{
    public bool NewLobbyCreated { get; set; }
    public bool GameStarted { get; set; }
    public string Message { get; set; }
    public Guid LobbyId { get; set; }
}

public class PlayCommandUseCase : IRequestHandler<PlayCommand, PlayResult>
{
    private readonly IUserStorage userStorage;
    private readonly ILobbyStorage lobbyStorage;

    public PlayCommandUseCase(IUserStorage _userStorage, ILobbyStorage _lobbyStorage)
    {
        userStorage = _userStorage;
        lobbyStorage = _lobbyStorage;
    }

    public async Task<PlayResult> Handle(PlayCommand request, CancellationToken cancellationToken)
    {
        if (!userStorage.UserExists(request.Username).Result)
        {
            return new PlayResult {NewLobbyCreated = false, GameStarted = false, Message = "User does not exist. Log In first"};
        }
        if (request.GameType.ToLower() == "pve")
        {
            Lobby lobby = await CreatePvE(request.Username);
            return new PlayResult {NewLobbyCreated = true, GameStarted = true, LobbyId = lobby.GameId};
        }
        else if(request.GameType.ToLower() == "pvp")
        {
            var lobbyToJoin = lobbyStorage.FindLobbyToJoinAsync().Result;
            if (lobbyToJoin == null)
            {
                Lobby lobby = await CreatePvP(request.Username);
                return new PlayResult {NewLobbyCreated = true, GameStarted = false, LobbyId = lobby.GameId};
            }
            lobbyToJoin.AddPlayer(new LobbyPlayer(request.Username, new HumanPlayer(request.Username, new HumanMoveHandler())));
            lobbyToJoin.StartGame();
            return new PlayResult {GameStarted = true, LobbyId = lobbyToJoin.GameId};
        }
        return new PlayResult {NewLobbyCreated = false, GameStarted = false, Message = "Wrong game type"};
    }

    private async Task<Lobby> CreatePvE(string playerUsername)
    {
        Lobby lobby = new Lobby(false);
        lobby.AddPlayer(new LobbyPlayer(playerUsername, new HumanPlayer(playerUsername, new HumanMoveHandler())));
        lobby.AddPlayer(new LobbyPlayer("bot", new AIPlayer("BOT")));
        lobby.StartGame();
        await lobbyStorage.AddAsync(lobby);
        return lobby;
    }

    private async Task<Lobby> CreatePvP(string playerUsername)
    {
        Lobby lobby = new Lobby(true);
        lobby.AddPlayer(new LobbyPlayer(playerUsername, new HumanPlayer(playerUsername, new HumanMoveHandler())));
        await lobbyStorage.AddAsync(lobby);
        return lobby;
    }
}