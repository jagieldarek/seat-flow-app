namespace SeatFlow.Domain.Entities;

public class Ticket
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public Guid OrderId { get; set; }

    public Order Order { get; set; } = null!;

    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Seat Seat { get; set; } = null!;

    public decimal Price { get; set; }

    public required string Code { get; set; }

    public DateTimeOffset? CheckedInAt { get; set; }

    public bool IsActive { get; set; } = true;
}