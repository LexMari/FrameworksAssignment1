namespace ApiServer.Api.FlashcardSets.Models;

/// <summary>
/// Data passed into the API to create a comment
/// </summary>
public class CommentRequest
{
    /// <summary>
    /// Comment text
    /// </summary>
    public string Comment { get; set; }
    
    /// <summary>
    /// Rating from 1 - 5 of the flashcard set
    /// </summary>
    public int? Rating { get; set; }
}