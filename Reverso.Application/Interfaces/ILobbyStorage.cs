namespace Application;

using Reverso.Domain.Web;
using Domain.Game;

public interface ILobbyStorage
{
    Task AddAsync(Lobby lobby);
    Task<bool> LobbyExists(Guid lobbyID);
    Task<Cell[,]> GetFieldByLobbyId(Guid lobbyId);
    Task<bool> CheckIfEndedByLobbyId(Guid lobbyId);
    Task<string> GetCurrentPlayerNameByLobbyId(Guid lobbyId);
    Task<Dictionary<string, int>> GetPointsByLobbyId(Guid lobbyId);
    Task<bool> IsLobbiesEmpty();
    Task<bool> IsLobbyStarted(Guid lobbyID);
    Task<Lobby?> FindByLobbyIdAsync(Guid lobbyID);
    Task<List<Message>> GetChatByLobbyId(Guid lobbyId);
    Task<List<Lobby>> GetAllLobbies();
    Task<Lobby?> FindLobbyToJoinAsync();
    Task<bool> IsStarted(Guid lobbyId);
    Task<bool> AddMessageByLobbyId(Guid lobbyId, string username, string text);
}