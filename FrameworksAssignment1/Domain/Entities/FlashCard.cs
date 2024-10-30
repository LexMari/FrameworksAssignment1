namespace FrameworksAssignment1.Domain.Entities;
using Domain.Enums;

public class FlashCard
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public Difficulty Difficulty;

    
    #region Constructors 
    public FlashCard()
    {
        
    }

    public FlashCard(
        string question,
        string answer,
        Difficulty difficulty) : this()
    {
        Question = question;
        Answer = answer;
        Difficulty = difficulty;
    }
    
    #endregion
}