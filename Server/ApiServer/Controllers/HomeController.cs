using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers;

/// <summary>
/// API server root route controller
/// </summary>
[Route("/")]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetFlashcardSets()
    {
        var result = new { version = _configuration.GetValue<string>("Version") };
        return Ok(result);
    }
}