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

    public bool IsUsersEmpty()
    {
        if (Users.Count == 0)
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

    public Task<Lobby?> GetLobbyById(Guid id)
    {
        Lobbies.TryGetValue(id, out var lobby);
        return Task.FromResult(lobby);
    }


}