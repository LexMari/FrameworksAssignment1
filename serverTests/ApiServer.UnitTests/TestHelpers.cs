using ApiServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer.UnitTests;

public class TestHelpers
{
    private static readonly IConfigurationRoot _configuration;
    
    private static readonly IServiceProvider _services;
    
    static TestHelpers()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);
        _configuration = builder.Build();
        
        var serviceCollection = new ServiceCollection();
            
        serviceCollection.AddDbContext<ApiContext>(options =>
            options
                .UseSqlite(
                    "Data Source=ApiServerTest.db;Cache=Shared",
                    b =>
                    {
                        b.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds);
                        b.MigrationsAssembly(typeof(ApiContext).Assembly.FullName);
                    })
                .EnableSensitiveDataLogging()
        );
            
        _services = serviceCollection.BuildServiceProvider();
        using var scope = _services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<ApiContext>();
        
        try
        {
            appContext.Database.EnsureDeleted();
            appContext.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            
            throw;
        }
    }
    
    public static IConfigurationRoot GetConfiguration()
    {
        return _configuration;
    }
    
    public static IServiceProvider GetServices()
    {
        return _services;
    }
}