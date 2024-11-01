using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApiContext>(options =>
            options
                .UseSqlite(
                    configuration.GetConnectionString("ApiContext"),
                    b =>
                    {
                        b.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds);
                        b.MigrationsAssembly(typeof(ApiContext).Assembly.FullName);
                    })
        );
        
        return services;
    }
}