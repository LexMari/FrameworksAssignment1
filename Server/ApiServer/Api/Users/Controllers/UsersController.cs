using System.Text.Json;
using System.Text.Json.Serialization;
using ApiServer.Api.Common.Models;
using ApiServer.Api.FlashcardSets.Controllers;
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
    private readonly ILogger<FlashcardSetController> _logger;
    private readonly ApiContext _context;

    #region Constructor
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public UsersController(
        ILogger<FlashcardSetController> logger, 
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
        var userData = await _context.Users
            .ToListAsync(cancellationToken);
        
        return Ok(userData);
    }

    /// <summary>
    /// Create a new user
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
        var user = new User(createUser.Username, createUser.Password, createUser.Admin);
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
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(error);
        }
        
        return Ok(user);
    }

    /// <summary>
    /// Update a user and their password
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="updateUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(int userId, 
        [FromBody] UserRequest updateUser,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(error);
        }
        
        user.Update(updateUser.Username, updateUser.Password, updateUser.Admin);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Ok(user);
    }
    
    /// <summary>
    /// Delete the user with the passed ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + userId + "]");
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
        var user = await _context.Users.FindAsync(userId, cancellationToken);
        if (user is null)
        {
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
}