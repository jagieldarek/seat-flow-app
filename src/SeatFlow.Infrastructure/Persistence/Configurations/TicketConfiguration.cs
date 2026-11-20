using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.Property(ticket => ticket.Id).ValueGeneratedNever();
        builder.Property(ticket => ticket.Price).HasPrecision(10, 2);
        builder.Property(ticket => ticket.Code).HasMaxLength(50);
        builder.HasIndex(ticket => ticket.Code).IsUnique();
        builder.HasIndex(ticket => new { ticket.EventId, ticket.SeatId })
            .IsUnique()
            .HasFilter("is_active"); // raw SQL: column name after snake_case convention
        builder.HasOne(ticket => ticket.Order)
            .WithMany(order => order.Tickets)
            .HasForeignKey(ticket => ticket.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ticket => ticket.Seat)
            .WithMany()
            .HasForeignKey(ticket => ticket.SeatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(ticket => ticket.EventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
