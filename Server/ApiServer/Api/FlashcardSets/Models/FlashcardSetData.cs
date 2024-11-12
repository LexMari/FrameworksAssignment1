using ApiServer.Domain.Enums;

namespace ApiServer.Api.FlashcardsSets.Models;

public class FlashcardSetData()
{
    public string Name { get; set; }
    public List<FlashCardEntry> Cards { get; set; } = new();

    public class FlashCardEntry()
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set;  } = string.Empty;
        public Difficulty Difficulty { get; set; } = Difficulty.Medium;
    }
}