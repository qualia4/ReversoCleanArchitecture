using System.Collections.Concurrent;
using Domain.Game;
using Reverso.Domain.Web;

namespace Reverso.Infrastructure;

public class DatabaseTemplate
{
    private static readonly ConcurrentDictionary<Guid, Lobby> Lobbies = new();
    private static readonly ConcurrentDictionary<string, User> Users = new();

    public void AddLobby(Lobby lobbyToAdd)
    {
        Lobbies[lobbyToAdd.GameId] = lobbyToAdd;
    }

    public void AddUser(User userToAdd)
    {
        Users[userToAdd.Username] = userToAdd;
    }

    public Task<User?> GetUserByUsername(string username)
    {
        Users.TryGetValue(username, out var user);
        return Task.FromResult(user);
    }

    public bool UserExists(string username)
    {
        if (Users.ContainsKey(username))
        {
            return true;
        }
        return false;
    }

    public bool LobbyExists(Guid lobbyID)
    {
        if (Lobbies.ContainsKey(lobbyID))
        {
            return true;
        }
        return false;
    }

    public bool IsUsersEmpty()
    {
        if (Users.Count == 0)
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

    public List<User> GetAllUsers()
    {
        List<User> result = new List<User>();
        foreach (var user in Users)
        {
            result.Add(user.Value);
        }
        return result;
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

}