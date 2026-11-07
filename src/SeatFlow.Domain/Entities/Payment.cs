using SeatFlow.Domain.Enums;

namespace SeatFlow.Domain.Entities;

public class Payment
{
	public Guid Id { get; private set; } = Guid.CreateVersion7();

	public Guid OrderId { get; set; }

	public Order Order { get; set; } = null!;

	public decimal Amount { get; set; }

	public PaymentStatus Status { get; set; }

	public required string IdempotencyKey { get; set; }

	public DateTimeOffset CreatedAt { get; set; }
}
