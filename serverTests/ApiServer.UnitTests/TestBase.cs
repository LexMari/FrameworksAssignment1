using System.Security.Claims;
using ApiServer.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;

namespace ApiServer.UnitTests;

public class TestBase
{
    
    protected IServiceProvider _provider;
#pragma warning disable NUnit1032
    
    protected ApiContext _context;
#pragma warning restore NUnit1032
    
    [SetUp]
    public void BaseSetUp()
    {
        _provider = TestHelpers.GetServices();
        _context = _provider.GetRequiredService<ApiContext>();
    }
    
    protected ClaimsPrincipal GetStudentPrincipal()
    {
        var fakeClaims = new List<Claim>()
        {
            new Claim(OpenIddictConstants.Claims.Name, "student"),
            new Claim(OpenIddictConstants.Claims.Email,"student"),
            new Claim(OpenIddictConstants.Claims.Role, "User"),
            new Claim(OpenIddictConstants.Claims.Nickname,"1"),
        };
        var fakeIdentity = new ClaimsIdentity(fakeClaims, 
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role
            );
        
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        return fakeClaimsPrincipal;
    }
    
    protected ClaimsPrincipal GetAdminPrincipal()
    {
        var fakeClaims = new List<Claim>()
        {
            new Claim(OpenIddictConstants.Claims.Name, "admin"),
            new Claim(OpenIddictConstants.Claims.Email,"admin"),
            new Claim(OpenIddictConstants.Claims.Role, "Administrator"),
            new Claim(OpenIddictConstants.Claims.Nickname,"2"),
        };
        var fakeIdentity = new ClaimsIdentity(fakeClaims, 
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role
        );
        
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        return fakeClaimsPrincipal;
    }
    
    [OneTimeTearDown]
    protected void TearDown()
    {
        _context.Dispose();
    }
}