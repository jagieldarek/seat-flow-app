using SeatFlow.Domain.Enums;
using SeatFlow.Domain.Exceptions;

namespace SeatFlow.Domain.Entities;

public class Order
{
    public static readonly TimeSpan PaymentWindow = TimeSpan.FromMinutes(10);

    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public Guid CustomerId { get; private set; }

    public Guid EventId { get; private set; }

    public Event Event { get; set; } = null!;

    public OrderStatus Status { get; private set; } = OrderStatus.PendingPayment;

    public decimal Total { get; set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset ExpiresAt { get; private set; }

    public ICollection<Ticket> Tickets { get; set; } = [];

    public ICollection<Payment> Payments { get; set; } = [];

    private Order() { }

    public static Order Create(Guid customerId, Guid eventId, DateTimeOffset now)
    {
        return new Order
        {
            CustomerId = customerId,
            EventId = eventId,
            CreatedAt = now,
            ExpiresAt = now + PaymentWindow
        };
    }

    public void MarkAsPaid(DateTimeOffset now)
    {
        EnsureStatus(OrderStatus.PendingPayment, "mark as paid");
        if (now >= ExpiresAt)
        {
            throw new DomainException("Cannot mark order as paid after it has expired.");
        }

        Status = OrderStatus.Paid;
    }

    public void Cancel()
    {
        EnsureStatus(OrderStatus.PendingPayment, "cancel");
        Status = OrderStatus.Cancelled;
        DeactivateTickets();
    }

    public void Expire(DateTimeOffset now)
    {
        EnsureStatus(OrderStatus.PendingPayment, "expire");
        if (now < ExpiresAt)
        {
            throw new DomainException("Cannot expire order before its payment window has passed.");
        }

        Status = OrderStatus.Expired;
        DeactivateTickets();
    }

    public void Refund()
    {
        EnsureStatus(OrderStatus.Paid, "refund");
        Status = OrderStatus.Refunded;
        DeactivateTickets();
    }

    private void EnsureStatus(OrderStatus expectedStatus, string action)
    {
        if (Status != expectedStatus)
        {
            throw new DomainException($"Cannot {action} order in status {Status}.");
        }
    }

    private void DeactivateTickets()
    {
        foreach (var ticket in Tickets)
        {
            ticket.IsActive = false;
        }
    }
}
