using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace ApiServer.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
public class UserInfoController : Controller
{
    private readonly ApiContext _dbContext;

    public UserInfoController(ApiContext dbContext)
        => _dbContext = dbContext;

    [HttpGet("~/connect/userinfo")]
    [HttpPost("~/connect/userinfo")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Userinfo()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == User.Identity!.Name);
        if (user == null)
        {
            return Challenge(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The specified access token is bound to an account that no longer exists."
                }!));
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
            [OpenIddictConstants.Claims.Subject] = user.Username
        };

        if (User.HasScope(OpenIddictConstants.Permissions.Scopes.Email))
        {
            claims[OpenIddictConstants.Claims.Email] = user.Username;
        }

        if (User.HasScope(OpenIddictConstants.Permissions.Scopes.Phone))
        {
            claims[OpenIddictConstants.Claims.PhoneNumber] = string.Empty;
            ;
        }

        claims[OpenIddictConstants.Claims.Username] = user.Username;
        claims[OpenIddictConstants.Claims.Nickname] = user.Id;
        claims[OpenIddictConstants.Claims.Role] = user.IsAdministrator ? "Administrator" : "User";

        return Ok(claims);
    }
}