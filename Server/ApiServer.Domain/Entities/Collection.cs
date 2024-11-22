using System.Text.Json.Serialization;

namespace ApiServer.Domain.Entities;

public class Collection
{
    /// <summary>
    /// The ID of the collection
    /// </summary>
    public int Id { get; private set; }
    
    /// <summary>
    /// The comment on the collection
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; private set; }
    
    /// <summary>
    /// User who made the comment
    /// </summary>
    [JsonIgnore]
    public int? UserId { get; private set; }
    [JsonPropertyName("user")]
    public User? User { get; private set; }
    
    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    
    /// <summary>
    /// Updated timestamp
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Flashcard sets that make up the collection
    /// </summary>
    private readonly List<FlashcardSet> _sets;
    [JsonPropertyName("sets")] 
    public IReadOnlyCollection<FlashcardSet> FlashcardSets => _sets;
    
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public Collection()
    {
        Id = default;
        Comment = string.Empty;
        UserId = default;
        _sets = new List<FlashcardSet>();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Collection(
        string comment,
        User user
    ) : this()
    {
        Comment = comment;
        UserId = user.Id;
    }
    
    #endregion

    #region Mutator methods

    /// <summary>
    /// Update collection attributes
    /// </summary>
    /// <param name="comment"></param>
    public void Update(string comment)
    {
        Comment = comment;
        UpdatedAt = DateTime.Now;
    }
    
    #endregion
    
    #region Member management

    /// <summary>
    /// Remove all sets from a collection
    /// </summary>
    public void ClearFlashcardSets()
    {
        _sets.Clear();
    }

    /// <summary>
    /// Add a new flashcard to the collection
    /// </summary>
    /// <param name="set"></param>
    public void AddFlashcardSet(FlashcardSet set)
    {
        _sets.Add(set);
        UpdatedAt = DateTime.Now;
    }

    /// <summary>
    /// Remove a flashcard set from a collection
    /// </summary>
    /// <param name="set"></param>
    public void RemoveFlashcardSet(FlashcardSet set)
    {
        _sets.Remove(set);
        UpdatedAt = DateTime.Now;
    }
    
    #endregion
    
    
}