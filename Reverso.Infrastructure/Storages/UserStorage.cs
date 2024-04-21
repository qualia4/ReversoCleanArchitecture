using Application;
using Reverso.Domain.Web;
namespace Reverso.Infrastructure;

public class UserStorage: IUserStorage
{
    private readonly DatabaseTemplate Database;

    public UserStorage(DatabaseTemplate _db)
    {
        Database = _db;
    }

    public async Task<bool> AddAsync(User user)
    {
        if (Database.UserExists(user.Username))
        {
            return false;
        }
        await Database.AddUser(user);
        return true;
    }

    public async Task<bool> UserExists(string username)
    {
        return Database.UserExists(username);
    }

    public async Task<bool> IsUsersEmpty()
    {
        return Database.IsUsersEmpty();
    }

    public async Task<List<User>> GetAllUsers()
    {
        return Database.GetAllUsers();
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await Database.GetUserByUsername(username);
    }

    public Task<User?> FindByUsernameAsync(string username)
    {
        return Database.GetUserByUsername(username);
    }

}