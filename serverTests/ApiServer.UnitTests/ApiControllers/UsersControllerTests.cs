using ApiServer.Api.Users.Controllers;
using ApiServer.Api.Users.Models;
using ApiServer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;

namespace ApiServer.UnitTests.ApiControllers;

[TestFixture]
public class UsersControllerTests : TestBase
{
    private ILogger<UsersController> _logger;
    
    [SetUp]
    public void SetUp()
    {
        _logger = new NullLogger<UsersController>();
    }

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

        var userRequest = new UserRequest("TEST_ADMIN_USER", true, "PASSWORD");
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
}