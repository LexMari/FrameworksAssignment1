using ApiServer.Infrastructure;
using OpenIddict.Abstractions;

namespace ApiServer.Identity;

public class ClientsSeeder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ClientsSeeder> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    public ClientsSeeder(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<ClientsSeeder> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Add application scopes
    /// </summary>
    public async Task AddScopes()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        var apiScope = await manager.FindByNameAsync("testvarapi");

        if (apiScope != null)
        {
            await manager.DeleteAsync(apiScope);
        }

        await manager.CreateAsync(new OpenIddictScopeDescriptor
        {
            DisplayName = "TestVar API scope",
            Name = "testvarapi",
            Resources =
            {
                "apiserver"
            }
        });
        
        _logger.LogDebug("Added identity API scope [TestVarAPI]");
    }
    
    /// <summary>
    /// Add application clients
    /// </summary>
    public async Task AddClients()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        await context.Database.EnsureCreatedAsync();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        
        var client = await manager.FindByClientIdAsync("swagger-client");
        if (client != null)
        {
            await manager.DeleteAsync(client);
        }

        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "swagger-client",
            ClientSecret = "EA5CD13A-472E-4CC4-A901-E51F0586B876",
            DisplayName = "Swagger client application",
            ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
            RedirectUris =
            {
                new Uri("https://localhost:7222/swagger/oauth2-redirect.html")
            },
            PostLogoutRedirectUris =
            {
                new Uri("https://localhost:7222/")
            },
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Authorization,
                OpenIddictConstants.Permissions.Endpoints.Logout,
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                OpenIddictConstants.Permissions.ResponseTypes.Code,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles,
                $"{OpenIddictConstants.Permissions.Prefixes.Scope}testvarapi"
            },
        });
        _logger.LogInformation("Registered Swagger.Client client for introspection");

        var reactClient = await manager.FindByClientIdAsync(_configuration["Identity:TestVarSpa:ClientId"]);
        if (reactClient != null)
        {
            await manager.DeleteAsync(reactClient);
        }

        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = _configuration["Identity:TestVarSpa:ClientId"],
            ClientSecret = _configuration["Identity:TestVarSpa:ClientSecret"],
            DisplayName = _configuration["Identity:TestVarSpa:DisplayName"],
            ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
            RedirectUris =
            {
                new Uri($"{_configuration["Identity:TestVarSpa:ApplicationUrl"]}/oauth/callback")
            },
            PostLogoutRedirectUris =
            {
                new Uri($"{_configuration["Identity:TestVarSpa:ApplicationUrl"]}/")
            },
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Authorization,
                OpenIddictConstants.Permissions.Endpoints.Logout,
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                OpenIddictConstants.Permissions.ResponseTypes.Code,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles,
                $"{OpenIddictConstants.Permissions.Prefixes.Scope}testvarapi"
            },
        });
        
        _logger.LogInformation("Registered TestVar.SPA client for introspection");
    }
    
}