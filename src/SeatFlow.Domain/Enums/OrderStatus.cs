namespace SeatFlow.Domain.Enums;

public enum OrderStatus
{
    PendingPayment = 0,
    Paid = 1,
    Expired = 2,
    Cancelled = 3,
    Refunded = 4
}