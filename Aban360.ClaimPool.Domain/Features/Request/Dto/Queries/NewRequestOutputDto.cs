namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record NewRequestOutputDto
    {
        public int TrackNumber { get; set; }
        public bool HasSendSms { get; set; }
        public string? SmsMessage { get; set; }
        public NewRequestOutputDto(int trackNumber,bool hasSendSms,string? smsMessage)
        {
            TrackNumber = trackNumber;
            HasSendSms = hasSendSms;
            this.SmsMessage = smsMessage;
        }
    }
}