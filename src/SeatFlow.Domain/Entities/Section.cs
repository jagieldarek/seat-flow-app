namespace SeatFlow.Domain.Entities;
public class Section
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid VenueId { get; set; }
    public Venue Venue { get; set; } = null!;
    public required string Name { get; set; }
    public ICollection<Seat> Seats { get; set; } = []; // simplified -> new List<Seat>();
}