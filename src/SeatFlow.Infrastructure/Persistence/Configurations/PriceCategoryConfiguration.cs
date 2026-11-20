using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatFlow.Domain.Entities;

namespace SeatFlow.Infrastructure.Persistence.Configurations;

public class PriceCategoryConfiguration : IEntityTypeConfiguration<PriceCategory>
{
    public void Configure(EntityTypeBuilder<PriceCategory> builder)
    {
        builder.Property(priceCategory => priceCategory.Id).ValueGeneratedNever();
        builder.Property(priceCategory => priceCategory.Price).HasPrecision(10, 2);
        builder.HasIndex(priceCategory => new { priceCategory.EventId, priceCategory.SectionId }).IsUnique();
        builder.HasOne(priceCategory => priceCategory.Event)
            .WithMany(events => events.PriceCategories)
            .HasForeignKey(priceCategory => priceCategory.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(priceCategory => priceCategory.Section)
            .WithMany()
            .HasForeignKey(priceCategory => priceCategory.SectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}