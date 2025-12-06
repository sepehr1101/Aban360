namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterLifeCalculationOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = default!;
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }=default!;
        public int BranchTypeId { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; } = default!;
        public string? WaterInstallationDateJalali { get; set; }
        public string? LatestChangeDateJalali { get; set; }
        public int LifeInDay { get; set; }
        public string? LifeText { get; set; }
    }
}
