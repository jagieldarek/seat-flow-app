using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeatFlow.Infrastructure.Persistence;

namespace SeatFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SeatFlow")
            ?? throw new InvalidOperationException("Connection string 'SeatFlow' is not configured.");

        services.AddDbContext<SeatFlowDbContext>(options => options.UseNpgsql(connectionString));
        services.AddHealthChecks()
            .AddDbContextCheck<SeatFlowDbContext>();

        return services;
    }
}
