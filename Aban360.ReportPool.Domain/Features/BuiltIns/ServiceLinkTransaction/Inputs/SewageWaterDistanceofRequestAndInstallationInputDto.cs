namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record SewageWaterDistanceofRequestAndInstallationInputDto
    {
        public bool IsWater { get; set; }
        public bool IsInstallation { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public ICollection<int> ZoneIds { get; set; }
    }
}
