namespace Application;

using Reverso.Domain.Web;

public interface ILobbyStorage
{
    Task<bool> AddAsync(Lobby lobby);
    Task<bool> LobbyExists(Guid lobbyID);
    Task<bool> IsLobbiesEmpty();
    Task<Lobby?> FindByLobbyIdAsync(Guid lobbyID);
    Task<List<Lobby>> GetAllLobbies();
    Task<Lobby?> FindLobbyToJoinAsync();
    Task<bool> IsStarted(Guid lobbyId);
}