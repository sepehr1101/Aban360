using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IDownloadLinkServices
    {
        Task GetDownloadLinkAsync(string uuid, bool oneTimeUse = true);
    }
    internal sealed class DownloadLinkServices : IDownloadLinkServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetTokenServices _tokenServices;
        private const string _BaseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetDownloadLinkFile/1.0";
        string _bearer = "Bearer";
        string _accept = "application/xml";
        public DownloadLinkServices(IHttpClientFactory httpClientFactory, IGetTokenServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

            _tokenServices = tokenServices;
            _tokenServices.NotNull(nameof(tokenServices));
        }

        public async Task GetDownloadLinkAsync(string uuid, bool oneTimeUse = true)
        {
            var client = _httpClientFactory.CreateClient();
            string token = await _tokenServices.Service();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_bearer, token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");

            string url = $"{_BaseUrl}?uuid={uuid}&oneTimeUse={oneTimeUse.ToString().ToLower()}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
        }
    }
}
