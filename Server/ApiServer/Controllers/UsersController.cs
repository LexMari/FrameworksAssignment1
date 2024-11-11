using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using ApiServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiServer.Controllers;

[ApiController]
[Route("users")]
public class UsersController : Controller
{
    private ILogger<UsersController> _logger;
    private readonly ApiContext _context;

    #region Constructor
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public UsersController(ILogger<UsersController> logger, ApiContext dbContext)
    {
        _logger = logger;
        _context = dbContext;
    }
    
    #endregion
    
    #region Base /users routes
    
    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var userData = await _context.Users
            .ToListAsync(cancellationToken);
        
        return Ok(JsonConvert.SerializeObject(userData));
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created user</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user could not be created</response>
    [HttpPost]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(
        [FromBody] UserParametersRecord data,
        CancellationToken cancellationToken)
    {
        var user = new User(data.User.Id, data.User.Username, data.Password, data.User.Admin);
        try
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Created(nameof(GetUser), JsonConvert.SerializeObject(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create users");
            var error = new Error("Unable to create user record");
            return BadRequest(error);
        }
    }
    
    #endregion
    
    #region /users/{userId} routes

    /// <summary>
    /// Get a user by ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    [Route("{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }
        
        return Ok(JsonConvert.SerializeObject(user));
    }
    
    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    [HttpPut]
    [Route("{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(int userId, 
        [FromBody] UserParametersRecord data,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }
        
        user.Update(data.User.Username, data.Password, data.User.Admin);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Ok(JsonConvert.SerializeObject(user));
    }
    
    /// <summary>
    /// Delete the flashcard set with the passed ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken"></param>
    [HttpDelete]
    [Route("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find Flashcard set with ID [" + userId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }

        // Remove comments for the user
        // Remove collections for the user
        // Remove flashcards sets for the user
        
        /*
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return new NoContentResult();
        */
        
        throw new NotImplementedException();
    }
    
    #endregion
    
    #region /users/{userId}/sets routes
    
    /// <summary>
    /// Get all the flashcard sets created a user by ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{userId:int}/sets")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserFlashcardSets(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(JsonConvert.SerializeObject(error));
        }
        
        var sets = await _context.FlashcardSets
            .Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);
        
        return Ok(sets);
    }
    
    #endregion
}