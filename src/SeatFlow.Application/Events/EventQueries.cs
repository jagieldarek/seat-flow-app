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
}
