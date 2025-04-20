namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record IntervalCalculationResult2
    {
        public string Formula { get; set; } = default!;
        public double Consumption { get; set; }
        public int Duration { get; set; }
        public double Amount { get; set; }
        public string OfferingTitle { get; set; } = default!;
        public string LineItemTypeTitle { get; set; } = default!;
    }
}