namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record OfferingUnitGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string Symbol { get; set; } = null!;
    }
}
