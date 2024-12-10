using ApiServer.Domain.Entities;
using Shouldly;

namespace ApiServer.Domain.UnitTests;

public class UserTests
{
    [Test]
    public void Constructor_Should_InitialiseAttributes()
    {
        // Arrange
        int userId = 10;
        string userName = "MY_USERNAME";
        string password = "A_Complex_P@55word";
        bool isAdmin = true;
        
        // Act
        var testUser = new User(userId, userName, password, isAdmin);

        // Assert
        testUser.Id.ShouldBe(userId);
        testUser.Username.ShouldBe(userName);
        testUser.PasswordHash.ShouldNotBe(string.Empty);
        testUser.PasswordSalt.ShouldNotBe(string.Empty);
        testUser.IsAdministrator.ShouldBeTrue();
        
        User.VerifyPassword(
            password, 
            testUser.PasswordHash, 
            Convert.FromBase64String(testUser.PasswordSalt)).ShouldBeTrue();
    }

    [Test]
    public void Update_Should_UpdateProperties()
    {
        // Arrange
        int userId = 10;
        string userName = "MY_USERNAME";
        string password = "A_Complex_P@55word";
        var testUser = new User(userId, userName, password, true);

        // Act
        string newUsername = "CHANGED_USERNAME";
        string newPassword = "CHANGED_PASSWORD";
        testUser.Update(newUsername, false);

        // Assert
        testUser.Id.ShouldBe(userId);
        testUser.Username.ShouldBe(newUsername);
        testUser.PasswordHash.ShouldNotBe(string.Empty);
        testUser.PasswordSalt.ShouldNotBe(string.Empty);
        testUser.IsAdministrator.ShouldBeFalse();
    }
    
    [Test]
    public void ChangePassword_Should_UpdateProperties()
    {
        // Arrange
        int userId = 10;
        string userName = "MY_USERNAME";
        string password = "A_Complex_P@55word";
        var testUser = new User(userId, userName, password, true);
        
        // Act
        string newPassword = "CHANGED_PASSWORD";
        testUser.ChangePassword(newPassword);
        
        // Assert
        testUser.Id.ShouldBe(userId);
        testUser.Username.ShouldBe(userName);
        testUser.PasswordHash.ShouldNotBe(string.Empty);
        testUser.PasswordSalt.ShouldNotBe(string.Empty);
        testUser.IsAdministrator.ShouldBeTrue();
        
        User.VerifyPassword(
            newPassword, 
            testUser.PasswordHash, 
            Convert.FromBase64String(testUser.PasswordSalt)).ShouldBeTrue();
    }
}