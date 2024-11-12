using ApiServer.Api.Common.Models;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiServer.Api.FlashcardsSets.Models;

namespace ApiServer.Api.FlashcardSets.Controllers;

[Route("sets")]
[ApiController]
public class FlashcardSetController : Controller
{
    private ILogger<FlashcardSetController> _logger;
    private readonly ApiContext _context;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

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
    /// Get all flashcard sets
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<FlashcardSet>), StatusCodes.Status200OK)]
    public async Task<IActionResult>  GetFlashcardSets(CancellationToken cancellationToken)
    {
        var setData = await _context.FlashcardSets
            .Include(x => x.Cards)
            .ToListAsync(cancellationToken);
        
        return Ok(JsonSerializer.Serialize(setData, _jsonSerializerOptions));
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
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CreateFlashcardSet(
        [FromBody] FlashcardSetData createCommand,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(cancellationToken);
        
        var flashcardSet = new FlashcardSet(createCommand.Name, user.Id);
        createCommand.Cards.ForEach(x => flashcardSet.AddCard(x.Question, x.Answer, x.Difficulty));
        await _context.FlashcardSets.AddAsync(flashcardSet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetFlashcardSet),
            new { setId = flashcardSet.Id },
            JsonSerializer.Serialize(flashcardSet, _jsonSerializerOptions));
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
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlashcardSet(int setId,
        CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Cards)
            .FirstOrDefaultAsync(x => x.Id == setId, cancellationToken);
        
        if (flashcardSet is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
        }

        return Ok(JsonSerializer.Serialize(flashcardSet, _jsonSerializerOptions));
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
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFlashcardSet(int setId,
        [FromBody] FlashcardSetData updateCommand,
        CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Cards)
            .FirstOrDefaultAsync(x => x.Id == setId, cancellationToken);

        if (flashcardSet == null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
        }
        
        /* TODO:Verify current user is the set author */
        
        flashcardSet.Update(updateCommand.Name);
        flashcardSet.ClearCards();
        await _context.SaveChangesAsync(cancellationToken);

        updateCommand.Cards.ForEach(x => flashcardSet.AddCard(x.Question, x.Answer, x.Difficulty));
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(JsonSerializer.Serialize(flashcardSet, _jsonSerializerOptions));
    }

    /// <summary>
    /// Delete flashcard set
    /// </summary>
    /// <param name="setId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{setId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFlashcardSet(int setId, CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(setId, cancellationToken);

        if (flashcardSet == null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
        }

        /* TODO: Verify the current user is the set author */
        /* TODO: Delete the comments for the set */

        _context.FlashcardSets.Remove(flashcardSet);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new NoContentResult();
    }

    #endregion

    #region /set/{setId}/comment routes

    /// <summary>
    /// Add a comment to a flashcard set
    /// </summary>
    /// <returns>The created user</returns>
    /// <response code="201">Returns the newly created flashcard set</response>
    /// <response code="404">The flashcard set was not found</response>
    [HttpPost]
    [Route("{setId:int}/comment")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComment(int setId, 
        [FromBody] string comment, 
        CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(setId, cancellationToken);

        if (flashcardSet is null)
        {
            var error = new Error($"Cannot find Flashcard set with ID [{setId}]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
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
    /// <returns></returns>
    [HttpGet]
    [Route("{setId:int}/cards")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<FlashCard>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlashCards(int setId, CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(setId, cancellationToken);

        if (flashcardSet is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
        }
        
        return Ok(JsonSerializer.Serialize(flashcardSet.Cards, _jsonSerializerOptions));
    }
    
    #endregion
}