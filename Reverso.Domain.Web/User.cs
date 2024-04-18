namespace Reverso.Domain.Web;

public class User
{
    public string Username { get; private set; }
    public string HashedPassword { get; private set; }
    private UserStats Stats { get; set; }

    public User(string username, string passwordHash)
    {
        Username = username;
        HashedPassword = passwordHash;
        Stats = new UserStats();
    }

    public void AddLostGame()
    {
        Stats.AddLoss();
    }

    public void AddWonGame()
    {
        Stats.AddVictory();
    }

    public void AddDraw()
    {
        Stats.AddDraw();
    }

    public UserStats GetStats()
    {
        return Stats;
    }
}

public class UserStats
{
    public int Draws { get; private set; }
    public int GamesWon { get; private set; }
    public int GamesLost { get; private set; }

    public UserStats()
    {
        Draws = 0;
        GamesWon = 0;
        GamesLost = 0;
    }

    public void AddLoss()
    {
        GamesLost++;
    }

    public void AddVictory()
    {
        GamesWon++;
    }

    public void AddDraw()
    {
        Draws++;
    }


}