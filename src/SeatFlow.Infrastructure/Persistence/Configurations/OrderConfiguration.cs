using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(order => order.Id).ValueGeneratedNever();
        builder.Property(order => order.Status)
            .HasConversion<string>()
            .HasMaxLength(30);
        builder.Property(order => order.Total).HasPrecision(10, 2);

        builder.HasOne(order => order.Event)
            .WithMany()
            .HasForeignKey(order => order.EventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}