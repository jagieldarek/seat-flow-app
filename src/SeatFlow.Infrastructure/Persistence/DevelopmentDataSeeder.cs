using Microsoft.EntityFrameworkCore;
using SeatFlow.Domain.Entities;
using SeatFlow.Domain.Enums;

namespace SeatFlow.Infrastructure.Persistence;

public static class DevelopmentDataSeeder
{
    public static void Seed(DbContext context)
    {
        if (context.Set<Venue>().Any())
        {
            return;
        }

        context.AddRange(CreateEvents());
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        if (await context.Set<Venue>().AnyAsync(cancellationToken))
        {
            return;
        }

        context.AddRange(CreateEvents());
        await context.SaveChangesAsync(cancellationToken);
    }

    private static Event[] CreateEvents()
    {
        var venue = new Venue
        {
            Name = "SeatFlow Arena",
            City = "Kraków",
            Address = "ul. Przykładowa 1"
        };
        var sectorA = CreateSection(venue, "Sector A");
        var sectorB = CreateSection(venue, "Sector B");
        var balcony = CreateSection(venue, "Balcony");
        var today = new DateTimeOffset(DateTime.UtcNow.Date, TimeSpan.Zero);

        var jazzNight = new Event
        {
            Title = "Jazz Night",
            StartsAt = today.AddDays(30).AddHours(18),
            Status = EventStatus.OnSale,
            Venue = venue,
            PriceCategories =
            [
                new PriceCategory { Section = sectorA, Price = 250m },
                new PriceCategory { Section = sectorB, Price = 180m },
                new PriceCategory { Section = balcony, Price = 120m }
            ]
        };
        var rockFestival = new Event
        {
            Title = "Rock Festival",
            StartsAt = today.AddDays(60).AddHours(18),
            Status = EventStatus.Draft,
            Venue = venue
        };

        return [jazzNight, rockFestival];
    }

    private static Section CreateSection(Venue venue, string name)
    {
        var section = new Section { Name = name };
        venue.Sections.Add(section);

        for (var row = 'A'; row <= 'J'; row++)
        {
            for (var number = 1; number <= 15; number++)
            {
                section.Seats.Add(new Seat { Row = row.ToString(), Number = number });
            }
        }

        return section;
    }
}
