using Application;
using Reverso.Domain.Web;
namespace Reverso.Infrastructure;

public class LobbyStorage: ILobbyStorage
{
    private readonly DatabaseTemplate Database;

    public LobbyStorage(DatabaseTemplate _db)
    {
        Database = _db;
    }

    public async Task<bool> AddAsync(Lobby lobby)
    {
        Database.AddLobby(lobby);
        return true;
    }

    public async Task<bool> LobbyExists(Guid lobbyID)
    {
        return Database.LobbyExists(lobbyID);
    }

    public async Task<bool> IsLobbiesEmpty()
    {
        return Database.IsLobbiesEmpty();
    }

    public async Task<Lobby?> FindByLobbyIdAsync(Guid lobbyID)
    {
        return Database.GetLobbyById(lobbyID).Result;
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
}