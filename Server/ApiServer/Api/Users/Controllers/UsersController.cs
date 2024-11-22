using ApiServer.Api.Collections.Models;
using ApiServer.Api.Common.Models;
using ApiServer.Api.Users.Models;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace ApiServer.Api.Users.Controllers;

/// <summary>
/// Controller to  implement the /users API endpoints
/// </summary>
[Route("api/users")]
[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;
    private readonly ApiContext _context;

    #region Constructor
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public UsersController(
        ILogger<UsersController> logger, 
        ApiContext dbContext)
    {
        _logger = logger;
        _context = dbContext;
    }
    
    #endregion
    
    #region  Base /users routes
    
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /users", username);
        
        var userData = await _context.Users
            .ToListAsync(cancellationToken);
        
        return Ok(userData);
    }

    /// <summary>
    /// Create a new user. The endpoint can be called anonymously to register a users,
    /// however, only an authenticated requests by an Administrator can set the IsAdministrator field
    /// on the created user.
    /// </summary>
    /// <param name="createUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created user</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user could not be created</response>
    [HttpPost]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(
        [FromBody] UserRequest createUser,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name ?? "ANONYMOUS";
        _logger.LogDebug("User [{username}] requested POST /users", username);
        
        // Check for an authenticated administrator
        var isAdmin = false;
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
            isAdmin = (currentUser!.IsAdministrator && createUser.Admin);
        }
        
        var user = new User(createUser.Username, createUser.Password, isAdmin);
        try
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Created(nameof(GetUser), user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create users");
            var error = new Error("Unable to create the user record");
            return BadRequest(error);
        }
    }

    #endregion
    
    #region /users/{userId} routes actions

    /// <summary>
    /// Get a user by ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int userId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /users/{userId}", username, userId);
        
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            _logger.LogError("Cannot find User with ID [{userId}]", userId);
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(error);
        }
        
        return Ok(user);
    }

    /// <summary>
    /// Update a user and their password. The update request my either by made by the user themself or an
    /// administrator. Only an administrator update he IsAdministrator field.
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="updateUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(int userId, 
        [FromBody] UserRequest updateUser,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested PUT /users/{userId}", username, userId);

        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            _logger.LogError("Cannot find User with ID [{userId}]", userId);
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(error);
        }
        
        // Get the current user
        var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (currentUser is not null &&
            currentUser.Id != userId &&
            !currentUser.IsAdministrator)
        {
            _logger.LogError("Only an administrator or the user themself may perform an update {userId}", userId);
            var error = new Error("Only an administrator or the user themself may perform an update [" + userId + "]");
            return BadRequest(error);
        }
        var isAdmin = (currentUser!.IsAdministrator && updateUser.Admin);
        
        user.Update(updateUser.Username, updateUser.Password, isAdmin);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Ok(user);
    }
    
    /// <summary>
    /// Delete the flashcard set with the passed ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested DELETE /users/{userId}", username, userId);
        
        var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (currentUser is null || !currentUser.IsAdministrator)
        {
            _logger.LogError("Only an administrator can delete a user [{userId}]", userId);
            return Forbid();
        }
        
        var user = await _context.Users.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            _logger.LogError("Cannot find User with ID [{userId}]", userId);
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(error);
        }

        /* TODO: Remove collections for this user */
        
        // Remove any flashcard sets (and comments)
        var sets = await _context.FlashcardSets
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
        _context.FlashcardSets.RemoveRange(sets);
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return new NoContentResult();
    }
    
    #endregion
    
    #region /user/{userId}/sets route actions
    
    /// <summary>
    /// Get all the flashcard sets created a user by ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{userId:int}/sets")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<FlashcardSet>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserFlashcardSets(int userId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /users/{userId}/sets", username, userId);
        
        var user = await _context.Users.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            _logger.LogError("Cannot find User with ID [{userId}]", userId);
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(error);
        }
        
        var sets = await _context.FlashcardSets
            .Include(x => x.Cards)
            .Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);
        
        return Ok(sets);
    }
    
    #endregion
    
    #region #region /users/{userId}/collections route actions
    
    /// <summary>
    /// Get all flashcard set collections for the specified user account
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("{userId}/collections")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Collection), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(int userId,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /users/{userId}/collections", username, userId);
        
        var userData = await _context.Collections
            .Include(x => x.User)
            .Include(x => x.FlashcardSets)
            .ThenInclude(x => x.Cards)
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
        
        return Ok(userData);
    }
    
    #endregion
    
    #region /users/{userId}/collections/{collectionId} route actions

    /// <summary>
    /// Get a flashcard set collection by user and ID
    /// </summary>
    /// <param name="userId">The ID owning user</param>
    /// <param name="collectionId">The ID of the flashcard set collection</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{userId}/collections/{collectionId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Collection), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCollection(int userId, int collectionId,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /users/{userId}/collections/{collectionId}", username, userId, collectionId);

        var collection = await _context.Collections   
            .Include(x => x.User)
            .Include(x => x.FlashcardSets)
            .ThenInclude(x => x.Cards)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == collectionId, cancellationToken);
            
        if (collection is null)
        {
            _logger.LogError("Flashcard set collection not found for user ID {userId} [{collectionId}]", userId, collectionId);
            return Problem(
                title: "Flashcard set collection not found.",
                detail: $"Flashcard set collection not found for user ID {userId} [{collectionId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        return Ok(collection);
    }

    /// <summary>
    /// Update a flashcard set collection by user and ID
    /// </summary>
    /// <param name="userId">The ID owning user</param>
    /// <param name="collectionId">The ID of the flashcard set collection</param>
    /// <param name="updateCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{userId}/collections/{collectionId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Collection), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCollection(int userId, int collectionId,
        [FromBody] CollectionRequest updateCommand,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /users/{userId}/collections/{collectionId}", username, userId, collectionId);

        var collection = await _context.Collections   
            .Include(x => x.FlashcardSets)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == collectionId, cancellationToken);
            
        if (collection is null)
        {
            _logger.LogError("Flashcard set collection not found for user ID {userId} [{collectionId}]", userId, collectionId);
            return Problem(
                title: "Flashcard set collection not found.",
                detail: $"Flashcard set collection not found for user ID {userId} [{collectionId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
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

        if (collection.UserId != user.Id)
        {
            _logger.LogError("User '{username}' is not allowed to update this collection [{collectionId}]", username, collectionId);
            return Problem(
                title: "Authenticated user is not authorized.",
                detail: $"User '{username}' is not allowed to update this collection.",
                statusCode: StatusCodes.Status403Forbidden
            );    
        }
        
        collection.Update(updateCommand.Comment);
        collection.ClearFlashcardSets();
        foreach (var setId in updateCommand.Sets)
        {
            var set = await _context.FlashcardSets.FindAsync(setId, cancellationToken);
            if (set is null)
            {
                _logger.LogWarning("Ignoring attempt to add non-existent flashcard set to collection [{setId}]", setId);
                continue;
            }
            collection.AddFlashcardSet(set);
        }
        await _context.SaveChangesAsync(cancellationToken);
        
        return Ok(collection);
    }
    
    /// <summary>
    /// Delete a flashcard set collection by user and ID
    /// </summary>
    /// <param name="userId">The ID owning user</param>
    /// <param name="collectionId">The ID of the flashcard set collection</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{userId}/collections/{collectionId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Collection), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCollection(int userId, int collectionId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested DELETE /users/{userId}/collections/{collectionId}", username, userId, collectionId);

        var collection = await _context.Collections   
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == collectionId, cancellationToken);
            
        if (collection is null)
        {
            _logger.LogError("Flashcard set collection not found for user ID {userId} [{collectionId}]", userId, collectionId);
            return Problem(
                title: "Flashcard set collection not found.",
                detail: $"Flashcard set collection not found for user ID {userId} [{collectionId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
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

        if (collection.UserId != user.Id)
        {
            _logger.LogError("User '{username}' is not allowed to update this collection [{collectionId}]", username, collectionId);
            return Problem(
                title: "Authenticated user is not authorized.",
                detail: $"User '{username}' is not allowed to delete this collection.",
                statusCode: StatusCodes.Status403Forbidden
            );    
        }
        
        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    #endregion
}