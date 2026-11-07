namespace SeatFlow.Domain.Entities;

public class Venue
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public required string Name { get; set; }

    public required string City { get; set; }

    public required string Address { get; set; }

    public ICollection<Section> Sections { get; set; } = [];
}