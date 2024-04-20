using Domain.Game;
using Domain.Abstractions;

namespace Reverso.Domain.Web;

public class Lobby
{
    public Guid GameId { get; set; }
    public ReversoGameWithEvents Game { get; private set; }
    public bool IsPublic;
    public bool IsStarted = false;
    public List<Message> Chat { get; private set; }
    public List<LobbyPlayer> Players { get; set; }

    public Lobby(bool isPublic)
    {
        GameId = Guid.NewGuid();
        Game = new ReversoGameWithEvents();
        IsPublic = isPublic;
        Players = new List<LobbyPlayer>();
        Chat = new List<Message>();
    }

    public void AddPlayer(LobbyPlayer player)
    {
        if (Players.Count() < 2)
        {
            Players.Add(player);
        }
        else
        {
            throw new Exception("Lobby already full");
        }
    }

    public void StartGame()
    {
        if (Players.Count() < 2)
        {
            throw new Exception("You need more players");
        }
        else
        {
            Player firstPlayer = Players[0].Player;
            Player secondPlayer = Players[1].Player;
            Game.StartGame(firstPlayer, secondPlayer);
            IsStarted = true;
        }
    }

    public async Task MakeMove()
    {
        if (IsStarted)
        {
            await Game.MakeMove();
        }
        else
        {
            throw new Exception("Game has not been started");
        }
    }
}

public class LobbyPlayer
{
    public string Username { get; set; }
    public Player Player { get; set; }

    public LobbyPlayer(string username, Player player)
    {
        Username = username;
        Player = player;
    }
}
