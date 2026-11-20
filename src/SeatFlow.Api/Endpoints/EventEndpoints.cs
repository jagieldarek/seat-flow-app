using Microsoft.AspNetCore.Http.HttpResults;
using SeatFlow.Application.Events;

namespace SeatFlow.Api.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints(this IEndpointRouteBuilder app)
    {
        var events = app.MapGroup("/api/events").WithTags("Events");

        events.MapGet("", GetOnSaleEvents);
        events.MapGet("/{id:guid}", GetEvent);
    }

    private static async Task<Ok<IReadOnlyList<EventSummary>>> GetOnSaleEvents(EventQueries queries, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await queries.GetOnSaleAsync(cancellationToken));
    }

    private static async Task<Results<Ok<EventDetails>, NotFound>> GetEvent(Guid id, EventQueries queries, CancellationToken cancellationToken)
    {
        var eventDetails = await queries.GetOnSaleByIdAsync(id, cancellationToken);

        return eventDetails is null ? TypedResults.NotFound() : TypedResults.Ok(eventDetails);
    }
}
