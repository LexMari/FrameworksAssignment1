using ApiServer.Api.Collections.Controllers;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using ApiServer.UnitTests.DataHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;

namespace ApiServer.UnitTests.ApiControllers;

[TestFixture]
public class CollectionsControllerTests : TestBase
{
    private ILogger<CollectionsController> _logger;
#pragma warning disable NUnit1032
    private ApiContext _context;
#pragma warning restore NUnit1032

    private bool _dataSeeded;

    [SetUp]
    public async Task SetUp()
    {
        _logger = new NullLogger<CollectionsController>();
        _context = _provider.GetRequiredService<ApiContext>();
        
        if (!_dataSeeded)
        {
            await CollectionDataHelpers.SeedCollections(_context);
            _dataSeeded = true;
        }
    }

    #region Base controller actions

    [Test]
    [NonParallelizable]
    public async Task GetCollections_Should_Return200ResponseAndListCollections()
    {
        // Arrange
        CollectionsController controller = new CollectionsController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal()
        };

        // Act
        var result = await controller.GetCollections(CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<List<Collection>>();

        var dataResult = okResult.Value as List<Collection>;
        dataResult.ShouldNotBeNull();
        dataResult.Count.ShouldBeGreaterThan(1);
    }
    
    [Test]
    [NonParallelizable]
    public async Task CreateFlashcardSet_Should_Return201Response_AfterCreation()
    {
        // Arrange
        CollectionsController controller = new CollectionsController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var createRequest = CollectionDataHelpers.GetCollectionRequest();
        var ignoreId = 9999;
        createRequest.Sets.Add(ignoreId);
        
        var result = await controller.CreateCollection(createRequest, CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(CreatedAtActionResult));
        var createObject = result as CreatedAtActionResult;

        createObject.ShouldNotBeNull();
        createObject.StatusCode.ShouldBe(201);
        createObject.Value.ShouldBeOfType<Collection>();

        var dataResult = createObject.Value as Collection;
        dataResult.ShouldNotBeNull();
        dataResult.Id.ShouldBeGreaterThan(1);
        dataResult.User.Username.ShouldBe("student");
        dataResult.Comment.ShouldStartWith("ADDITIONAl COLLECTION");
        dataResult.FlashcardSets.Count.ShouldBe(1);
        dataResult.FlashcardSets.ShouldNotContain(x => x.Id == ignoreId);
    }
    
    #endregion
    
    #region Random route controller actions
    
    [Test]
    [NonParallelizable]
    public async Task GetRandomCollection_Should_Return302Response()
    {
        // Arrange
        CollectionsController controller = new CollectionsController(_logger, _context);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = GetStudentPrincipal() 
        };
        
        // Act
        var result = await controller.GetRandomCollection(CancellationToken.None);

        // Assert
        result.ShouldBeOfType(typeof(RedirectResult));
        var resultObject = result as RedirectResult;

        resultObject.ShouldNotBeNull();
        resultObject.Url.ShouldMatch("/api/users/\\d*/collections/\\d*");
    }

    #endregion
}