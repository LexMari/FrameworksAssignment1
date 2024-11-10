using System.Text.Json.Serialization;

namespace ApiServer.Domain.Entities;

public class FlashcardSet
{
    /// <summary>
    /// Unique identifier for flashcard set
    /// </summary>
    public int Id { get; private set; }
    
    /// <summary>
    /// Name or title of flashcard set
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// User that created the flashcard set
    /// </summary>
    public int UserId { get; private set; }
    
    /// <summary>
    /// Creation time
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    
    /// <summary>
    /// Updated time
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Flashcards that make up this set
    /// </summary>
    private readonly List<Flashcard> _cards;
    public IReadOnlyCollection<Flashcard> Cards => _cards;
    
    /// <summary>
    /// Comments on the flashcard set
    /// </summary>
    private readonly List<Comment> _comments;
    public IReadOnlyCollection<Comment> Comment => _comments;
    
    #region Readonly properties
    
    /// <summary>
    /// Denoting if the set has any cards
    /// </summary>
    [JsonIgnore]
    public bool HasCards => Cards.Count > 0;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Default constructor
    /// </summary>
    public FlashcardSet()
    {
        Id = default;
        Name = string.Empty;
        UserId = default;
        _cards = new List<Flashcard>();
        _comments = new List<Comment>();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Constructor with parameters
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="userId"></param>
    public FlashcardSet(
        int id,
        string name,
        int userId
    ) : this()
    {
        //Id = id;
        Name = name;
        UserId = userId;
    }

    #endregion
}