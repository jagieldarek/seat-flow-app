using SeatFlow.Application.Events;

namespace SeatFlow.Api.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints(this IEndpointRouteBuilder app)
    {
        var events = app.MapGroup("/api/events").WithTags("Events");

        events.MapGet("", async (EventQueries queries, CancellationToken cancellationToken) =>
            TypedResults.Ok(await queries.GetOnSaleAsync(cancellationToken)));
    }
}
