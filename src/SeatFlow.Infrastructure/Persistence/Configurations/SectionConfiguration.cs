using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.Property(section => section.Id).ValueGeneratedNever();
        builder.Property(section => section.Name).HasMaxLength(40);
        builder.HasIndex(section => new { section.VenueId, section.Name }).IsUnique();
        builder.HasOne(section => section.Venue)
            .WithMany(venue => venue.Sections)
            .HasForeignKey(section => section.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}