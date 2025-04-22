namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CaluclationIntervalDiscrepancy
    {
        public int CustomerNumber { get; set; }
        public string BillId { get; set; } = default!;
        public long Amount { get; set; }
        public int FromWaterMeterNumber { get; set; }
        public int ToWaterMeterNumber { get; set; }
        public string FromReadingDate { get; set; }= default!;
    }
    public record CaluclationIntervalDiscrepancytWrapper
    {
        public long PreviousSystemSum { get; set; }
        public long CurrentSystemSum { get; set; }
        public long DifferenceSum { get; set; }
        public ICollection<CaluclationIntervalDiscrepancy>? DiscrepancyDetails { get; set; }
    }
        
}
