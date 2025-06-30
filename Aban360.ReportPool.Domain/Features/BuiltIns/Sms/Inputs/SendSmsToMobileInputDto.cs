namespace Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs
{
    public record SendSmsToMobileInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string Mobile { get; set; }
    }
}
