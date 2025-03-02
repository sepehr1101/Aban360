namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record OfferingGroupUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
