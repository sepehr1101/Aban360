namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public record WaterMeterSummaryDto
    {
        public string? BodySerial { get; set; }
        public string InstallationDate { get; set; } = default!;
        public string MeterUseTypeTitle { get; set; } = default!;
        public string MeterDiameterTitle { get; set; } = default!;
    }
}
