using System.Text.Json.Serialization;

namespace ApiServer.Domain.Entities;

public class Comment
{
    /// <summary>
    /// Set the comment is a member of
    /// </summary>
    [JsonIgnore]
    public int? FlashcardSetId { get; private set; }
    
    [JsonPropertyName("set")]
    public FlashcardSet? FlashcardSet { get; private set; }
    
    /// <summary>
    /// The comment text
    /// </summary>
    [JsonPropertyName("commentText")]
    public string CommentText { get; private set; }
    
    /// <summary>
    /// User who made the comment
    /// </summary>
    [JsonIgnore]
    public int? UserId { get; private set; }

    [JsonPropertyName("author")]
    public User? Author { get; private set; }
    
    /// <summary>
    /// Creation timestamp
    /// </summary>
    [JsonIgnore]
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
    /// <param name="flashcardSet"></param>
    /// <param name="user"></param>
    public Comment(string commentText, FlashcardSet? flashcardSet, User? user) : this()
    {
        CommentText = commentText;
        FlashcardSetId = flashcardSet?.Id;
        UserId = user?.Id;
    }
    
    #endregion
}