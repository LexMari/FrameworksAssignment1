using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiServer.Domain.Entities;

public class User
{
    /// <summary>
    /// Unique user ID
    /// </summary>
    public int Id { get; private set; }
    
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; private set; }
    
    /// <summary>
    /// Hashed Password
    /// </summary>
    [JsonIgnore]
    public string PasswordHash { get; private set; }
    
    /// <summary>
    /// Salt used to hash the password
    /// </summary>
    [JsonIgnore]
    public string PasswordSalt { get; private set; }
    
    /// <summary>
    /// Flag to denote if the user is an admin
    /// </summary>
    [JsonPropertyName("admin")]
    public bool IsAdministrator { get; set; }
    
    #region Constructors

    // Default Constructor
    public User()
    {
        Id = default;
        Username = string.Empty;
        PasswordHash = string.Empty;
        PasswordSalt = string.Empty;
        IsAdministrator = false;
    }
    
    //Constructor with parameters
    public User(
        string username,
        string password,
        bool isAdministrator
    ) : this()
    {
        Username = username;
        IsAdministrator = isAdministrator;
        PasswordHash = HashPassword(password, out byte[] salt);
        PasswordSalt = Convert.ToBase64String(salt);
    }
    
    /// <summary>
    /// Constructor with parameters used for seeding data
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="isAdministrator"></param>
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
    
    #region Mutator methods

    /// <summary>
    /// Update the user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="isAdministrator"></param>
    public void Update(string username,
        string password,
        bool isAdministrator)
    {
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