using ApiServer.Domain.Entities;

namespace ApiServer.Api.FlashcardsSets.Models;

/// <summary>
/// API Response DTO containing Flashcard set data
/// </summary>
public class FlashcardSetDto
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public IReadOnlyCollection<FlashCard> Cards { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public List<CommentDto>? Comments { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="set"></param>
    /// <param name="comments"></param>
    public FlashcardSetDto(FlashcardSet set, List<Comment> comments)
    {
        Id = set.Id;
        Name = set.Name;
        Cards = set.Cards;
        CreatedAt = set.CreatedAt;
        UpdatedAt = set.UpdatedAt;

        if (comments.Count > 0)
        {
            Comments = new List<CommentDto>();
            comments.ForEach(x => Comments.Add(new CommentDto(x)));
        }
    }
}