namespace Application;

public interface IPasswordHasher
{
    string HashPassword(string password);
}