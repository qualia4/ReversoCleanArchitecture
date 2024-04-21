using Application;
using Reverso.Domain.Web;
using Domain.Game;
namespace Reverso.Infrastructure;

public class LobbyStorage: ILobbyStorage
{
    private readonly DatabaseTemplate Database;

    public LobbyStorage(DatabaseTemplate _db)
    {
        Database = _db;
    }

    public async Task AddAsync(Lobby lobby)
    {
        await Database.AddLobby(lobby);
    }

    public async Task<bool> LobbyExists(Guid lobbyID)
    {
        return Database.LobbyExists(lobbyID);
    }

    public async Task<bool> IsLobbiesEmpty()
    {
        return Database.IsLobbiesEmpty();
    }

    public async Task<bool> IsLobbyStarted(Guid lobbyID)
    {
        return await Database.IsLobbyStarted(lobbyID);
    }

    public async Task<Lobby?> FindByLobbyIdAsync(Guid lobbyID)
    {
        return await Database.GetLobbyById(lobbyID);
    }

    public async Task<List<Lobby>> GetAllLobbies()
    {
        return Database.GetAllLobbies();
    }

    public Task<Lobby?> FindLobbyToJoinAsync()
    {
        return Database.FindLobbyToJoin();
    }

    public Task<bool> IsStarted(Guid lobbyId)
    {
        return Database.IsLobbyStarted(lobbyId);
    }

    public async Task<Cell[,]> GetFieldByLobbyId(Guid lobbyId)
    {
        return await Database.GetFieldByLobbyId(lobbyId);
    }

    public async Task<Dictionary<string, int>> GetPointsByLobbyId(Guid lobbyId)
    {
        return await Database.GetPointsByLobbyId(lobbyId);
    }

    public async Task<bool> CheckIfEndedByLobbyId(Guid lobbyId)
    {
        return await Database.CheckIfEndedByLobbyId(lobbyId);
    }

    public async Task<List<Message>> GetChatByLobbyId(Guid lobbyId)
    {
        return await Database.GetChatByLobbyId(lobbyId);
    }

    public async Task<string> GetCurrentPlayerNameByLobbyId(Guid lobbyId)
    {
        return await Database.GetCurrentPlayerNameByLobbyId(lobbyId);
    }

    public async Task<bool> AddMessageByLobbyId(Guid lobbyId, string username, string text)
    {
        return await Database.AddMessageByLobbyid(lobbyId, username, text);
    }
}