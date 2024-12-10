using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace ApiServer.Api.Settings.Controllers;

/// <summary>
/// Administrative settings for the ApiService
/// </summary>
[Route("api/settings")]
[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
public class ApiSettingsController : Controller
{
    private readonly ILogger<ApiSettingsController> _logger;
    private readonly ApiContext _context;
    
    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public ApiSettingsController(ILogger<ApiSettingsController> logger, ApiContext dbContext)
    {
        _logger = logger;
        _context = dbContext;
    }
    #endregion
    
    #region Base /settings routes
    /// <summary>
    /// Get ApiServer settings
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<ApiSetting>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSettings(CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name ?? "UNKNOWN";
        _logger.LogDebug("User [{username}] requested GET /settings", username);
        
        var settings = await _context.ApiSettings
            .ToListAsync(cancellationToken);
        
        return Ok(settings);
    }
    
    /// <summary>
    /// Update a setting description and or value
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Route("{key}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiSetting), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSettings(string key, [FromBody] ApiSetting settingData, CancellationToken cancellationToken)
    {
        var username = HttpContext.User.Identity!.Name;
        _logger.LogDebug("User [{username}] requested PUT /settings", username);
        var setting = await _context.ApiSettings
            .FirstOrDefaultAsync(x => x.Id == settingData.Id, cancellationToken);
        
        if (setting is null)
        {
            _logger.LogError("Setting not found [{key}]", settingData.Id);
            return Problem(
                title: "Setting not found",
                detail: $"Setting not found [{settingData.Id}]",
                statusCode: StatusCodes.Status404NotFound
            );
        }
        if (!ApiSetting.ValidateValue(setting.Type, settingData.Value))
        {
            _logger.LogError("Invalid value for setting [{key}] - {Type} --> {Value}",
                setting.Id, 
                setting.Type.ToString(),
                settingData.Value);
            return Problem(
                title: "Invalid setting value",
                detail: $"Setting [{setting.Id}] expects a value that can be converted to {setting.Type}",
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        
        setting.Update(settingData.Description, settingData.Value); 
        await _context.SaveChangesAsync(cancellationToken);
        return Ok(setting);
    }
    
    #endregion
}