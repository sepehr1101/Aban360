namespace Aban360.NotificationPool.Domain.Features.Sms
{
    public record SendSmsDataDto
    {
        public string Text { get; set; }
        public string Recipient { get; set; }
        public string? ClientCorrelatedId { get; set; }
        public SendSmsDataDto(string text, string recipient, string? clientCorrelatedId)
        {
            Text = text;
            Recipient = recipient;
            ClientCorrelatedId = clientCorrelatedId;
        }
        public SendSmsDataDto()
        {
        }
    }
}
