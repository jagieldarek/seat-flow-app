using Microsoft.EntityFrameworkCore;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Application.Abstractions;

public interface ISeatFlowDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<Event> Events { get; }
    DbSet<Payment> Payments { get; }
    DbSet<PriceCategory> PriceCategories { get; }
    DbSet<Seat> Seats { get; }
    DbSet<Section> Sections { get; }
    DbSet<Ticket> Tickets { get; }
    DbSet<Venue> Venues { get; }
}
