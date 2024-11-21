using ApiServer.Api.Common.Models;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiServer.Api.FlashcardSets.Models;
using ApiServer.Api.FlashcardSets.Models;
using OpenIddict.Validation.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace ApiServer.Api.FlashcardSets.Controllers;

[Route("api/sets")]
[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
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
        
        return Ok(setData);
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

        var username = HttpContext.User.Identity!.Name;
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

        if (user is null)
        {
            _logger.LogError("Non existent user [{username}]", username);
            var error = new Error("Cannot find user record [" + username + "]");
            return NotFound(error);
        }
        
        var flashcardSet = new FlashcardSet(createCommand.Name, user.Id);
        createCommand.Cards.ForEach(x => flashcardSet.AddCard(x.Question, x.Answer, x.Difficulty));
        await _context.FlashcardSets.AddAsync(flashcardSet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetFlashcardSet),
            new { setId = flashcardSet.Id },
            flashcardSet);
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
    [ProducesResponseType(typeof(FlashcardSetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlashcardSet(int setId,
        CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Cards)
            .Include(x => x.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == setId, cancellationToken);
        
        if (flashcardSet is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(error);
        }

        var comments = await _context.Comments
            .Include(x => x.Author)
            .Where(x => x.FlashcardSetId == setId)
            .ToListAsync(cancellationToken);

        var responseDto = new FlashcardSetDto(flashcardSet, comments);

        return Ok(responseDto);
    }

    /// <summary>
    /// Update a flashcard set
    /// </summary>
    /// <param name="setId"></param>
    /// <param name="updateCommand"></param>
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
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested PUT /sets/{setId}", username, setId);
        
        var flashcardSet = await _context.FlashcardSets
            .Include(x  => x.Cards)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == setId, cancellationToken);
        
        if (flashcardSet is null)
        {
            _logger.LogError("Non existent flashcard set [{setId}]", setId);
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(error);
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (user is null || flashcardSet.UserId != user.Id)
        {
            _logger.LogError("Attempt to update flashcard set not made by owner [{username}]", username);
            var error = new Error("You cannot update a flashcard set that you do not own");
            return BadRequest(error);
        }
        
        flashcardSet.Update(updateCommand.Name);
        flashcardSet.ClearCards();
        await _context.SaveChangesAsync(cancellationToken);
        
        updateCommand.Cards.ForEach(x => flashcardSet.AddCard(x.Question, x.Answer, x.Difficulty));
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(flashcardSet);
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
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested DELETE /sets/{setId}", username, setId);
        
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(setId, cancellationToken);

        if (flashcardSet is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(error);
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (user is null || flashcardSet.UserId != user.Id)
        {
            _logger.LogError("Attempt to update flashcard set not made by owner [{username}]", username);
            var error = new Error("You cannot update a flashcard set that you do not own");
            return BadRequest(error);
        }

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
        [FromBody] CommentRequest commentRequest, 
        CancellationToken cancellationToken)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Cards)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x=> x.Id == setId, cancellationToken);

        if (flashcardSet is null)
        {
            var error = new Error($"Cannot find Flashcard set with ID [{setId}]");
            return NotFound(error);
        }

        var user = await _context.Users.FirstAsync(cancellationToken);

        var comment = new Comment(commentRequest.Comment, flashcardSet, user);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetFlashcardSet),
            new { setId = flashcardSet.Id },
            comment);
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
            .Include(x => x.Cards)
            .FirstOrDefaultAsync(x => x.Id == setId, cancellationToken);

        if (flashcardSet is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + setId + "]");
            return NotFound(error);
        }
        
        return Ok(flashcardSet.Cards);
    }
    
    #endregion
}