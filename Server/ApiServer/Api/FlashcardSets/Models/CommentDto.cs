using ApiServer.Domain.Entities;

namespace ApiServer.Api.FlashcardsSets.Models;

/// <summary>
/// API Response DTO representing a comment
/// </summary>
public class CommentDto
{
    public string Comment { get; private set; }
    
    public User? Author { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="comment"></param>
    public CommentDto(Comment comment)
    {
        Comment = comment.CommentText;
        Author = comment.Author;
    }
}