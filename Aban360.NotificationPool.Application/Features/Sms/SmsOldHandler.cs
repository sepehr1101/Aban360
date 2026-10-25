using Aban360.Common.Extensions;
using Microsoft.Extensions.Options;
using Aban360.Common.Literals;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Aban360.NotificationPool.Domain.Features.Sms;
using static System.Net.Mime.MediaTypeNames;

namespace Aban360.NotificationPool.Application.Features.Sms
{
    public interface ISmsOldHandler
    {
        Task SendNew(string mobile, string text, Guid trackingId);
        Task Send(string mobile, string text, Guid trackingId = default, SmsMessageType messageType = SmsMessageType.Notification);
        Task Send(string mobile, string text, [Optional] Guid trackingId);
        Task Send(IEnumerable<SendSmsDataDto> data, SmsMessageType messageType = SmsMessageType.Notification);
    }

    internal sealed class SmsOldHandler : ISmsOldHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly SmsOptions _options;

        public SmsOldHandler(IHttpClientFactory httpClientFactory, IOptions<SmsOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));
            _httpClient = httpClientFactory.CreateClient(HttpClientNames.Sms);
            _httpClient.NotNull(nameof(_httpClient));
            _options = options.Value;
            _options.NotNull(nameof(_options));
        }
        public async Task SendNew(string mobile, string text, Guid trackingId)
        {           
            await Send(mobile, text, trackingId, SmsMessageType.Notification);
        }

        public async Task Send(string mobile, string text, Guid trackingId = default, SmsMessageType messageType = SmsMessageType.Notification)
        {
            using HttpRequestMessage request = new(HttpMethod.Post, "messages");
            request.Headers.Add("X-Api-Key", _options.ApiKey);
            request.Content = JsonContent.Create(new
            {
                senderLine = _options.Sender,
                messageTypeId = (byte)messageType,
                clientBatchId = Guid.NewGuid().ToString("N"),
                messages = new[]
                {
                    new
                    {
                        recipient = mobile,
                        text,
                        clientCorrelatedId = trackingId == Guid.Empty ? (string?)null : trackingId.ToString()
                    }
                }
            });

            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
        }
        public async Task Send(IEnumerable<SendSmsDataDto> data, SmsMessageType messageType = SmsMessageType.Notification)
        {
            using HttpRequestMessage request = new(HttpMethod.Post, "messages");
            request.Headers.Add("X-Api-Key", _options.ApiKey);
            request.Content = JsonContent.Create(new
            {
                senderLine = _options.Sender,
                messageTypeId = (byte)messageType,
                clientBatchId = Guid.NewGuid().ToString("N"),
                messages = data
            });

            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
        }

        public async Task Send(string mobile, string text, [Optional] Guid trackingId)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient("172");

            // Build query string manually
            string encodedMobile = Uri.EscapeDataString(mobile);
            string encodedText = Uri.EscapeDataString(text);
            string url = $"http://172.18.12.14:100/CRM/Sms/SendManual?key=dontYouKnowJesusLovesYouAll&mobile={encodedMobile}&text={encodedText}&trackingId={trackingId}";

            // Send GET request
            using HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
