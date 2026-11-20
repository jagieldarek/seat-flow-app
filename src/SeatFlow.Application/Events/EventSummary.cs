namespace SeatFlow.Application.Events;

public record EventSummary(Guid Id, string Title, DateTimeOffset StartsAt, string VenueName, string City);
