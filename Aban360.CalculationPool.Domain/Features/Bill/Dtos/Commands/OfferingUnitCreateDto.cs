namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record OfferingUnitCreateDto
    {
        public string Title { get; set; } = null!;
        public string Symbol { get; set; } = null!;
    }
}
