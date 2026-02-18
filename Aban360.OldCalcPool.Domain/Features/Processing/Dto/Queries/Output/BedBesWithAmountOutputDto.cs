namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BedBesWithAmountOutputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public long Payable { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public float ConsumptionAverage { get; set; }
        public int PreviousMeterNumber { get; set; }
        public int CurrentMeterNumber { get; set; }
        public int Consumption { get; set; }
        public string RegisterDateJalali { get; set; }
    }
}