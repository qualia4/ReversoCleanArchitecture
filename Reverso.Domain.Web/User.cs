namespace Reverso.Domain.Web;

public class User
{
    public string Username { get; private set; }
    private string HashedPassword { get; set; }
    public int Draws { get; private set; }
    public int GamesWon { get; private set; }
    public int GamesLost { get; private set; }

    public User(string username, string passwordHash)
    {
        Username = username;
        HashedPassword = passwordHash;
        Draws = 0;
        GamesWon = 0;
        GamesLost = 0;
    }

    public void AddLoss()
    {
        GamesLost++;
    }

    public bool ComparePassword(string passwordToCompare)
    {
        if (HashedPassword == passwordToCompare)
        {
            return true;
        }
        return false;
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