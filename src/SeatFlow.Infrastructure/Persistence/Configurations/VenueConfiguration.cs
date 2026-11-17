using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.Property(venue => venue.Name).HasMaxLength(200);
        builder.Property(venue => venue.Address).HasMaxLength(200);
        builder.Property(venue => venue.City).HasMaxLength(100);
        builder.Property(venue => venue.Id).ValueGeneratedNever();
    }
}