using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using Shouldly;

namespace ApiServer.Domain.UnitTests;

public class FlashCardTests
{
    [Test]
    public void Constructor_Should_InitialiseAttributes()
    {
        // Arrange
        FlashcardSet set = new FlashcardSet("TEST SET", 1);
        string question = "QUESTION";
        string answer = "ANSWER";
        Difficulty difficulty = Difficulty.Easy;
        
        // Act
        var flashcard = new FlashCard(set, question, answer, difficulty);

        // Assert
        flashcard.Id.ShouldBe(default);
        flashcard.FlashcardSetId.ShouldBe(default);
        flashcard.Question.ShouldStartWith(question);
        flashcard.Answer.ShouldBe(answer);
        flashcard.Difficulty.ShouldBe(difficulty);
    }
}