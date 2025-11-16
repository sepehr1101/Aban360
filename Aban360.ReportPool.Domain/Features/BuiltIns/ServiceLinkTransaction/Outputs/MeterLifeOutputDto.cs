namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterLifeOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int BranchTypeId { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public string WaterInstallationDateJalali { get; set; }
        public string? LatestChangeDataJalali { get; set; }
        public int LifeInDay { get; set; }
        public string LifeText { get; set; }
    }
}
