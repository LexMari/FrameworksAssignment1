using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using Shouldly;

namespace ApiServer.Domain.UnitTests;

public class FlashcardSetTests
{
    [Test]
    public void Constructor_Should_InitialiseAttributes()
    {
        // Arrange
        string setName = "SET_NAME";
        int userId = 3;

        // Act
        var flashcardSet = new FlashcardSet(setName, userId);

        // Assert
        flashcardSet.Id.ShouldBe(default);
        flashcardSet.Name.ShouldBe(setName);
        flashcardSet.UserId.ShouldBe(userId);
        flashcardSet.Cards.Count.ShouldBe(0);
        flashcardSet.CreatedAt.ShouldBeLessThan(DateTime.Now);
    }

    [Test]
    public void Update_Should_UpdateName()
    {
        // Arrange
        var flashcardSet = new FlashcardSet("STARTING_NAME", 3);
        var initialUpdatedAt = flashcardSet.UpdatedAt;
        
        // Act
        var updatedName = "CHANGED_NAME";
        flashcardSet.Update(updatedName);
        
        // Assert
        flashcardSet.Name.ShouldBe(updatedName);
        flashcardSet.UpdatedAt.ShouldBeGreaterThanOrEqualTo(initialUpdatedAt);
    }
    
    [Test]
    public void AddCard_Should_AddACard()
    {
        // Arrange
        var flashcardSet = new FlashcardSet("STARTING_NAME", 3);
        flashcardSet.Cards.Count.ShouldBe(0);
        
        // Act
        const string question = "QUESTION";
        const string answer = "ANSWER";
        var difficulty = Difficulty.Easy;
        
        flashcardSet.AddCard(question, answer, difficulty);
        
        // Assert
        flashcardSet.Cards.Count.ShouldBe(1);
    }
    
    [Test]
    public void ClearCards_Should_DeleteAllCards()
    {
        // Arrange
        var flashcardSet = new FlashcardSet("STARTING_NAME", 3);
        const string question = "QUESTION";
        const string answer = "ANSWER";
        var difficulty = Difficulty.Easy;
        flashcardSet.AddCard(question, answer, difficulty);
        
        flashcardSet.Cards.Count.ShouldBe(1);

        // Act
        flashcardSet.ClearCards();
        
        // Assert
        flashcardSet.Cards.Count.ShouldBe(0);
    }
}