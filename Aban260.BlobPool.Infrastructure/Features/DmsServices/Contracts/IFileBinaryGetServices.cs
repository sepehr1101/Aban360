using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IFileBinaryGetServices
    {
        Task<string> Services(string documentId);
    }
    internal sealed class FileBinaryGetServices : IFileBinaryGetServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        string _baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetBinaryFile/1.0/";
        public FileBinaryGetServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));
        }

        public async Task<string> Services(string documentId)
        {
            var client = _httpClientFactory.CreateClient("token");
            string finalUrl = $"{_baseUrl}?docId={documentId}";


            var response = await client.GetAsync(finalUrl);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
