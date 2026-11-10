using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.NotificationPool.Domain.Features.Sms;

internal sealed class TicketSmsFake : ISmsOldHandler
{
    public List<(string Mobile, string Text)> Messages { get; } = new();
    public bool ThrowOnSend { get; set; }
    public Task Send(string mobile, string text, Guid trackingId = default, SmsMessageType messageType = SmsMessageType.Notification)
    {
        if (ThrowOnSend) throw new HttpRequestException("Simulated SMS outage");
        Messages.Add((mobile, text));
        return Task.CompletedTask;
    }
    public Task SendNew(string mobile, string text, Guid trackingId) => Send(mobile, text, trackingId, SmsMessageType.Notification);
    public Task Send(string mobile, string text, Guid trackingId) => Send(mobile, text, trackingId, SmsMessageType.Notification);
    public Task Send(IEnumerable<SendSmsDataDto> data, SmsMessageType messageType = SmsMessageType.Notification)
        => throw new NotSupportedException();
}
