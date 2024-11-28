using ApiServer.Api.FlashcardSets.Controllers;
using ApiServer.Api.FlashcardSets.Models;
using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
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
public class FlashcardSetControllerTests : TestBase
    {
    private ILogger<FlashcardSetController> _logger;
#pragma warning disable NUnit1032
    private ApiContext _context;
#pragma warning restore NUnit1032

        private bool _dataSeeded;
    
    [SetUp]
    public async Task SetUp()
    {
        _logger = new NullLogger<FlashcardSetController>();
        _context = _provider.GetRequiredService<ApiContext>();

        if (!_dataSeeded)
        {
            await FlashcardDataHelpers.SeedFlashcardSets(_context);
            _dataSeeded = true;
        }
        
    }
    
    #region Base controller actions

    [Test]
    [NonParallelizable]
    public async Task GetFlashcardSets_Should_ReturnListOfFlashcardSets()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.GetFlashcardSets(CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<List<FlashcardSet>>();

        var dataResult = okResult.Value as List<FlashcardSet>;
        dataResult.ShouldNotBeNull();
        dataResult.Count.ShouldBeGreaterThan(1);
    }
    
    [Test]
    [NonParallelizable]
    public async Task CreateFlashcardSet_Should_Return201Response_AfterCreation()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var createCommand = FlashcardDataHelpers.GetFlashcardSetRequest();
        var result = await controller.CreateFlashcardSet(createCommand, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(CreatedAtActionResult));
        var createResult = result as CreatedAtActionResult;

        createResult.ShouldNotBeNull();
        createResult.StatusCode.ShouldBe(201);
        createResult.Value.ShouldBeOfType<FlashcardSet>();

        var dataResult = createResult.Value as FlashcardSet;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBeGreaterThan(1);
        dataResult.User.Username.ShouldBe("student");
        dataResult.Name.ShouldStartWith("ADDITIONAl FLASHCARD SET");
        dataResult.Cards.Count.ShouldBe(3);
    }
    
    [Test]
    [NonParallelizable]
    public async Task CreateFlashcardSet_ForStudent_Should_Return429Response_WhenExceedingDailyLimit()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        await FlashcardDataHelpers.SetFlashcardLimit(_context, 1);

        try
        {
            // Act
            var createCommand = FlashcardDataHelpers.GetFlashcardSetRequest();
            var result = await controller.CreateFlashcardSet(createCommand, CancellationToken.None);

            // Assert
            result.ShouldBeOfType(typeof(ObjectResult));
            var objectResult = result as ObjectResult;
        
            objectResult.ShouldNotBeNull();
            objectResult.StatusCode.ShouldBe(429);
            objectResult.Value.ShouldBeOfType<ProblemDetails>();
        
            var problemResult = objectResult.Value as ProblemDetails;
            problemResult.ShouldNotBeNull();
            problemResult.Title.ShouldBe("Flashcard set limit reached");
            problemResult.Detail.ShouldNotBeEmpty();

        }
        finally
        {
            await FlashcardDataHelpers.SetFlashcardLimit(_context);
        }
    }
    
    [Test]
    [NonParallelizable]
    public async Task CreateFlashcardSet_ForAdmin_Should_IngnoreDailyLimit()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetAdminPrincipal() 
        };
        await FlashcardDataHelpers.SetFlashcardLimit(_context, 1);

        try
        {
            // Act
            var createCommand = FlashcardDataHelpers.GetFlashcardSetRequest();
            var result = await controller.CreateFlashcardSet(createCommand, CancellationToken.None);
            
            // Assert
            result.ShouldBeOfType(typeof(CreatedAtActionResult));
            var createResult = result as CreatedAtActionResult;

            createResult.ShouldNotBeNull();
            createResult.StatusCode.ShouldBe(201);
            createResult.Value.ShouldBeOfType<FlashcardSet>();

        }
        finally
        {
            await FlashcardDataHelpers.SetFlashcardLimit(_context);
        }
    }

    #endregion
    
    #region Flashcard set controller actions
    
    [Test]
    [NonParallelizable]
    public async Task GetFlashcardSet_Should_Return200ResponseAndReturnFlashcardSetDetailResponse()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        var setId = (await _context.FlashcardSets.FirstAsync(CancellationToken.None)).Id;
        
        // Act
        var result = await controller.GetFlashcardSet(setId, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<FlashcardSetDetailResponse>();

        var dataResult = okResult.Value as FlashcardSetDetailResponse;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBe(setId);
        dataResult.Name.ShouldNotBeEmpty();
        dataResult.Cards.Count.ShouldBeGreaterThan(0);
        dataResult.Comments?.Count.ShouldBeGreaterThan(0);
    }
    
    [Test]
    [NonParallelizable]
    public async Task GetFlashcardSet_Should_Return404Response_WhenFlashcardSetNotFound()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.GetFlashcardSet(99999, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Flashcard set not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateFlashcardSet_Should_Return200Response_WhenUpdated()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        var createRequest = FlashcardDataHelpers.GetFlashcardSetRequest();
        var createResult = await controller.CreateFlashcardSet(createRequest, CancellationToken.None);
        createResult.ShouldBeOfType(typeof(CreatedAtActionResult));
        var createObject = createResult as CreatedAtActionResult;
        var createSet = createObject!.Value as FlashcardSet;
        var initialUpdate = createSet!.UpdatedAt;
        
        var updateRequest = new FlashcardSetRequest() { Name = "UPDATED NAME" };
        updateRequest.Cards.Add(createRequest.Cards.First(x => x.Answer == "One"));
        updateRequest.Cards.Add(createRequest.Cards.First(x => x.Answer == "Two"));
        updateRequest.Cards.Add(new FlashcardSetRequest.FlashCardRequest() {
            Question = "Card Four", Answer = "Four", Difficulty = Difficulty.Easy
        });
        updateRequest.Cards.Add(new FlashcardSetRequest.FlashCardRequest() {
            Question = "Card Five", Answer = "Five", Difficulty = Difficulty.Easy
        });
        
        // Act
        var result = await controller.UpdateFlashcardSet(createSet.Id, updateRequest, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var updateResult = result as OkObjectResult;

        updateResult.ShouldNotBeNull();
        updateResult.StatusCode.ShouldBe(200);
        updateResult.Value.ShouldBeOfType<FlashcardSet>();

        var dataResult = updateResult.Value as FlashcardSet;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBe(createSet.Id);
        dataResult.Name.ShouldNotBe(createRequest.Name);
        dataResult.Name.ShouldBe(updateRequest.Name);
        dataResult.Cards.Count.ShouldBe(4);
        dataResult.Cards.Any(x => x.Answer == "Three").ShouldBeFalse();
        dataResult.UpdatedAt.ShouldBeGreaterThan(initialUpdate);
    }
    
    [Test]
    [NonParallelizable]
    public async Task UpdateFlashcardSet_Should_Return403Response_WhenNotOwner()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };

        var adminSet = await _context.FlashcardSets.FirstAsync(x => x.User.Username == "admin", CancellationToken.None);
        
        // Act
        var updateRequest = FlashcardDataHelpers.GetFlashcardSetRequest();
        var result = await controller.UpdateFlashcardSet(adminSet.Id, updateRequest, CancellationToken.None);

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
    public async Task UpdateFlashcardSet_Should_Return404Response_WhenFlashcardSetNotFound()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var updateRequest = FlashcardDataHelpers.GetFlashcardSetRequest();
        var result = await controller.UpdateFlashcardSet(99999, updateRequest, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Flashcard set not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }

    [Test] [NonParallelizable] public async Task DeleteFlashcardSet_Should_Return204NoContent_AfterDeletion()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        var createRequest = FlashcardDataHelpers.GetFlashcardSetRequest();
        var createResult = await controller.CreateFlashcardSet(createRequest, CancellationToken.None);
        createResult.ShouldBeOfType(typeof(CreatedAtActionResult));
        var createObject = createResult as CreatedAtActionResult;
        var createData = createObject!.Value as FlashcardSet;
        
        // Act
        var result = await controller.DeleteFlashcardSet(createData!.Id, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(NoContentResult));
        var resultObject = result as NoContentResult;

        resultObject.ShouldNotBeNull();
        resultObject.StatusCode.ShouldBe(204);
    }
    
    [Test]
    [NonParallelizable]
    public async Task DeleteFlashcardSet_Should_Return403Response_WhenNotOwner()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };

        var adminSet = await _context.FlashcardSets.FirstAsync(x => x.User.Username == "admin", CancellationToken.None);
        
        // Act
        var result = await controller.DeleteFlashcardSet(adminSet.Id, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(403);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Delete not permitted");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    [Test]
    [NonParallelizable]
    public async Task DeleteFlashcardSet_Should_Return404Response_WhenFlashcardSetNotFound()
    {
        // Arrange
        FlashcardSetController controller = new FlashcardSetController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.DeleteFlashcardSet(99999, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(ObjectResult));
        var objectResult = result as ObjectResult;

        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(404);
        objectResult.Value.ShouldBeOfType<ProblemDetails>();

        var problemResult = objectResult.Value as ProblemDetails;
        problemResult.ShouldNotBeNull();
        problemResult.Title.ShouldBe("Flashcard set not found");
        problemResult.Detail.ShouldNotBeEmpty();
    }
    
    #endregion
}