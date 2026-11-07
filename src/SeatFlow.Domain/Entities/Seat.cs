namespace SeatFlow.Domain.Entities;
public class Seat
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid SectionId { get; set; }
    public Section Section { get; set; } = null!;
    public required string Row { get; set; }
    public int Number { get; set; }
}