using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using Shouldly;

namespace ApiServer.Domain.UnitTests;

public class FlashCardTests
{
    [Test]
    public void Constructor_Should_InitializeAttributes()
    {
        // Arrange
        var flashcardSet = new FlashcardSet("Test Set", 10);
        string question = "What is the capital of France?";
        string answer = "Paris";
        Difficulty difficulty = Difficulty.Medium;

        // Act
        var flashCard = new FlashCard(flashcardSet, question, answer, difficulty);

        // Assert
        flashCard.FlashcardSet.ShouldBe(flashcardSet);
        flashCard.FlashcardSetId.ShouldBe(flashcardSet.Id);
        flashCard.Question.ShouldBe(question);
        flashCard.Answer.ShouldBe(answer);
        flashCard.Difficulty.ShouldBe(difficulty);
    }

    [Test]
    public void Constructor_Should_TrimInputValues()
    {
        // Arrange
        var flashcardSet = new FlashcardSet("Test Set", 10);
        string question = "  What is 2 + 2?  ";
        string answer = "  4  ";
        Difficulty difficulty = Difficulty.Easy;

        // Act
        var flashCard = new FlashCard(flashcardSet, question, answer, difficulty);

        // Assert
        flashCard.Question.ShouldBe("What is 2 + 2?");
        flashCard.Answer.ShouldBe("4");
        flashCard.Difficulty.ShouldBe(difficulty);
    }

    [Test]
    public void DefaultConstructor_Should_InitializeDefaultValues()
    {
        // Act
        var flashCard = new FlashCard();

        // Assert
        flashCard.Question.ShouldBe(string.Empty);
        flashCard.Answer.ShouldBe(string.Empty);
        flashCard.Difficulty.ShouldBeNull();
        flashCard.FlashcardSetId.ShouldBe(0);
        flashCard.FlashcardSet.ShouldBeNull();
    }
}