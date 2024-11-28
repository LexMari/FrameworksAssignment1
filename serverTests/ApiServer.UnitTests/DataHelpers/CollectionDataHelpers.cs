using ApiServer.Api.Collections.Models;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.UnitTests.DataHelpers;

public static class CollectionDataHelpers
{
    public static CollectionRequest GetCollectionRequest()
    {
        var request = new CollectionRequest()
        {
            Comment = $"ADDITIONAL COLLECTION ${DateTime.UtcNow}",
            Sets = [1]
        };

        return request;
    }
    
    public static async Task<Collection> AddCollection(ApiContext context)
    {
        var admin = await context.Users.FirstAsync(x => x.Username == "admin", CancellationToken.None);
        
        var collection = new Collection($"ADDITIONAL COLLECTION ${DateTime.UtcNow}", admin);
        
        context.Collections.Add(collection);
        await context.SaveChangesAsync();

        var studentSet = FlashcardDataHelpers.GetStudentFlashcardSet();
        var adminSet = FlashcardDataHelpers.GetAdminFlashcardSet();
        
        context.FlashcardSets.Add(studentSet);
        context.FlashcardSets.Add(adminSet);
        await context.SaveChangesAsync();
        
        collection.AddFlashcardSet(studentSet);
        collection.AddFlashcardSet(adminSet);
        await context.SaveChangesAsync();

        return collection;
    }

    public static async Task SeedCollections(ApiContext context)
    {
        var student = await context.Users.FirstAsync(x => x.Username == "student", CancellationToken.None);
        var admin = await context.Users.FirstAsync(x => x.Username == "admin", CancellationToken.None);
        
        var studentCollection = new Collection("STUDENT Collection", student);
        var adminCollection = new Collection("ADMIN Collection", admin);
        
        context.Collections.Add(studentCollection);
        context.Collections.Add(adminCollection);
        await context.SaveChangesAsync();
        var studentSet = FlashcardDataHelpers.GetStudentFlashcardSet();
        var adminSet = FlashcardDataHelpers.GetAdminFlashcardSet();
        
        context.FlashcardSets.Add(studentSet);
        context.FlashcardSets.Add(adminSet);
        await context.SaveChangesAsync();
        
        studentCollection.AddFlashcardSet(studentSet);
        studentCollection.AddFlashcardSet(adminSet);
        await context.SaveChangesAsync();
        
        adminCollection.AddFlashcardSet(studentSet);
        adminCollection.AddFlashcardSet(adminSet);
        await context.SaveChangesAsync();
    }
}