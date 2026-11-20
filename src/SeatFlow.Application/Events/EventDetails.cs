namespace SeatFlow.Application.Events;

public record EventDetails(
    Guid Id,
    string Title,
    DateTimeOffset StartsAt,
    string VenueName,
    string City,
    string Address,
    IReadOnlyList<SectionPrice> Prices);

public record SectionPrice(Guid SectionId, string SectionName, decimal Price);
