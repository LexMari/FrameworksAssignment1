namespace ApiServer.Domain.Entities;

public class FlashcardSet
{
    public int Id { get; private set; }
    
    public string Name { get; private set; }
    
    //User that created this set
    public int UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }

    //Flashcards that make up this set
    private readonly List<FlashCard> _cards;
    public IReadOnlyCollection<FlashCard> Cards => _cards;
    
    //Comments on this flashcard set
    private readonly List<Comment> _comments;
    public IReadOnlyCollection<Comment> Comment => _comments;
    
    #region Readonly properties
    
    //Flag denoting whether the set has any cards
    public bool HasCards => Cards.Count > 0;
    
    #endregion
    
    #region Constructors
    
    //Default constructor
    public FlashcardSet()
    {
        Id = default;
        Name = string.Empty;
        UserId = default;
        _cards = new List<FlashCard>();
        _comments = new List<Comment>();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    //Constructor with parameters
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