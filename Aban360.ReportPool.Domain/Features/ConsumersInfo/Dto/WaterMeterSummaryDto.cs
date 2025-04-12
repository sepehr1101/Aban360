namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record WaterMeterSummaryDto
    {
        public int Id { get; set; }
        public string? BodySerial { get; set; }
        public string InstallationDate { get; set; } = default!;
        public string MeterUseTypeTitle { get; set; } = default!;
        public string MeterDiameterTitle { get; set; } = default!;
    }
}
