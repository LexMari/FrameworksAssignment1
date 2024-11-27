using ApiServer.Api.FlashcardSets.Models;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace ApiServer.Api.FlashcardSets.Controllers;

/// <summary>
/// Controlled for processing all API routes associated with Flashcard sets
/// </summary>
[Route("api/sets")]
[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class FlashcardSetController : Controller
{
    private readonly ILogger<FlashcardSetController> _logger;
    private readonly ApiContext _context;
    
    private const string LimitSettingKey = "SET_LIMIT_DAY";

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
        var username = HttpContext.User.Identity!.Name ?? "UNKNOWN";
        _logger.LogDebug("User [{username}] requested GET /sets", username);
        
        var setData = await _context.FlashcardSets
            .Include(x => x.Cards)
            .ToListAsync(cancellationToken);
        
        return Ok(setData);
    }

    /// <summary>
    /// Create a flashcard set
    /// </summary>
    /// <param name="createCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created user</returns>
    /// <response code="201">Returns the newly created flashcard set</response>
    /// <response code="400">If the flashcard set could not be created</response>
    /// <response code="429">Exceeded limit</response>
    [HttpPost]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CreateFlashcardSet(
        [FromBody] FlashcardSetData createCommand,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested POST /sets", username);
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (user is null)
        {
            _logger.LogError("Attempt to update flashcard set not made by owner [{username}]", username);
            return Problem(
                title: "User not authenticated.",
                detail: $"User '{username}' is not a valid user.",
                statusCode: StatusCodes.Status401Unauthorized
            );
        }
        
        
        var setting = await _context.ApiSettings.FindAsync(LimitSettingKey);
        if (setting is not null && setting.IntegerValue > 0 && !user.IsAdministrator)
        {
            var limit = setting.IntegerValue;
            
            var createdCount = _context.FlashcardSets.Count(x =>
                x.CreatedAt.Date == DateTime.Today);
            
            if (createdCount >= limit)
            {
                _logger.LogError("The daily limit for flashcard set creation has been reached [{username}] - {limit} sets", username, limit);
                return Problem(
                    title: "Flashcard set limit reached",
                    detail: $"The maximum number of flashcard sets that can be created today has been reached [{limit}]",
                    statusCode: StatusCodes.Status429TooManyRequests
                );
            }
        }
        
        try
        {
            var flashcardSet = new FlashcardSet(createCommand.Name, user.Id);
            createCommand.Cards.ForEach(x => flashcardSet.AddCard(x.Question, x.Answer, x.Difficulty));
            await _context.FlashcardSets.AddAsync(flashcardSet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(
                nameof(GetFlashcardSet), 
                new {setId = flashcardSet.Id},
                flashcardSet);
        }
        catch (Exception ex)
        {
            _context.ChangeTracker.Clear();
            _logger.LogError(ex, "Failed to create flashcard set");
            return Problem(
                title: "Failed to create flashcard set.",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
    }

    #endregion

    #region /set/{setId} route actions

    /// <summary>
    /// Get a flashcard set by ID
    /// </summary>
    /// <param name="setId">The ID of the flashcard set</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{setId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlashcardSet(int setId,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /sets/{setId}", username, setId);

        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Cards)
            .Include(x => x.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == setId, cancellationToken);
            
        if (flashcardSet is null)
        {
            _logger.LogError("Non existent flashcard set [{setId}]", setId);
            return Problem(
                title: "Flashcard set not found",
                detail: $"Cannot find Flashcard set with ID [{setId}]",
                statusCode: StatusCodes.Status404NotFound
            );
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
    /// <param name="setId">The ID of the flashcard set</param>
    /// <param name="updateCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{setId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
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
            _logger.LogError("Flashcard set not found [{setId}]", setId);
            return Problem(
                title: "Flashcard set not found",
                detail: $"Cannot find Flashcard set with ID [{setId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (user is null || flashcardSet.UserId != user.Id)
        {
            _logger.LogError("Attempt to update flashcard set not made by owner [{username}]", username);
            return Problem(
                title: "Update not permitted",
                detail: $"You cannot update a flashcard set that you do not own [{setId}]",
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        
        flashcardSet.Update(updateCommand.Name);
        flashcardSet.ClearCards();
        await _context.SaveChangesAsync(cancellationToken);
        
        updateCommand.Cards.ForEach(x => flashcardSet.AddCard(x.Question, x.Answer, x.Difficulty));
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(flashcardSet);
    }
    
    /// <summary>
    /// Delete the flashcard set with the passed ID
    /// </summary>
    /// <param name="setId">The ID of the flashcard set</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{setId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFlashcardSet(int setId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested DELETE /sets/{setId}", username, setId);
        
        var flashcardSet = await _context.FlashcardSets
            .FindAsync(setId, cancellationToken);

        if (flashcardSet is null)
        {
            _logger.LogError("Flashcard set not found [{setId}]", setId);
            return Problem(
                title: "Flashcard set not found",
                detail: $"Cannot find Flashcard set with ID [{setId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (user is null || flashcardSet.UserId != user.Id)
        {
            _logger.LogError("Attempt to delete flashcard set not made by owner [{username}]", username);
            return Problem(
                title: "Delete not permitted",
                detail: $"You cannot update a flashcard set that you do not own [{setId}]",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        _context.FlashcardSets.Remove(flashcardSet);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new NoContentResult();
    }
    
    #endregion
    
    #region /set/(setId}/comment route actions
    
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddComment(int setId, 
        [FromBody] CommentRequest commentRequest, 
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested POST /sets/{setId}/comment", username, setId);
        
        var flashcardSet = await _context.FlashcardSets
            .Include(x  => x.Cards)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == setId, cancellationToken);

        if (flashcardSet is null)
        {
            _logger.LogError("Flashcard set not found [{setId}]", setId);
            return Problem(
                title: "Flashcard set not found",
                detail: $"Cannot find Flashcard set with ID [{setId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (user is null)
        {
            _logger.LogError("Unknown user [{username}]", username); return Problem(
                title: "Current user not found",
                detail: $"Cannot find user [{username}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        var comment = new Comment(commentRequest.Comment, flashcardSet, user);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);
        
        return CreatedAtAction(
            nameof(GetFlashcardSet), 
            new {setId = flashcardSet.Id},
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlashCards(int setId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /sets/{setId}/cards", username, setId);
        
        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Cards)
            .FirstOrDefaultAsync(x => x.Id == setId, cancellationToken);

        if (flashcardSet is null)
        {
            _logger.LogError("Flashcard set not found [{setId}]", setId);
            return Problem(
                title: "Flashcard set not found",
                detail: $"Cannot find Flashcard set with ID [{setId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        return Ok(flashcardSet.Cards);
    }
    
    #endregion
}