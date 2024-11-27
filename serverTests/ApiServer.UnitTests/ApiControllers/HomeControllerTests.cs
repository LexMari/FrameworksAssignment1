using ApiServer.Api.Common.Models;
using ApiServer.Api.Common.Controllers;
using ApiServer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;

namespace ApiServer.UnitTests.ApiControllers;

[TestFixture]
public class HomeControllerTests : TestBase
{
    private ILogger<HomeController> _logger;
    private IConfiguration _configuration;

    [SetUp]
    public void SetUp()
    {
        _logger = new NullLogger<HomeController>();
        _configuration = TestHelpers.GetConfiguration();
    }

    [Test]
    [NonParallelizable]
    public void GetApiVersion_Should_ReturnVersionString()
    {
        // Arrange
        HomeController controller = new HomeController(_logger, _configuration);
        
        // Act
        var result = controller.GetApiVersion();
        
        // Assert
        result.ShouldBeOfType(typeof(OkObjectResult));
        var okResult = result as OkObjectResult;

        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        okResult.Value.ShouldBeOfType<ApiVersion>();

        var dataResult = okResult.Value as ApiVersion;
        dataResult.ShouldNotBeNull();
        dataResult.Version.ShouldBe(_configuration.GetValue<string>("Version"));
    }
}