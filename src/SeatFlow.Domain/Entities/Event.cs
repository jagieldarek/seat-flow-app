using SeatFlow.Domain.Enums;

namespace SeatFlow.Domain.Entities;

public class Event
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public required string Title { get; set; }

    public DateTimeOffset StartsAt { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Draft;

    public Guid VenueId { get; set; }

    public Venue Venue { get; set; } = null!;

    public Guid OrganizerId { get; set; }

    public ICollection<PriceCategory> PriceCategories { get; set; } = [];
}