using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IGetFileBainaryServices
    {
        Task Services(string documentId);
    }
    internal sealed class GetFileBainaryServices : IGetFileBainaryServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetTokenServices _tokenServices;
        string _bearer = "Bearer";
        string _baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetBinaryFile/1.0/";
        public GetFileBainaryServices(IHttpClientFactory httpClientFactory, IGetTokenServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

            _tokenServices = tokenServices;
            _tokenServices.NotNull(nameof(tokenServices));
        }

        public async Task Services(string documentId)
        {
            string token = await _tokenServices.Service();
            var client = _httpClientFactory.CreateClient();
            string finalUrl = $"{_baseUrl}?docId={documentId}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_bearer, token);

            var response = await client.GetAsync(finalUrl);
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync();
        }
    }
}
