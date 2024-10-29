using System.Runtime.InteropServices.JavaScript;

namespace FrameworksAssignment1.Domain.Entities;

public class FlashcardSet
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public List<FlashCard> Cards { get; set; }
    
    public string UserId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    #region Constructors
    
    public FlashcardSet()
    {
        Cards = new List<FlashCard>();
    }
    
    public FlashcardSet(
        string name,
        string userId
    ) : this()
    {
        Name = name;
        UserId = userId;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    #endregion
}

