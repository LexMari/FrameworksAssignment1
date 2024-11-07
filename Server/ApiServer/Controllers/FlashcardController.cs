using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiServer.Controllers;

[Route("flashcards")]
[ApiController]
public class FlashcardController : Controller
{
    private ILogger<FlashcardController> _logger;
    private readonly ApiContext _context;
    
    #region Constructor
    
    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public FlashcardController(ILogger<FlashcardController> logger, ApiContext dbContext)
    {
        _logger = logger;
        _context = dbContext;
    }
    #endregion
    
    /// <summary>
    /// Get all Flashcards
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetFlashcards()
    {
        var setData = _context.Flashcards;
        return Ok(JsonConvert.SerializeObject(setData));
    }
}