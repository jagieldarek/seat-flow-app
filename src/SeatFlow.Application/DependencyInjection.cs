using Microsoft.Extensions.DependencyInjection;
using SeatFlow.Application.Events;

namespace SeatFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<EventQueries>();

        return services;
    }
}
