using System.Security.Cryptography;
using System.Text;

namespace ApiServer.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    
    public string Username { get; private set; }
    
    public string PasswordHash { get; private set; }
    
    public string PasswordSalt { get; private set; }
    
    public bool IsAdministrator { get; set; }
    
    #region Constructors

    // Default Constructor
    public User()
    {
        Id = -1;
        Username = string.Empty;
        PasswordHash = string.Empty;
        PasswordSalt = string.Empty;
        IsAdministrator = false;
    }
    
    //Constructor with parameters
    public User(
        int userId,
        string username,
        string password,
        bool isAdministrator
    ) : this()
    {
        Id = userId;
        Username = username;
        IsAdministrator = isAdministrator;
        PasswordHash = HashPassword(password, out byte[] salt);
        PasswordSalt = Convert.ToBase64String(salt);
    }
    #endregion
    
    #region Static methods for password hashing and verification

    //Method for hashing password
    public static string HashPassword(string password, out byte[] salt)
    {
        int keySize = 64;
        salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            100,
            HashAlgorithmName.SHA512,
            keySize);
        return Convert.ToBase64String(hash);
    }
    
    //Method for verifying Hashes are the same
    public static bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100, HashAlgorithmName.SHA512, 64);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromBase64String(hash));
    }
    
    #endregion
}