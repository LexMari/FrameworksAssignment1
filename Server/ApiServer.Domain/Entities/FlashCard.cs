using System.Text.Json.Serialization;
using ApiServer.Domain.Enums;

namespace ApiServer.Domain.Entities;

public class FlashCard
{
    /// <summary>
    /// Id of this flashcard
    /// </summary>
    [JsonIgnore]
    public int Id { get; set; }
    
    /// <summary>
    /// Set the flashcard is a member of
    /// </summary>
    [JsonIgnore]
    public int FlashcardSetId { get; private set; }
    [JsonIgnore]
    public FlashcardSet FlashcardSet { get; private set; }
   
    /// <summary>
    /// Flashcard question
    /// </summary>
    public string Question { get; private set; }
    
    /// <summary>
    /// Answer on the flashcard
    /// </summary>
    public string Answer { get; private set; }

    /// <summary>
    /// Difficulty of the question
    /// </summary>
    public Difficulty? Difficulty { get; private set; }
    
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public FlashCard()
    {
        Question = string.Empty;
        Answer = string.Empty;
    }
    
    /// <summary>
    /// Constructor with parameters
    /// </summary>
    /// <param name="flashcardSet"></param>
    /// <param name="question"></param>
    /// <param name="answer"></param>
    /// <param name="difficulty"></param>
    public FlashCard(
        FlashcardSet flashcardSet,
        string question,
        string answer,
        Difficulty? difficulty)
    {
        FlashcardSetId = flashcardSet.Id;
        FlashcardSet = flashcardSet;

        question = question.Trim();
        question = (question.EndsWith('?') ? question : $"{question}");
        Question = question;

        Answer = answer.Trim();
        Difficulty = difficulty;
    }
    
    #endregion
    
}