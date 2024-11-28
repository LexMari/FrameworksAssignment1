using ApiServer.Api.FlashcardSets.Models;
using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using ApiServer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.UnitTests.DataHelpers;

public static class FlashcardDataHelpers
{
    public static FlashcardSet GetStudentFlashcardSet()
    {
        var flashcardSet = new FlashcardSet($"Student Set ${DateTime.UtcNow}", 1);
        
        flashcardSet.AddCard("Card One", "One", Difficulty.Easy);
        flashcardSet.AddCard("Card Two", "Two", Difficulty.Medium);
        flashcardSet.AddCard("Card Three", "Three", Difficulty.Hard);
        
        return flashcardSet;
    }
    
    public static FlashcardSet GetAdminFlashcardSet()
    {
        var flashcardSet = new FlashcardSet($"Admin Set ${DateTime.UtcNow}", 2);
        
        flashcardSet.AddCard("Card One", "One", Difficulty.Easy);
        flashcardSet.AddCard("Card Two", "Two", Difficulty.Medium);
        flashcardSet.AddCard("Card Three", "Three", Difficulty.Hard);

        return flashcardSet;
    }
    
    public static FlashcardSetRequest GetFlashcardSetRequest()
    {
        var request = new FlashcardSetRequest
        { 
            Name = $"ADDITIONAl FLASHCARD SET ${DateTime.UtcNow}"
        };
        
        request.Cards.Add(new FlashcardSetRequest.FlashCardRequest() {
            Question = "Card One", Answer = "One", Difficulty = Difficulty.Easy
        });
        request.Cards.Add(new FlashcardSetRequest.FlashCardRequest() {
            Question = "Card Two", Answer = "Two", Difficulty = Difficulty.Medium
        });
        request.Cards.Add(new FlashcardSetRequest.FlashCardRequest() {
            Question = "Card Three", Answer = "Thre", Difficulty = Difficulty.Hard
        });
        
        return request;
    }

    public static async Task SeedFlashcardSets(ApiContext context)
    {
        var studentSet = GetStudentFlashcardSet();
        var adminSet = GetAdminFlashcardSet();
        
        var student = await context.Users.FirstAsync(x => x.Username == "student", CancellationToken.None);
        var admin = await context.Users.FirstAsync(x => x.Username == "admin", CancellationToken.None);
        
        context.FlashcardSets.Add(studentSet);
        context.FlashcardSets.Add(adminSet);
        await context.SaveChangesAsync();

        context.Comments.Add(new Comment("Comment on student set", studentSet, student));
        context.Comments.Add(new Comment("Comment on admin set", adminSet, admin));
        await context.SaveChangesAsync();
    }
    
    public static async Task SetFlashcardLimit(ApiContext context, int limit = 20)
    {
        var setting = await context.ApiSettings.FirstAsync(x => x.Id == "SET_LIMIT_DAY", CancellationToken.None);
        setting.Update(setting.Description, limit.ToString());
        await context.SaveChangesAsync();
    }
}