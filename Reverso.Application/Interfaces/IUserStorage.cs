namespace Application;

using Reverso.Domain.Web;

public interface IUserStorage
{
    Task<bool> AddAsync(User user);
    Task<bool> UserExists(string username);
    Task<bool> IsUsersEmpty();
    Task<User?> FindByUsernameAsync(string username);
    Task<List<User>> GetAllUsers();
    Task<User> GetUserByUsername(string username);
}