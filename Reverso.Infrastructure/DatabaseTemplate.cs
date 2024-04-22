using System.Collections.Concurrent;
using Application;
using Domain.Game;
using Reverso.Domain.Web;

namespace Reverso.Infrastructure;

public class DatabaseTemplate
{
    private static readonly ConcurrentDictionary<Guid, Lobby> Lobbies = new();

    public Task AddLobby(Lobby lobbyToAdd)
    {
        Lobbies[lobbyToAdd.GameId] = lobbyToAdd;
        return Task.CompletedTask;
    }

    public bool LobbyExists(Guid lobbyID)
    {
        if (Lobbies.ContainsKey(lobbyID))
        {
            return true;
        }
        return false;
    }

    public bool IsLobbiesEmpty()
    {
        if (Lobbies.Count == 0)
        {
            return true;
        }
        return false;
    }

    public List<Lobby> GetAllLobbies()
    {
        List<Lobby> result = new List<Lobby>();
        foreach (var lobby in Lobbies)
        {
            result.Add(lobby.Value);
        }
        return result;
    }

    public Task<Lobby?> GetLobbyById(Guid id)
    {
        Lobbies.TryGetValue(id, out var lobby);
        return Task.FromResult(lobby);
    }

    public Task<bool> IsLobbyStarted(Guid id)
    {
        Lobbies.TryGetValue(id, out var lobby);
        return Task.FromResult(lobby.IsStarted);
    }

    public Task<Lobby?> FindLobbyToJoin()
    {
        if (IsLobbiesEmpty())
        {
            return Task.FromResult<Lobby?>(null);
        }
        else
        {
            foreach (var lobby in Lobbies)
            {
                if (lobby.Value.IsPublic && !lobby.Value.IsStarted)
                {
                    return Task.FromResult(lobby.Value);
                }
            }
        }
        return Task.FromResult<Lobby?>(null);
    }

    public async Task<Cell[,]> GetFieldByLobbyId(Guid lobbyId)
    {
        Lobbies.TryGetValue(lobbyId, out var lobby);
        return await lobby.GetField();
    }

    public async Task<Dictionary<string, int>> GetPointsByLobbyId(Guid lobbyId)
    {
        Lobbies.TryGetValue(lobbyId, out var lobby);
        return await lobby.GetPoints();
    }

    public async Task<bool> CheckIfEndedByLobbyId(Guid lobbyId)
    {
        Lobbies.TryGetValue(lobbyId, out var lobby);
        return await lobby.GetEnded();
    }

    public async Task<List<Message>> GetChatByLobbyId(Guid lobbyId)
    {
        Lobbies.TryGetValue(lobbyId, out var lobby);
        return lobby.Chat;
    }

    public async Task<string> GetCurrentPlayerNameByLobbyId(Guid lobbyId)
    {
        Lobbies.TryGetValue(lobbyId, out var lobby);
        return await lobby.GetCurrentPlayerName();
    }

    public async Task<bool> AddMessageByLobbyid(Guid lobbyId, string username, string text)
    {
        Lobbies.TryGetValue(lobbyId, out var lobby);
        return await lobby.AddMessage(username, text);
    }

}