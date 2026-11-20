using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedNever();
        builder.Property(e => e.Title)
            .HasMaxLength(maxLength: 200);
        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(maxLength: 30);
        
        builder.HasOne(e => e.Venue)
            .WithMany()
            .HasForeignKey(evt => evt.VenueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}