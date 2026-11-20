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
    }
}