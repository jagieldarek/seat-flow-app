namespace SeatFlow.Application.Events;

public record SeatMap(Guid EventId, IReadOnlyList<SeatMapSection> Sections);

public record SeatMapSection(Guid SectionId, string Name, decimal Price, IReadOnlyList<SeatMapSeat> Seats);

public record SeatMapSeat(Guid SeatId, string Row, int Number, bool IsTaken);
