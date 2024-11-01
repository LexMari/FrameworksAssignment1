
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
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
    
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers()
    {
        var responseData = await _context.Users
            .Select(x=> new UserDto(x.Id, x.Username, x.IsAdministrator))
            .ToListAsync();
        
        return Ok(JsonConvert.SerializeObject(responseData));
    }
    
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="createUser"></param>
    /// <returns>The created user</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user could not be created</response>
    [HttpPost]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUser)
    {
        var user = new User(createUser.Id, createUser.Username, createUser.Password, createUser.Admin);
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var responseData = new UserDto(user.Id, user.Username, user.IsAdministrator);
            return Created(nameof(GetUser), JsonConvert.SerializeObject(responseData));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create users");
            return BadRequest("Unable to create user record");
        }
    }
    
    /// <summary>
    /// Get a user by ID
    /// </summary>
    /// <param name="id">The ID of the user</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users
            .FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        var responseData = new UserDto(user.Id, user.Username, user.IsAdministrator);
        return Ok(JsonConvert.SerializeObject(responseData));
    }
    
    #region DTO
    
    private record UserDto(int Id, string Username, bool Admin);
    public record CreateUserDto(int Id, string Username, string Password, bool Admin);

    #endregion
}