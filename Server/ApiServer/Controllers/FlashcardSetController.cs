using System.Net;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using ApiServer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiServer.Controllers;

[Route("sets")]
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

    #region Base /set routes

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
    /// <param name="flashcardSet"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CreateFlashcardSet([FromBody] FlashcardSet flashcardSet)
    {
        var user = await _context.Users.FindAsync(flashcardSet.UserId);
        if (user is null)
        {
            _logger.LogError("Invalid user ID [{UserId}] passed to create Flashcard set", flashcardSet.UserId);
            return BadRequest("User does not exist");
        }

        if (!user.IsAdministrator &&
            _context.FlashcardSets.Count(x => x.UserId == user.Id) > 20)
        {
            _logger.LogWarning("User Id [{UserId}] has exceeded their maximum flashcard set allowance",
                flashcardSet.UserId);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            var error = new Error("User has exceeded their maximum flashcard set allowance");
            return new ObjectResult(JsonConvert.SerializeObject(error));
        }

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

    #endregion

    #region /set/{setId} routes

    /// <summary>
    /// Get a flashcard set by Id
    /// </summary>
    /// <param name="setId">Id of the flashcard set</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{setId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlashcardSet(int setId, CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(setId, cancellationToken);

        if (flashcardSet is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }

        return Ok(JsonConvert.SerializeObject(flashcardSet));
    }

    /// <summary>
    /// Update a flashcard set
    /// </summary>
    /// <param name="setId"></param>
    /// <param name="flashcardSet"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{setId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFlashcardSet(int setId, [FromBody] FlashcardSet flashcardSet)
    {
        var existingFlashcardSet = await _context.FlashcardSets.FindAsync(setId);

        if (existingFlashcardSet == null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }

        try
        {
            existingFlashcardSet.Name = flashcardSet.Name;
            existingFlashcardSet.UpdatedAt = DateTime.Now;
            // Add other properties here

            _context.FlashcardSets.Update(existingFlashcardSet);
            await _context.SaveChangesAsync();
            return Ok(existingFlashcardSet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update flashcard set");
            return BadRequest("Unable to update flashcard set");
        }
    }

    /// <summary>
    /// Delete flashcard set
    /// </summary>
    /// <param name="setId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{setId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFlashcardSet(int setId)
    {
        var flashcardSet = await _context.FlashcardSets.FindAsync(setId);

        if (flashcardSet == null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }

        _context.FlashcardSets.Remove(flashcardSet);

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete flashcard set");
            return BadRequest("Unable to delete flashcard set");
        }
    }

    #endregion

    #region /set/{setId}/comment routes

    /// <summary>
    /// Add a comment to a flashcard set
    /// </summary>
    /// <returns>The created user</returns>
    [HttpPost]
    [Route("{setId:int}/comment")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComment(int setId,
        [FromBody] string comment,
        CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(setId, cancellationToken);

        if (flashcardSet is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }

        // Determine the current user from authentication

        /*
         var comment = new Comment(comment, flashcardSet, user);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return Created(comment);
        */

        throw new NotImplementedException();
    }

    #endregion

    #region /set/(setId}/cards route actions
    
    /// <summary>
    /// Get the cards for the flashcard set
    /// </summary>
    /// <param name="setId">The ID of the flashcard set</param>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    [Route("{setId:int}/cards")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlashCards(int setId,
        CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(setId, cancellationToken);

        if (flashcardSet is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }
        
        return Ok(JsonConvert.SerializeObject(flashcardSet.Cards));
    }
    
    #endregion
}