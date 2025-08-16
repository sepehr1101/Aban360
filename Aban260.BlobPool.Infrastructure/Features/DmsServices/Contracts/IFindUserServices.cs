using Aban360.Common.Extensions;
using System.Net.Http.Headers;
using System.Text;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IFindUserServices
    {
        Task Services(string folderPath, string property, string path);
    }
    internal sealed class FindUserServices : IFindUserServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetTokenServices _tokenServices;
        string _bearer = "Bearer";
        string _content = "text/plain";
        string baseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-SearchByMetadataAndPath/1.0";
        public FindUserServices(IHttpClientFactory httpClientFactory, IGetTokenServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

            _tokenServices = tokenServices;
            _tokenServices.NotNull(nameof(httpClientFactory));
        }

        public async Task Services(string folderPath, string property, string path)
        {
            var client = _httpClientFactory.CreateClient();
            string token = await _tokenServices.Service();
            string finalUrl = $"{baseUrl}?property={property}&path={path}";


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_bearer, token);
            var content = new StringContent(folderPath, Encoding.UTF8, _content);

            var response = await client.PostAsync(finalUrl, content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
        }
    }
}
