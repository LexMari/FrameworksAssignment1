using ApiServer.Domain.Entities;

namespace ApiServer.Api.FlashcardSets.Models;

/// <summary>
/// API Response DTO representing a comment
/// </summary>
public class CommentDetailResponse
{
    public string Comment { get; private set; }
    
    public User? Author { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="comment"></param>
    public CommentDetailResponse(Comment comment)
    {
        Comment = comment.CommentText;
        Author = comment.Author;
        CreatedAt = comment.CreatedAt;
    }
}