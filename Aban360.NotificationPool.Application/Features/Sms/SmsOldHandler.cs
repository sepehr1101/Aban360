using Aban360.Common.Extensions;

namespace Aban360.NotificationPool.Application.Features.Sms
{
    public interface ISmsOldHandler
    {
        Task Send(string mobile, string text);
    }

    internal sealed class SmsOldHandler : ISmsOldHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public SmsOldHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));
        }
        public async Task Send(string mobile, string text)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("172");

            // Build query string manually
            string encodedMobile = Uri.EscapeDataString(mobile);
            string encodedText = Uri.EscapeDataString(text);
            string url = $"http://172.18.12.14:100/CRM/Sms/SendManual?key=dontYouKnowJesusLovesYouAll&mobile={encodedMobile}&text={encodedText}";

            // Send GET request
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
