using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Reverso.Domain.Web;

public class User
{
    [BsonId]
    public string Username { get; private set; }
    public string HashedPassword { get; private set; }
    public int GamesPlayed { get; private set; }
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
        GamesPlayed = 0;
    }

    public void AddLoss()
    {
        GamesLost++;
        GamesPlayed++;
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
        GamesPlayed++;
    }

    public void AddDraw()
    {
        Draws++;
        GamesPlayed++;
    }
}