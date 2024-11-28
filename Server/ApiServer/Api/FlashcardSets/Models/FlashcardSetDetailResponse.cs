using ApiServer.Api.FlashcardSets.Models;
using ApiServer.Domain.Entities;

namespace ApiServer.Api.FlashcardSets.Models;

/// <summary>
/// API Response DTO containing Flashcard set data
/// </summary>
public class FlashcardSetDetailResponse
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public IReadOnlyCollection<FlashCard> Cards { get; private set; }
    public User? User { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public List<CommentDetailResponse>? Comments { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="set"></param>
    /// <param name="comments"></param>
    public FlashcardSetDetailResponse(FlashcardSet set, List<Comment> comments)
    {
        Id = set.Id;
        Name = set.Name;
        Cards = set.Cards;
        User = set.User;
        CreatedAt = set.CreatedAt;
        UpdatedAt = set.UpdatedAt;

        if (comments.Count > 0)
        {
            Comments = new List<CommentDetailResponse>();
            comments.ForEach(x => Comments.Add(new CommentDetailResponse(x)));
        }
    }
}