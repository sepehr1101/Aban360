namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record LatestWaterMeterInfoDto
    {
        public string WaterMeterNumber { get; set; }//Todo : rename
        public string  LatestDate { get; set; }
    }
}
