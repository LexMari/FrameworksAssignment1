using ApiServer.Domain.Enums;

namespace ApiServer.Api.FlashcardsSets.Models;

/// <summary>
/// Data passed into the API to create/update a flashcard
/// </summary>
public class FlashcardSetData()
{
    public string Name { get; set; }
    public List<FlashCardEntry> Cards { get; set; } = new();

    /// <summary>
    /// Internal class for a flashcard
    /// </summary>
    public class FlashCardEntry()
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set;  } = string.Empty;
        public Difficulty Difficulty { get; set; } = Difficulty.Medium;
    }
}