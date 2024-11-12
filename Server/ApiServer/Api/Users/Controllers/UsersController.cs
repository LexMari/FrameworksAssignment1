using System.Text.Json;
using ApiServer.Api.Common.Models;
using ApiServer.Api.FlashcardSets.Controllers;
using ApiServer.Api.Users.Models;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Api.Users.Controllers;

/// <summary>
/// Controller to implement /users API endpoints
/// </summary>
[ApiController]
[Route("users")]
public class UsersController : Controller
{
    private ILogger<FlashcardSetController> _logger;
    private readonly ApiContext _context;

    private JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    #region Constructor
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public UsersController(ILogger<FlashcardSetController> logger,
        ApiContext dbContext)
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
    [ProducesResponseType(typeof(List<User>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var userData = await _context.Users
            .ToListAsync(cancellationToken);

        return Ok(JsonSerializer.Serialize(userData, _jsonSerializerOptions));
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
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(
        [FromBody] UserParametersRecord data,
        CancellationToken cancellationToken)
    {
        var user = new User(data.User.Id, data.User.Username, data.Password, data.User.Admin);
        try
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Created(nameof(GetUser), JsonSerializer.Serialize(user, _jsonSerializerOptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create users");
            var error = new Error("Unable to create user record");
            return BadRequest(JsonSerializer.Serialize(error, _jsonSerializerOptions));
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
    [ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
        }

        return Ok(JsonSerializer.Serialize(user, _jsonSerializerOptions));
    }
    
    /// <summary>
    /// Update a user and password
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    [HttpPut]
    [Route("{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(int userId, 
        [FromBody] UserParametersRecord data,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
        }
        
        user.Update(data.User.Username, data.Password, data.User.Admin);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(JsonSerializer.Serialize(user, _jsonSerializerOptions));
    }
    
    /// <summary>
    /// Delete the user with the passed ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken"></param>
    [HttpDelete]
    [Route("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find user with ID [" + userId + "]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
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
    [ProducesResponseType(typeof(List<FlashcardSet>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserFlashcardSets(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FindAsync(userId, cancellationToken);

        if (user is null)
        {
            var error = new Error("Cannot find User with ID [" + userId + "]");
            return NotFound(JsonSerializer.Serialize(error, _jsonSerializerOptions));
        }
        
        var sets = await _context.FlashcardSets
            .Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);

        return Ok(JsonSerializer.Serialize(sets, _jsonSerializerOptions));
    }
    
    #endregion
}