namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record CustomerGeneralBillInfoDto
    {
        public string CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }
        public int LatestMeterNumber { get; set; }
        public string LatestMeterReading { get; set; }
        public string UsageStatusTitle { get; set; }

    }
}
