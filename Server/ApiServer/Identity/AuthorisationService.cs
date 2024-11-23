using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;

namespace ApiServer.Identity;

public class AuthorisationService
{
    /// <summary>
    /// Parse OAuth parameters recieved from authorisation request
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="excluding"></param>
    /// <returns></returns>
    public IDictionary<string, StringValues> ParseOAuthParameters(
        HttpContext httpContext,
        List<string>? excluding = null)
    {
        excluding ??= new List<string>();
        
        //Parse parameters into a dictionary
        var parameters = httpContext.Request.HasFormContentType
            ? httpContext.Request.Form
                .Where(v => !excluding.Contains(v.Key))
                .ToDictionary(v => v.Key, v => v.Value)
            : httpContext.Request.Query
                .Where(v => !excluding.Contains(v.Key))
                .ToDictionary(v => v.Key, v => v.Value);

        return parameters;
    }

    /// <summary>
    /// Construct OAuth redirect URL
    /// </summary>
    /// <param name="request"></param>
    /// <param name="oAuthParameters"></param>
    /// <returns></returns>
    public string BuildRedirectUrl(HttpRequest request, IDictionary<string, StringValues> oAuthParameters)
    {
        var url = request.PathBase + request.Path + QueryString.Create(oAuthParameters);
        return url;
    }

    /// <summary>
    /// Check for existing authenticated state
    /// </summary>
    /// <param name="authenticateResult"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public bool IsAuthenticated(AuthenticateResult authenticateResult, OpenIddictRequest request)
    {
        //Check authentication result is successful, else false
        if (!authenticateResult.Succeeded)
        {
            return false;
        }

        //Validate current authentication is within max session age
        if (request.MaxAge.HasValue && authenticateResult.Properties != null)
        {
            //Convert from seconds to timespan
            var maxAgeSeconds = TimeSpan.FromSeconds(request.MaxAge.Value);

            //Check if timestamp is valid, else missing/exceeds then return expired
            var expired = !authenticateResult.Properties.IssuedUtc.HasValue ||
                          DateTimeOffset.UtcNow - authenticateResult.Properties.IssuedUtc > maxAgeSeconds;
            if (expired)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Validate destination for the passed claim
    /// </summary>
    /// <param name="identity"></param>
    /// <param name="claim"></param>
    /// <returns></returns>
    public static List<string> GetDestinations(ClaimsIdentity identity, Claim claim)
    {
        var destinations = new List<string>();

        if (claim.Type is OpenIddictConstants.Claims.Name or 
            OpenIddictConstants.Claims.Email or 
            OpenIddictConstants.Claims.Role)
        {
            destinations.Add(OpenIddictConstants.Destinations.AccessToken);
        }

        return destinations;
    }
}