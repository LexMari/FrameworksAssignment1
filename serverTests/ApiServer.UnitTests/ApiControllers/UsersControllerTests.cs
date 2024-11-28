using ApiServer.Api.Users.Controllers;
using ApiServer.Api.Users.Models;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using ApiServer.UnitTests.DataHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;

namespace ApiServer.UnitTests.ApiControllers;

[TestFixture]
public class UsersControllerTests : TestBase
{
    private ILogger<UsersController> _logger;
#pragma warning disable NUnit1032
    private ApiContext _context;
#pragma warning restore NUnit1032
    
    private bool _dataSeeded;
    
    [SetUp]
    public async Task SetUp()
    {
        _logger = new NullLogger<UsersController>();
        _context = _provider.GetRequiredService<ApiContext>();
        
        if (!_dataSeeded)
        {
            await FlashcardDataHelpers.SeedFlashcardSets(_context);
            await CollectionDataHelpers.SeedCollections(_context);
            _dataSeeded = true;
        }
    }

    #region Base controller actions
    
    [Test]
    [NonParallelizable]
    public async Task GetUsers_Should_ReturnUsers()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.GetUsers(CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<List<User>>();

        var dataResult = okResult.Value as List<User>;
        dataResult.ShouldNotBeNull();
        dataResult.Count.ShouldBeGreaterThan(1);
    }
    
    [Test]
    [NonParallelizable]
    public async Task CreateUser_ForUser_Should_AddUserToDatabaseWithoutAdminFlag()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };

        var userRequest = new UserRequest("TEST_STUDENT_USER", true, "PASSWORD");
        var userCount = await _context.Users.CountAsync(CancellationToken.None);
        
        // Act
        var result = await controller.CreateUser(userRequest, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(CreatedResult));
        var createdResult = result as CreatedResult;

        createdResult.ShouldNotBeNull();
        createdResult.StatusCode.ShouldBe(201);
        createdResult.Value.ShouldBeOfType<User>();
        
        userCount.ShouldBeLessThan(await _context.Users.CountAsync(CancellationToken.None));

        var dataResult = createdResult.Value as User;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBeGreaterThan(0);
        dataResult.Username.ShouldBe(userRequest.Username);
        dataResult.IsAdministrator.ShouldBeFalse();
    }
    
    [Test]
    [NonParallelizable]
    public async Task CreateUser_ForAdmin_Should_AddUserToDatabaseWithAdminFlag()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal() 
        };

        var userRequest = new UserRequest("TEST_ADMIN_USER", true, "PASSWORD");
        var userCount = await _context.Users.CountAsync(CancellationToken.None);
        
        // Act
        var result = await controller.CreateUser(userRequest, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(CreatedResult));
        var createdResult = result as CreatedResult;

        createdResult.ShouldNotBeNull();
        createdResult.StatusCode.ShouldBe(201);
        createdResult.Value.ShouldBeOfType<User>();
        
        userCount.ShouldBeLessThan(await _context.Users.CountAsync(CancellationToken.None));

        var dataResult = createdResult.Value as User;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBeGreaterThan(0);
        dataResult.Username.ShouldBe(userRequest.Username);
        dataResult.IsAdministrator.ShouldBeTrue();
    }
    
    [Test]
    [NonParallelizable]
    public async Task CreateUser_Should_Return400Response_When_UsernameExists()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal() 
        };
        var userRequest = new UserRequest("DUPLICATE_USER_TEST", true, "PASSWORD");
        await controller.CreateUser(userRequest, CancellationToken.None);
        var userCount = await _context.Users.CountAsync(CancellationToken.None);

        // Act
        var result = await controller.CreateUser(userRequest, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(400);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();
        
        userCount.ShouldBe(await _context.Users.CountAsync(CancellationToken.None));

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Failed to create user.");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    #endregion
    
    #region User controller action tests
    
    [Test]
    [NonParallelizable]
    public async Task GetUser_Should_ReturnUserObject()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.GetUser(2, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<User>();

        var dataResult = okResult.Value as User;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBe(2);
        dataResult.Username.ShouldBe("admin");
        dataResult.IsAdministrator.ShouldBeTrue();
    }
    
    [Test]
    [NonParallelizable]
    public async Task GetUser_Should_Return404Response_When_UserIdIsInvalid()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.GetUser(99999, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("User not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateUser_ForAdmin_Should_UpdateAllUserFields()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal() 
        };

        var createRequest = new UserRequest("UPDATE_TEST_ADMIN", false, "PASSWORD");
        var setupResult = await controller.CreateUser(createRequest, CancellationToken.None);
        var setupObject = setupResult as ObjectResult;
        var setupUser = setupObject?.Value as User;

        // Act
        var updateRequest =  new UserRequest("UPDATE_CHANGE_ADMIN", true, "CHANGED");
        var result = await controller.UpdateUser(setupUser!.Id, updateRequest, CancellationToken.None);
        
        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var updateResult = result as OkObjectResult;

        updateResult.ShouldNotBeNull();
        updateResult.StatusCode.ShouldBe(200);
        updateResult.Value.ShouldBeOfType<User>();
        
        var dataResult = updateResult.Value as User;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBe(setupUser.Id);
        dataResult.Username.ShouldNotBe(createRequest.Username);
        dataResult.Username.ShouldBe(updateRequest.Username);
        dataResult.IsAdministrator.ShouldNotBe(createRequest.Admin);
        dataResult.IsAdministrator.ShouldBe(updateRequest.Admin);
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateUser_ForStudent_Should_UpdateFieldsButNotAdminFlag()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var updateRequest =  new UserRequest("student_update", true, "CHANGED");
        var result = await controller.UpdateUser(1, updateRequest, CancellationToken.None);
        
        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var updateResult = result as OkObjectResult;

        updateResult.ShouldNotBeNull();
        updateResult.StatusCode.ShouldBe(200);
        updateResult.Value.ShouldBeOfType<User>();
        
        var dataResult = updateResult.Value as User;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBe(1);
        dataResult.Username.ShouldNotBe("student");
        dataResult.Username.ShouldBe(updateRequest.Username);
        dataResult.IsAdministrator.ShouldBeFalse();
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateUser_ForAdmin_Should_Return400Response_WhenUsernameAlreadyExists()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal() 
        };

        var createRequest = new UserRequest("UPDATE_400_TEST", false, "PASSWORD");
        var setupResult = await controller.CreateUser(createRequest, CancellationToken.None);
        var setupObject = setupResult as ObjectResult;
        var setupUser = setupObject?.Value as User;

        // Act
        var updateRequest =  new UserRequest("student", true, "CHANGED");
        var result = await controller.UpdateUser(setupUser!.Id, updateRequest, CancellationToken.None);
        
        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(400);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Failed to update user");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    
    [Test]
    [NonParallelizable]
    public async Task UpdateUser_ForStudent_Should_Return403Response_WhenUpdatingOtherUser()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var createRequest = new UserRequest("UPDATE_FAIL_STUDENT", false, "PASSWORD");
        var setupResult = await controller.CreateUser(createRequest, CancellationToken.None);
        var setupObject = setupResult as ObjectResult;
        var setupUser = setupObject?.Value as User;

        // Act
        var updateRequest =  new UserRequest("UPDATE_CHANGE_STUDENT", false, "CHANGED");
        var result = await controller.UpdateUser(setupUser!.Id, updateRequest, CancellationToken.None);

        
        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(403);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Update not permitted");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateUser_Should_Return404Response_WhenUserDoesNotExist()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };

        var userRequest = new UserRequest("NEW_USERNAME", true, "CHANGED_PASSWORD");
        
        // Act
        var result = await controller.UpdateUser(99999, userRequest, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("User not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    [Test]
    [NonParallelizable]
    public async Task DeleteUser_ForAdmin_Should_ReturnNoContent_AfterDeletion()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal() 
        };
        
        var userRequest = new UserRequest("ADMIN_DELETE", false, "PASSWORD");
        var setupResult = await controller.CreateUser(userRequest, CancellationToken.None);
        var setupObject = setupResult as ObjectResult;
        var setupUser = setupObject?.Value as User;

        var userCount = await _context.Users.CountAsync(CancellationToken.None);
 
        // Act
        var result = await controller.DeleteUser(setupUser!.Id, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(NoContentResult));
        userCount.ShouldBeGreaterThan(await _context.Users.CountAsync(CancellationToken.None));
    }
    
    [Test]
    [NonParallelizable]
    public async Task DeleteUser_ForStudent_Should_Return403Response()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        var userRequest = new UserRequest("DELETE_STUDENT", false, "PASSWORD");
        var setupResult = await controller.CreateUser(userRequest, CancellationToken.None);
        var setupObject = setupResult as ObjectResult;
        var setupUser = setupObject?.Value as User;
        
        var userCount = await _context.Users.CountAsync(CancellationToken.None);

        // Act
        var result = await controller.DeleteUser(setupUser!.Id, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(403);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("User deletion not permitted");
        problemResult.Detail.ShouldNotBeEmpty();
        
        userCount.ShouldBe(await _context.Users.CountAsync(CancellationToken.None));
    }
    
    [Test]
    [NonParallelizable]
    public async Task DeleteUser_Should_Return404Response_WhenUserDoesNotExist()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal() 
        };
 
        // Act
        var result = await controller.DeleteUser(99999, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("User not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    #endregion
    
    #region User sets controller action tests
    
    [Test]
    [NonParallelizable]
    public async Task GetUserFlashcardSets_Should_Return200ResponseAndListFlashcardSet()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.GetUserFlashcardSets(1, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var objectResult = result as OkObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(200);
        objectResult.Value.ShouldBeOfType<List<FlashcardSet>>();

        var dataResult = objectResult.Value as List<FlashcardSet>;
        dataResult.ShouldNotBeNull();
        dataResult.Count.ShouldBeGreaterThan(0);
    }
    
    [Test]
    [NonParallelizable]
    public async Task GetUserFlashcardSets_Should_Return404Response_WhenUserIdNotFound()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.GetUserFlashcardSets(9999, CancellationToken.None);
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("User not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    #endregion
    
    #region User collections controller action tests
    
    [Test]
    [NonParallelizable]
    public async Task GetUserCollections_Should_Return200ResponseAndListOfCollections()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal()
        };

        // Act
        var result = await controller.GetUsers(1, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<List<Collection>>();

        var dataResult = okResult.Value as List<Collection>;
        dataResult.ShouldNotBeNull();
        dataResult.Count.ShouldBeGreaterThan(0);
        dataResult[0].User.ShouldNotBeNull();
        dataResult[0].FlashcardSets.Count.ShouldBeGreaterThan(0);
    }
    
    [Test]
    [NonParallelizable]
    public async Task GetUserCollections_Should_Return404Response_WhenUserIdNotFound()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal()
        };

        // Act
        var result = await controller.GetUsers(9999, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("User not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    #endregion
    
    #region User collection controller action tests
    
    [Test]
    [NonParallelizable]
    public async Task GetCollection_Should_Return200ResponseAndCollections()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal()
        };
        var collection = await _context.Collections.FirstAsync(x => x.UserId == 1, CancellationToken.None);

        // Act
        var result = await controller.GetCollection(collection.UserId, collection.Id, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<Collection>();

        var dataResult = okResult.Value as Collection;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBe(collection.Id);
        dataResult.Comment.ShouldNotBeEmpty();
        dataResult.User.ShouldNotBeNull();
        dataResult.User.Id.ShouldBe(collection.UserId);
        dataResult.FlashcardSets.Count.ShouldBeGreaterThan(0);
    }
    
    [Test]
    [NonParallelizable]
    public async Task GetCollection_Should_Return404Response_WhenTheCollectionIsNotFound()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal()
        };

        // Act
        var result = await controller.GetCollection(1, 9999, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Flashcard set collection not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateCollection_Should_Return200ResponseAndUpdateCollection()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal()
        };
        var collection = await CollectionDataHelpers.AddCollection(_context);
        var initialUpdate = collection.UpdatedAt;
        var keepSetId = collection.FlashcardSets.First().Id;
        var removeSetId = collection.FlashcardSets.Last().Id;
        var ignoreSetId = 9999;
        
        var newSet = FlashcardDataHelpers.GetStudentFlashcardSet();
        _context.FlashcardSets.Add(newSet);
        await _context.SaveChangesAsync();
        
        var updateRequest = CollectionDataHelpers.GetCollectionRequest();
        updateRequest.Sets.Clear();
        updateRequest.Sets.Add(keepSetId);
        updateRequest.Sets.Add(newSet.Id);
        updateRequest.Sets.Add(ignoreSetId);
            
        // Act
        var result = await controller.UpdateCollection(
            collection.UserId, collection.Id, 
            updateRequest, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<Collection>();

        var dataResult = okResult.Value as Collection;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBe(collection.Id);
        dataResult.Comment.ShouldBe(updateRequest.Comment);
        dataResult.UpdatedAt.ShouldBeGreaterThan(initialUpdate);
        
        dataResult.FlashcardSets.Count.ShouldBe(2);
        dataResult.FlashcardSets.ShouldContain(x => x.Id == newSet.Id);
        dataResult.FlashcardSets.ShouldContain(x => x.Id == keepSetId);
        dataResult.FlashcardSets.ShouldNotContain(x => x.Id == removeSetId);
        dataResult.FlashcardSets.ShouldNotContain(x => x.Id == ignoreSetId);
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateCollection_Should_Return403Response_WhenNotCollectionOwner()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal()
        };
        var collection = await CollectionDataHelpers.AddCollection(_context);
        var updateRequest = CollectionDataHelpers.GetCollectionRequest();
            
        // Act
        var result = await controller.UpdateCollection(
            collection.UserId, collection.Id, 
            updateRequest, CancellationToken.None);
        
        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(403);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Authenticated user is not authorized");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateCollection_Should_Return404Response_WhenTheCollectionIsNotFound()
    {
        // Arrange
        UsersController controller = new UsersController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal()
        };
        var collection = await CollectionDataHelpers.AddCollection(_context);
        var updateRequest = CollectionDataHelpers.GetCollectionRequest();
            
        // Act
        var result = await controller.UpdateCollection(
            collection.UserId, 99999, 
            updateRequest, CancellationToken.None);
        
        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Flashcard set collection not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    [Test]
    [NonParallelizable]
    public async Task DeleteCollection_Should_ReturnNoContent_AfterDeletion()
    {
        throw new NotImplementedException();
    }
    
    [Test]
    [NonParallelizable]
    public async Task DeleteCollection_Should_Return403Response_WhenNotCollectionOwner()
    {
        throw new NotImplementedException();
    }
    
    [Test]
    [NonParallelizable]
    public async Task DeleteCollection_Should_Return404Response_WhenTheCollectionIsNotFound()
    {
        throw new NotImplementedException();
    }
    
    #endregion
}