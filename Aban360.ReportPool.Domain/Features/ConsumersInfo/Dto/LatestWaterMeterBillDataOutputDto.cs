namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record LatestWaterMeterBillDataOutputDto
    {
        public float ConsumptionAverage { get; set; }
        public string MeterStatusTitle { get; set; }
        public string LatestMeterNumber { get; set; }
        public string LatestMeterReading { get; set; }

    }
}
