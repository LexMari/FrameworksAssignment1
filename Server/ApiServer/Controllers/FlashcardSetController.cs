using System.Net;
using ApiServer.Domain.Entities;
using ApiServer.Domain.Enums;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiServer.Controllers;

[Route("flashcardsets")]
[ApiController]
public class FlashcardSetController : Controller
{
    private ILogger<FlashcardSetController> _logger;
    private readonly ApiContext _context;

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public FlashcardSetController(ILogger<FlashcardSetController> logger, ApiContext dbContext)
    {
        _logger = logger;
        _context = dbContext;
    }

    #endregion

    /// <summary>
    /// Get all Flashcard Sets
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetFlashcardSets()
    {
        var setData = _context.FlashcardSets;
        return Ok(JsonConvert.SerializeObject(setData));
    }

    /// <summary>
    /// Create a flashcard set
    /// </summary>
    /// <param name="setData"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CreateFlashcardSet([FromBody] CreateSetDto setData)
    {
        var user = await _context.Users.FindAsync(setData.UserId);
        if (user is null)
        {
            _logger.LogError("Invalid user ID [{UserId}] passed to create Flashcard set", setData.UserId);
            return BadRequest("User does not exist");
        }

        if (!user.IsAdministrator &&
            _context.FlashcardSets.Count(x => x.UserId == user.Id) > 20)
        {
            _logger.LogWarning("User Id [{UserId}] has exceeded their maximum flashcard set allowance", setData.UserId);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            return new ObjectResult(new ErrorDto("User has exceeded their maximum flashcard set allowance"));
        }

        var flashcardSet = new FlashcardSet(setData.Id, setData.Name, setData.UserId);
        try
        {
            await _context.FlashcardSets.AddAsync(flashcardSet);
            await _context.SaveChangesAsync();
            return Created(nameof(GetFlashcardSets), JsonConvert.SerializeObject(flashcardSet));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create flashcard set");
            return BadRequest("Unable to create user record");
        } 
    }

    /// <summary>
    /// Get a flashcard set by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlashcardSet(int id)
    {
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(id);

        if (flashcardSet is null)
        {
            return NotFound();
        }
        return Ok(JsonConvert.SerializeObject(flashcardSet));
    }

    /// <summary>
    /// Update a flashcard set by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateData"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFlashcardSet(int id, [FromBody] FlashcardSet updateData)
    {
        var existingFlashcardSet = await _context.FlashcardSets.FindAsync(id);

        if (existingFlashcardSet == null)
        {
            return NotFound(new ErrorDto("Flashcard set not found"));
        }

        try
        {
            existingFlashcardSet.Name = updateData.Name;
            // Add other properties here
            
            _context.FlashcardSets.Update(existingFlashcardSet);
            await _context.SaveChangesAsync();
            return Ok(existingFlashcardSet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update flashcard set");
            return BadRequest(new ErrorDto("Unable to update flashcard set"));
        }
    }
    
    #region DTOs

    /// <summary>
    /// Dto for creating set
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="UserId"></param>
    public record CreateSetDto(int Id, string Name, int UserId);

    /// <summary>
    /// Dto for error messages
    /// </summary>
    /// <param name="Message"></param>
    public record ErrorDto(string Message);
    
    #endregion

}