namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record WaterMeterReplacementsInputDto
    {
        public bool IsChangeDate { get; set; }
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }
    }
}
