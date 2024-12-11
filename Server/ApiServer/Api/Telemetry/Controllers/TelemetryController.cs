using ApiServer.Api.FlashcardSets.Controllers;
using ApiServer.Api.FlashcardSets.Models;
using ApiServer.Api.Telemetry.Models;
using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace ApiServer.Api.Telemetry.Controllers;

/// <summary>
/// Controller for telemetry requests
/// </summary>
[Route("api/telemetry")]
[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class TelemetryController : Controller
{
    private readonly ILogger<TelemetryController> _logger;
    private readonly ApiContext _context;
    
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public TelemetryController(ILogger<TelemetryController> logger, ApiContext dbContext)
    {
        _logger = logger;
        _context = dbContext;
    }

    #endregion

    #region Base /telemetry routes

    /// <summary>
    /// Get all telemetry sessions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<TelemetrySession>), StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
    public async Task<IActionResult>  GetTelemetrySessions(CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name ?? "UNKNOWN";
        _logger.LogDebug("User [{username}] requested GET /telemetry", username);
        
        var sessionData = await _context.TelemetrySessions
            .Include(x => x.User)
            .Include(x => x.FlashcardSet)
            .ToListAsync(cancellationToken);
        
        return Ok(sessionData);
    }

    /// <summary>
    /// Start a telemetry session for the current user on a flashcard set
    /// </summary>
    /// <param name="startCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created flashcard set</returns>
    /// <response code="201">Returns the newly started telemetry session</response>
    /// <response code="404">If the flashcard set could not be foound</response>
    [HttpPost]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TelemetrySession), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartTelemetrySession(
        [FromBody] TelemetrySessionRequest startCommand,
        CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested POST /telemetry", username);
        
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
        
        var flashcardSet = await _context.FlashcardSets.FirstOrDefaultAsync(x => x.Id == startCommand.SetId, cancellationToken);
        if (flashcardSet is null)
        {
            _logger.LogError("Flashcard set not found [{setId}]", startCommand.SetId);
            return Problem(
                title: "Flashcard set not found",
                detail: $"Cannot find Flashcard set with ID [{ startCommand.SetId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var session = new TelemetrySession(user, flashcardSet);
        _context.TelemetrySessions.Add(session);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(session);
    }

    #endregion
    
    #region /telemetry/{sessionId} routes
    
    /// <summary>
    /// Get a telemetry session
    /// </summary>
    /// <param name="sessionId">The ID of the telemetry session</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{sessionId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<TelemetrySession>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
    public async Task<IActionResult> GetTelemetrySession(Guid sessionId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested GET /telemetry/{sessionId}", username, sessionId);
        
        var session = await _context.TelemetrySessions
            .Include(x => x.User)
            .Include(x => x.FlashcardSet)
            .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);

        if (session is null)
        {
            _logger.LogError("Telemetry session not found [{sessionId}]", sessionId);
            return Problem(
                title: "Telemetry session not found",
                detail: $"Cannot find Telemetry session with ID [{sessionId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        return Ok(session);
    }
    
    /// <summary>
    /// Complete a telemetry session
    /// </summary>
    /// <param name="sessionId">The ID of the telemetry session</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{sessionId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<TelemetrySession>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteTelemetrySession(Guid sessionId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested PUT /telemetry/{sessionId}", username, sessionId);
        
        var session = await _context.TelemetrySessions
            .Include(x => x.User)
            .Include(x => x.FlashcardSet)
            .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);

        if (session is null)
        {
            _logger.LogError("Telemetry session not found [{sessionId}]", sessionId);
            return Problem(
                title: "Telemetry session not found",
                detail: $"Cannot find Telemetry session with ID [{sessionId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        try
        {
            session.CompleteSession();
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Telemetry session could not be completed [{sessionId}]", sessionId);
            return Problem(
                title: "Telemetry session could not be completed",
                detail: e.Message,
                statusCode: StatusCodes.Status404NotFound
            );
        }
        return Ok(session);
    }
    
    /// <summary>
    /// Abort/cancel a telemetry session
    /// </summary>
    /// <param name="sessionId">The ID of the telemetry session</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{sessionId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<TelemetrySession>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AbortTelemetrySession(Guid sessionId, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested DELETE /telemetry/{sessionId}", username, sessionId);
        
        var session = await _context.TelemetrySessions
            .Include(x => x.User)
            .Include(x => x.FlashcardSet)
            .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);

        if (session is null)
        {
            _logger.LogError("Telemetry session not found [{sessionId}]", sessionId);
            return Problem(
                title: "Telemetry session not found",
                detail: $"Cannot find Telemetry session with ID [{sessionId}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        try
        {
            session.AbortSession();
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Telemetry session could not be aborted [{sessionId}]", sessionId);
            return Problem(
                title: "Telemetry session could not be aborted",
                detail: e.Message,
                statusCode: StatusCodes.Status404NotFound
            );
        }
        
        return Ok(session);
    }
    
    #endregion
}