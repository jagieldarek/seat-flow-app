namespace SeatFlow.Domain.Entities;

public class PriceCategory
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public decimal Price { get; set; }

    public Guid EventId { get; set; }

    public Event Event { get; set; } = null!;

    public Guid SectionId { get; set; }

    public Section Section { get; set; } = null!;
}