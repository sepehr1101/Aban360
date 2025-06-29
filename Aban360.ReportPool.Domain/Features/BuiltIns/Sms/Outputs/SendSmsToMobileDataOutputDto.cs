namespace Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs
{
    public record SendSmsToMobileDataOutputDto
    {
        public string Text { get; set; }
        public string SendTime { get; set; }
        public string SendDate { get; set; }
        public string FinalDeliveryStateTitle { get; set; }
    }
}
