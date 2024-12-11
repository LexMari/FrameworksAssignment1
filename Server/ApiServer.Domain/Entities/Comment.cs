using System.Text.Json.Serialization;

namespace ApiServer.Domain.Entities;

public class Comment
{
    [JsonIgnore]
    public int Id { get; private set; }
    
    /// <summary>
    /// Set the comment is a member of
    /// </summary>
    [JsonIgnore]
    public int FlashcardSetId { get; private set; }
    [JsonPropertyName("set")]
    public FlashcardSet FlashcardSet { get; private set; }
    
    /// <summary>
    /// The comment text
    /// </summary>
    [JsonPropertyName("commentText")]
    public string CommentText { get; private set; }
    
    /// <summary>
    /// User who made the comment
    /// </summary>
    [JsonIgnore]
    public int? AuthorId { get; private set; }
    [JsonPropertyName("author")]
    public User? Author { get; private set; }
    
    /// <summary>
    /// Review start rating of the flashcard set from 1 - 5
    /// </summary>
    public int? Rating { get; private set; }
    
    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    #region Constructors
    
    /// <summary>
    /// Default constructor
    /// </summary>
    public Comment()
    {
        CommentText = string.Empty;
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Constructor with parameters
    /// </summary>
    /// <param name="commentText"></param>
    /// <param name="rating"></param>
    /// <param name="flashcardSet"></param>
    /// <param name="user"></param>
    public Comment(string commentText,
        int? rating,
        FlashcardSet flashcardSet,
        User? user) : this()
    {
        CommentText = commentText;
        Rating = rating;
        FlashcardSetId = flashcardSet.Id;
        AuthorId = user?.Id;
    }
    
    #endregion
}