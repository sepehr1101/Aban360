namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record InstallGtRequestDataOutputDto
    {
        public string WaterRequestDateJalali { get; set; }
        public string WaterInstallDateJalali { get; set; }
        public string CustomerNumber { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string FullName { get; set; }
        public string ReadingNumber { get; set; }
        public string UsageTitle { get; set; }
        public int Distance { get; set; }
    }
}