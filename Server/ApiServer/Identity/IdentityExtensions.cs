using ApiServer.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;

namespace ApiServer.Identity;

public static class IdentityExtensions
{
        public static IServiceCollection AddIdenityServices(this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApiContext>();
            })
            .AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("connect/authorize")
                    .SetLogoutEndpointUris("connect/logout")
                    .SetTokenEndpointUris("connect/token")
                    .SetUserinfoEndpointUris("/connect/userinfo");

                options.RegisterScopes(
                    OpenIddictConstants.Permissions.Scopes.Email, 
                    OpenIddictConstants.Permissions.Scopes.Profile, 
                    OpenIddictConstants.Permissions.Scopes.Roles
                    );

                options.AllowAuthorizationCodeFlow();

                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String("ZHXeT9NInSHtdkTTOr1RoZPuSTxXC6rA6cncv9E5VUc=")));
        
                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                options.DisableAccessTokenEncryption();
        
                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.SetIssuer("https://localhost:7222/");
                options.AddAudiences("apiserver");

                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String("QL9aWhlqmBkLibA9tlWcdh0k7JElqsugsJ8w0ciqteY=")));

                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

        // Register the authorisation service for DI
        services.AddTransient<AuthorisationService>();
        services.AddTransient<ClientsSeeder>();

        return services;
    }
}