using System.Text.Json.Serialization;
using ApiServer.Domain.Enums;

namespace ApiServer.Domain.Entities;

public class FlashcardSet
{
    
    #region Properties 
    
    /// <summary>
    /// Unique identifier for flashcard set
    /// </summary>
    public int Id { get; private set; }
    
    /// <summary>
    /// Name or title of flashcard set
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// User that created the flashcard set
    /// </summary>
    [JsonIgnore]
    public int UserId { get; private set; }
    
    [JsonIgnore]
    public User User { get; private set; }
    
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
    private readonly List<FlashCard> _cards;
    public IReadOnlyCollection<FlashCard> Cards => _cards;

    /// <summary>
    ///  The sets that make up a collection
    /// </summary>
    private readonly List<Collection> _collections;
    [JsonIgnore] 
    public IReadOnlyCollection<Collection> Collections => _collections;
    
    #endregion
    
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
        _cards = new List<FlashCard>();
        _collections = new List<Collection>();
        _collections = new List<Collection>();
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
        string name,
        int userId
    ) : this()
    {
        Name = name.Trim();
        UserId = userId;
    }

    #endregion
    
    #region Mutator Methods

    /// <summary>
    /// Update properties of the flashcard set
    /// </summary>
    /// <param name="name"></param>
    public void Update(string name)
    {
        Name = name.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
    
    #endregion
    
    #region Flashcard management

    /// <summary>
    /// Remove all cards from the flashcard set
    /// </summary>
    public void ClearCards()
    {
        _cards.Clear();
    }

    /// <summary>
    /// Add a new card to the end of the flashcard set
    /// </summary>
    /// <param name="question"></param>
    /// <param name="answer"></param>
    /// <param name="difficulty"></param>
    public void AddCard(string question, string answer, Difficulty difficulty)
    {
        _cards.Add(new FlashCard(this, question, answer, difficulty));
    }
    
    #endregion
}