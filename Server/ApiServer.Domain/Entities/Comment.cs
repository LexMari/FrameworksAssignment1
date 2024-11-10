namespace ApiServer.Domain.Entities;

public class Comment
{
    public int Id { get; private set; }
    
    public int FlashcardSetId { get; set; }
    
    public FlashcardSet FlashcardSet { get; private set; }
    
    public string CommentText { get; private set; }
    
    public string Username { get; private set; }
    
    public DateTime CreatedAt { get; set; }
    
    #region Constructors

    //Default Constructor
    public Comment()
    {
        CommentText = string.Empty;
        CreatedAt = DateTime.Now;
    }
    
    //Constructor with Parameters
    public Comment(
        FlashcardSet flashcardSet,
        string commentText,
        string username
    ) : this()
    {
        CommentText = commentText;
        Username = username;
    }
    
    #endregion
}