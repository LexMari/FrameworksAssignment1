using System.Security.Claims;
using ApiServer.ViewModels;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace ApiServer.Controllers;

public class AuthenticateController : Controller
{
    private readonly ILogger<AuthenticateController> _logger;
    private readonly ApiContext _context;
    
    #region Constructor

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    public AuthenticateController(
        ILogger<AuthenticateController> logger,
        ApiContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    #endregion

    [HttpGet]
    [AllowAnonymous]
    [Route("Authenticate")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// Process login attempt
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("Authenticate")]
    [ValidateAntiForgeryToken]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
    {
        var returnUrl = (string.IsNullOrWhiteSpace(model.ReturnUrl) ? "~/" : model.ReturnUrl);

        if (ModelState.IsValid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == model.Username, cancellationToken);
            if (user == null)
            {
                ModelState.AddModelError("Username", "Unknown username or password.");
                return View(model);
            }

            var signInResult = Domain.Entities.User.VerifyPassword(
                model.Password,
                user.PasswordHash,
                Convert.FromBase64String(user.PasswordSalt));

            if (!signInResult)
            {
                return Unauthorized();
            }
            
            var claims = new List<Claim>()
            {
                new Claim(OpenIddictConstants.Claims.Subject, user.Username),
                new Claim(ClaimTypes.Email, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.IsAdministrator ? "Administrator" : "User"),
                new Claim(ClaimTypes.NameIdentifier, user.Username)
            };
            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            _logger.LogInformation("User logged in {Username}", model.Username);
            return LocalRedirect(returnUrl);
        }

        return View();
    }
}