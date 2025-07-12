namespace Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs
{
    public record SendSmsToMobileHeaderOutputDto
    {
        public string Receiver { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
    }
}
