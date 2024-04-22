using Application;
using Reverso.Domain.Web;
using MongoDB.Driver;

namespace Reverso.Infrastructure;

public class MongoUserStorage: IUserStorage
{
    private readonly MongoDbContext _context;

    public MongoUserStorage(MongoDbContext context)
    {
        _context = context;
        AddAsync(new User("BOT", Guid.NewGuid().ToString()));
        AddAsync(new User("MMBOT", Guid.NewGuid().ToString()));
        GlobalWeb.ResultNotifier.GameEnded += RecalculateStatsAsync;
    }

    private async void RecalculateStatsAsync(Dictionary<string, int> points)
    {
        if (points.Count == 0)
        {
            Console.WriteLine("No points data available.");
            return;
        }

        string winner = "";
        string loser = "";
        int maxPoints = int.MinValue;
        int minPoints = int.MaxValue;

        foreach (var user in points)
        {
            if (user.Value > maxPoints)
            {
                maxPoints = user.Value;
                winner = user.Key;
            }
            if (user.Value < minPoints)
            {
                minPoints = user.Value;
                loser = user.Key;
            }
        }

        var winnerUser = await _context.Users.Find(u => u.Username == winner).FirstOrDefaultAsync();
        var loserUser = await _context.Users.Find(u => u.Username == loser).FirstOrDefaultAsync();

        if (maxPoints == minPoints)
        {
            winnerUser.AddDraw();
            loserUser.AddDraw();
        }
        else
        {
            winnerUser.AddVictory();
            loserUser.AddLoss();
        }

        await _context.Users.ReplaceOneAsync(u => u.Username == winnerUser.Username, winnerUser);
        await _context.Users.ReplaceOneAsync(u => u.Username == loserUser.Username, loserUser);
    }

    public async Task<bool> AddAsync(User user)
    {
        if (await UserExists(user.Username))
            return false;

        await _context.Users.InsertOneAsync(user);
        return true;
    }

    public async Task<bool> UserExists(string username)
    {
        return await _context.Users.Find(u => u.Username == username).AnyAsync();
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await _context.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await FindByUsernameAsync(username) ?? throw new Exception("User not found");
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.Find(_ => true).ToListAsync();
    }

    public async Task<bool> IsUsersEmpty()
    {
        return await _context.Users.CountDocumentsAsync(_ => true) == 0;
    }
}