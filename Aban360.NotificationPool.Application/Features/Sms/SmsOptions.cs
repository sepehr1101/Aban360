using Microsoft.Extensions.Configuration;

namespace Aban360.NotificationPool.Application.Features.Sms
{
    public class SmsOptions
    {
        public const string SectionName = "sms";
        public bool SendBatch { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;

        [ConfigurationKeyName("api-key")]
        public string ApiKey { get; set; } = string.Empty;
    }
}
