using Microsoft.EntityFrameworkCore;
using SeatFlow.Application.Abstractions;
using SeatFlow.Domain.Enums;

namespace SeatFlow.Application.Events;

public class EventQueries(ISeatFlowDbContext dbContext)
{
    public async Task<IReadOnlyList<EventSummary>> GetOnSaleAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Events
            .Where(evt => evt.Status == EventStatus.OnSale)
            .OrderBy(evt => evt.StartsAt)
            .Select(evt => new EventSummary(evt.Id, evt.Title, evt.StartsAt, evt.Venue.Name, evt.Venue.City))
            .ToListAsync(cancellationToken);
    }

    public async Task<EventDetails?> GetOnSaleByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Events
            .Where(evt => evt.Id == id && evt.Status == EventStatus.OnSale)
            .Select(evt => new EventDetails(
                evt.Id,
                evt.Title,
                evt.StartsAt,
                evt.Venue.Name,
                evt.Venue.City,
                evt.Venue.Address,
                evt.PriceCategories
                    .OrderBy(priceCategory => priceCategory.Section.Name)
                    .Select(priceCategory => new SectionPrice(priceCategory.SectionId, priceCategory.Section.Name, priceCategory.Price))
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<SeatMap?> GetSeatMapAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var isOnSale = await dbContext.Events
            .AnyAsync(evt => evt.Id == eventId && evt.Status == EventStatus.OnSale, cancellationToken);

        if (!isOnSale)
        {
            return null;
        }

        var sections = await dbContext.PriceCategories
            .Where(priceCategory => priceCategory.EventId == eventId)
            .OrderBy(priceCategory => priceCategory.Section.Name)
            .Select(priceCategory => new SeatMapSection(
                priceCategory.SectionId,
                priceCategory.Section.Name,
                priceCategory.Price,
                priceCategory.Section.Seats
                    .OrderBy(seat => seat.Row)
                    .ThenBy(seat => seat.Number)
                    .Select(seat => new SeatMapSeat(
                        seat.Id,
                        seat.Row,
                        seat.Number,
                        dbContext.Tickets.Any(ticket => ticket.EventId == eventId && ticket.SeatId == seat.Id && ticket.IsActive)))
                    .ToList()))
            .ToListAsync(cancellationToken);

        return new SeatMap(eventId, sections);
    }
}
