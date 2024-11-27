using Microsoft.AspNetCore.Mvc;
using ApiServer.Api.Common.Models;

namespace ApiServer.Api.Common.Controllers;

/// <summary>
/// API server root route controller
/// </summary>
[Route("api/")]
[ApiController]
public class HomeController : Controller
{
    private ILogger<HomeController> _logger;
    private IConfiguration _configuration;

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public HomeController(
        ILogger<HomeController> logger, 
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    #endregion
    
    /// <summary>
    /// Get API version
    /// </summary>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiVersion), StatusCodes.Status200OK)]
    public IActionResult GetApiVersion()
    {
        var result = new ApiVersion() { Version = _configuration.GetValue<string>("Version") };
        return Ok(result);
    }
}