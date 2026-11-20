using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(payment => payment.Id).ValueGeneratedNever();
        builder.Property(payment => payment.Amount).HasPrecision(10, 2);
        builder.Property(payment => payment.Status)
            .HasConversion<string>()
            .HasMaxLength(30);
        builder.Property(payment => payment.IdempotencyKey).HasMaxLength(100);
        builder.HasIndex(payment => payment.IdempotencyKey).IsUnique();
        builder.HasOne(payment => payment.Order)
            .WithMany(order => order.Payments)
            .HasForeignKey(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
