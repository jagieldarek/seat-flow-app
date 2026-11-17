using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    { 
        builder.Property(seat => seat.Id).ValueGeneratedNever();
        builder.Property(seat => seat.Row).HasMaxLength(3); // A, B, AA, a12 etc.
        builder.HasIndex(seat => new { seat.SectionId, seat.Row, seat.Number })
            .IsUnique();
        builder.HasOne(seat => seat.Section)
            .WithMany(section => section.Seats)
            .HasForeignKey(seat => seat.SectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}