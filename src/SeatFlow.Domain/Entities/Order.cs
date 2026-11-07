using SeatFlow.Domain.Enums;

namespace SeatFlow.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public Guid CustomerId { get; set; }

    public Guid EventId { get; set; }
    
    public Event Event { get; set; } = null!;
    
    public OrderStatus Status { get; private set; } = OrderStatus.PendingPayment; 

    public decimal Total { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset ExpiresAt { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];

    public ICollection<Payment> Payments { get; set; } = [];    
}