using System.Collections.Immutable;
using System.Security.Claims;
using ApiServer.Infrastructure;
using ApiServer.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace ApiServer.Controllers;

public class AuthorisationController : Controller
{
    private readonly ILogger<AuthorisationController> _logger;

    private readonly IConfiguration _configuration;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly AuthorisationService _authService;
    private readonly ApiContext _dbContext;


    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    /// <param name="applicationManager"></param>
    /// <param name="scopeManager"></param>
    /// <param name="authService"></param>
    /// <param name="dbContext"></param>
    public AuthorisationController(
        ILogger<AuthorisationController> logger,
        IConfiguration configuration,
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictScopeManager scopeManager,
        AuthorisationService authService,
        ApiContext dbContext)
    {
        _logger = logger;
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
        _authService = authService;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    #endregion
    
    [AllowAnonymous]
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Authorize(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var parameters =
            _authService.ParseOAuthParameters(HttpContext, new List<string> { OpenIddictConstants.Parameters.Prompt });

        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!_authService.IsAuthenticated(result, request))
        {
            return Challenge(
                authenticationSchemes: new [] { CookieAuthenticationDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties
                {
                    RedirectUri = _authService.BuildRedirectUrl(HttpContext.Request, parameters)
                });
        }

        var application = await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                          throw new InvalidOperationException(
                              "Details concerning the calling client application cannot be found.");
        
        var userId = result.Principal.FindFirst(ClaimTypes.Email)!.Value;

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == userId, cancellationToken);
        var role = "User";
        if (user is not null || user.IsAdministrator)
            role = "Administrator";

        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role);

        identity.SetClaim(OpenIddictConstants.Claims.Subject, userId)
            .SetClaim(OpenIddictConstants.Claims.Email, userId)
            .SetClaim(OpenIddictConstants.Claims.Name, userId)
            .SetClaims(OpenIddictConstants.Claims.Role, new List<string> { role }.ToImmutableArray());

        identity.SetScopes(request.GetScopes());
        identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

        identity.SetDestinations(c => AuthorisationService.GetDestinations(identity, c));

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Perform OAUTH token exchange
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [HttpPost("~/connect/token")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Exchange(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
            throw new InvalidOperationException("The specified grant type is not supported.");

        var result =
            await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var userId = result.Principal.GetClaim(OpenIddictConstants.Claims.Subject);

        if (string.IsNullOrEmpty(userId))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Cannot find user from the token."
                }));
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == userId, cancellationToken);
        var role = "User";
        if (user is not null && user.IsAdministrator)
            role = "Administrator";

        var identity = new ClaimsIdentity(result.Principal.Claims,
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role);

        identity.SetClaim(OpenIddictConstants.Claims.Subject, userId)
            .SetClaim(OpenIddictConstants.Claims.Email, userId)
            .SetClaim(OpenIddictConstants.Claims.Name, userId)
            .SetClaims(OpenIddictConstants.Claims.Role, new List<string> { role }.ToImmutableArray());

        identity.SetDestinations(c => AuthorisationService.GetDestinations(identity, c));

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    
    /// <summary>
    /// Allow logout and invalidation of current authorisation
    /// </summary>
    /// <returns></returns>
    [HttpGet("~/connect/logout")]
    [HttpPost("~/connect/logout")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> LogoutPost()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            });
    }
}