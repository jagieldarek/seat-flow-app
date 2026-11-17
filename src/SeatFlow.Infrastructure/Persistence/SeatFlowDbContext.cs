using Microsoft.EntityFrameworkCore;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence;

public class SeatFlowDbContext(DbContextOptions<SeatFlowDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PriceCategory> PriceCategories { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SeatFlowDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
};