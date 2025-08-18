using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IDownloadLinkServices
    {
        Task<string> Services(string uuid, bool oneTimeUse = true);
    }
    internal sealed class DownloadLinkServices : IDownloadLinkServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string _BaseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetDownloadLinkFile/1.0";
        string _accept = "application/xml";
        public DownloadLinkServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

        }

        public async Task<string> Services(string uuid, bool oneTimeUse )
        {
            var client = _httpClientFactory.CreateClient("token");
           client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");

            string url = $"{_BaseUrl}?uuid={uuid}&oneTimeUse={oneTimeUse.ToString().ToLower()}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
