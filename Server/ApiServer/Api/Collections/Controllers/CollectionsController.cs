using ApiServer.Api.Common.Models;
using ApiServer.Api.Collections.Models;
using ApiServer.Api.Users.Controllers;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace ApiServer.Api.Collections.Controllers;

/// <summary>
/// Controller for /collections endpoints
/// </summary>
[Route("api/collections")]
[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class CollectionsController : Controller
{
    private readonly ILogger<CollectionsController> _logger;
    private readonly ApiContext _context;
    
    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public CollectionsController(
        ILogger<CollectionsController> logger,
        ApiContext dbContext)
    {
        _logger = logger;
        _context = dbContext;
    }
    
    #endregion
    
        #region  Base /collections routes
    
    /// <summary>
    /// Get all flashcard set collections
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Collection>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /collections", username);
        
        var userData = await _context.Collections
            .Include(x => x.User)
            .Include(x => x.FlashcardSets)
            .ThenInclude(x => x.Cards)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
        
        return Ok(userData);
    }
    
    /// <summary>
    /// Create a flashcard set collection
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
    [ProducesResponseType(typeof(Collection), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CreateFlashcardSet(
        [FromBody] CollectionRequest createCommand,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested POST /collections", username);
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (user is null)
        {
            _logger.LogError("Non existent user [{username}]", username);
            var error = new Error("Cannot find current user  [" + username + "]");
            return BadRequest(error);
        }
        
        var collection = new Collection(createCommand.Comment, user);

        foreach (var setId in createCommand.Sets)
        {
            var set = await _context.FlashcardSets.FindAsync(setId, cancellationToken);
            if (set is null)
            {
                _logger.LogWarning("Ignoring attempt to add non-existent flashcard set to collection [{setId}]", setId);
                continue;
            }
            collection.AddFlashcardSet(set);
        }
        
        await _context.Collections.AddAsync(collection, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(UsersController.GetCollection), 
            new {userId = user.Id, collectionId = collection.Id},
            collection);
    }
    
    #endregion
    
    #region /collections/random route
    
    /// <summary>
    /// Get a flashcard set collection by ID
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("random")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Collection), StatusCodes.Status302Found)]
    public async Task<IActionResult> GetCollection(CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /collections/random", username);

        var options = await _context.Collections
            .Select(x => new { x.UserId, CollectionId = x.Id })
            .ToListAsync(cancellationToken);

        if (options.Count == 0)
            return Redirect("/api/sets");
        
        var random = new Random();
        int index = random.Next(options.Count);
        var selected = options[index];
        
        var redirectUrl = $"/api/users/{selected.UserId}/collections/{selected.CollectionId}";
        _logger.LogDebug("Collections redirect to {url}", redirectUrl);
        
        return Redirect(redirectUrl);
    }
    
    #endregion
}