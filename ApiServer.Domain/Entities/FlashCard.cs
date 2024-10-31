using ApiServer.Domain.Enums;

namespace ApiServer.Domain.Entities;

public class FlashCard
{
    //Id for this Flashcard
    public int Id { get; set; }
    
    //The set this Flashcard is a member of
    public int FlashcardSetId { get; private set; }
    
    public FlashcardSet FlashcardSet { get; private set; }
   
    //Question on the Flashcard
    public string Question { get; private set; }
    
    //Answer on the Flashcard
    public string Answer { get; private set; }

    public Difficulty Difficulty { get; private set; }
    
    #region Constructors

    //Default Constructor
    public FlashCard()
    {
        Question = string.Empty;
        Answer = string.Empty;
        Difficulty = Difficulty.Easy;
    }
    
    //Constructor with Parameters
    public FlashCard(
        FlashcardSet flashcardSet,
        string question,
        string answer,
        Difficulty difficulty)
    {
        FlashcardSetId = flashcardSet.Id;
        FlashcardSet = flashcardSet;
        Question = question;
        Answer = answer;
        Difficulty = difficulty;
    }
    
    #endregion
    
}