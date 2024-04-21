namespace Reverso.Infrastructure;
using Application;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        using (var sha = new System.Security.Cryptography.SHA256Managed())
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha.ComputeHash(textBytes);

            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);

            return hash;
        }
    }
}