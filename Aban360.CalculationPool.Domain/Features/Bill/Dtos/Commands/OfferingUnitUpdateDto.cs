namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record OfferingUnitUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string Symbol { get; set; } = null!;
    }
}
