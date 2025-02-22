namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public record WaterMeterSummaryDto
    {
        public string? BodySerial { get; set; }
        public DateTime? InstallationDate { get; set; }
        public string MeterUseTypeTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
    }
}
