using SeatFlow.Domain.Entities;
using SeatFlow.Domain.Enums;
using SeatFlow.Domain.Exceptions;

namespace SeatFlow.UnitTests.Domain.Entities;

public class OrderTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 13, 18, 0, 0, TimeSpan.Zero);
    
    [Fact]
    public void MarkAsPaid_WithinPaymentWindow_SetsStatusToPaid()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);

        // Act
        order.MarkAsPaid(Now.AddMinutes(5));

        // Assert
        Assert.Equal(OrderStatus.Paid, order.Status);
    }

    [Fact]
    public void Create_SetsStatusToPendingPayment()
    {
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);
        Assert.Equal(OrderStatus.PendingPayment, order.Status);
    }

    [Fact]
    public void Create_SetsExpiresAtTenMinutesAfterCreatedAt()
    {
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);
        Assert.Equal(Now.AddMinutes(10), order.ExpiresAt);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(10.5)]
    [InlineData(60)]
    public void MarkAsPaid_AtOrAfterExpiresAt_ThrowsDomainException(double minutesAfterCreation)
    {
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);
        Assert.Throws<DomainException>(() => order.MarkAsPaid(Now.AddMinutes(minutesAfterCreation)));
    }

    [Theory]
    [InlineData(10)]
    [InlineData(11)]
    public void Expire_AtOrAfterExpiresAt_SetsStatusToExpired(double minutesAfterCreation)
    {
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);
        order.Expire(Now.AddMinutes(minutesAfterCreation));
        Assert.Equal(OrderStatus.Expired, order.Status);
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(9)]
    public void Expire_BeforeExpiresAt_ThrowsDomainException(double minutesAfterCreation)
    {
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);
        Assert.Throws<DomainException>(() => order.Expire(Now.AddMinutes(minutesAfterCreation)));
    }

    [Fact]
    public void Expire_AfterPaymentWindow_DeactivatesAllTickets()
    {
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);
        order.Tickets.Add(new Ticket { Code = "A1" });
        order.Tickets.Add(new Ticket { Code = "B2" });
        order.Expire(Now.AddMinutes(10));
        Assert.All(order.Tickets, ticket => Assert.False(ticket.IsActive));
    }
    
    [Fact]
    public void Refund_WhenPaid_SetsStatusToRefunded()
    {
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);
        order.MarkAsPaid(Now.AddMinutes(5));
        order.Refund();
        Assert.Equal(OrderStatus.Refunded, order.Status);
    }

    [Fact]
    public void Refund_WhenPendingPayment_ThrowsDomainException()
    {
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Now);
        Assert.Throws<DomainException>(order.Refund);
    }
}