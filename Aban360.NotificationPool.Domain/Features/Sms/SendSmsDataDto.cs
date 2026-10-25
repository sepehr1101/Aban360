namespace Aban360.NotificationPool.Domain.Features.Sms
{
    public record SendSmsDataDto
    {
        public string Text { get; set; }
        public string Recipient { get; set; }
        public string? ClientCorrelatedId { get; set; }
        public string? BillId { get; set; }
        public string? PayId { get; set; }
        public SendSmsDataDto(string text, string recipient, string? billId, string? payId, Guid trackingId = default)
        {
            Text = text;
            Recipient = recipient;
            ClientCorrelatedId = trackingId == Guid.Empty ? (string?)null : trackingId.ToString();
            BillId = billId;
            PayId = payId;
        }
        public SendSmsDataDto()
        {
        }
    }
}
