namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record IntervalCalculationResult3
    {
        public string FromDate { get; set; } = default!;
        public string ToDate { get; set; } = default!;
        public ICollection<IntervalCalculationResult2> CalculationInfo { get; set; } = default!;
    }
}