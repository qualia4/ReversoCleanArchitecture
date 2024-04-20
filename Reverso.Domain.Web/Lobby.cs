using Domain.Game;
using Domain.Abstractions;

namespace Reverso.Domain.Web;

public class Lobby
{
    public Guid GameId { get; set; }
    public ReversoGameWithEvents Game { get; private set; }
    public readonly bool IsPublic;
    public bool IsStarted { get; private set; }
    public List<Message> Chat { get; private set; }
    public List<LobbyPlayer> Players { get; set; }

    public Lobby(bool isPublic)
    {
        GameId = Guid.NewGuid();
        Game = new ReversoGameWithEvents();
        IsPublic = isPublic;
        IsStarted = false;
        Players = new List<LobbyPlayer>();
        Chat = new List<Message>();
        ListenTo();
    }

    private void ListenTo()
    {
        Game.GameEnded += OnGameEnded;
        Game.CanMakeMove += OnCanMakeMove;
    }

    public void AddPlayer(LobbyPlayer player)
    {
        switch (Players.Count())
        {
            case 0:
                Players.Add(player);
                return;
            case 1:
                if (Players[0].Username != player.Username)
                {
                    Players.Add(player);
                    return;
                }
                throw new Exception("You can not play with yourself");
            case 2:
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
            IsStarted = true;
            Game.StartGame(firstPlayer, secondPlayer);
        }
    }

    private async void OnCanMakeMove()
    {
        if (IsStarted)
        {
            await Game.MakeMove();
        }
        else
        {
            throw new Exception("Lobby has not been started");
        }
    }

    private void OnGameEnded((string, int)? winner)
    {
        throw new NotImplementedException();
    }

    public Task<Cell[,]> GetField()
    {
        return Task.FromResult(Game.GetField());
    }

    public Task<Dictionary<string, int>> GetPoints()
    {
        return Task.FromResult(Game.GetPoints());
    }

    public Task<bool> GetEnded()
    {
        return Task.FromResult(Game.Ended);
    }

    public Task<string> GetCurrentPlayerName()
    {
        return Task.FromResult(Game.GetCurrentPlayerName());
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
