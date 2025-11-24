namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record LatestBedBesConsumptionInfo
    {
        public int CustomerNumber { get; set; }
        public string LastMeterDateJalali { get; set; }
        public int? LastMeterNumber { get; set; }
        public float? LastMonthlyConsumption { get; set; }
        public float? LastConsumption { get; set; }
        public int? LastCounterStateCode { get; set; }
        public double? LastSumItems { get; set; }
    }
}
