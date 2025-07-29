using System.Security.Cryptography;
using System.Text;

namespace HealthFitnessAPI.Helpers;

public static class Hash
{
    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);

        return Convert.ToBase64String(salt.Concat(hash).ToArray());
    }
    
    public static bool VerifyPassword(string password, string storedHash)
    {
        var bytes = Convert.FromBase64String(storedHash);

        var salt = bytes.Take(16).ToArray();
        var storedHashBytes = bytes.Skip(16).ToArray();

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var computedHash = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(storedHashBytes, computedHash);
    }
}