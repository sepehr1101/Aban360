namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CaluclationIntervalBatchTestInput
    {
        public string RegisterDate { get; set; } = default!;
        public int ZoneId { get; set; }
        public string FormReadingNumber { get; set; }= default!;
        public string ToReadingNumber { get; set; } = default!;
        public int Tolerance { get; set; }
    }
}
