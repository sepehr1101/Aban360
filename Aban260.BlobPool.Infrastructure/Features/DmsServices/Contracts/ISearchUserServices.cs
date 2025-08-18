using Aban360.Common.Extensions;
using System.Text;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface ISearchUserServices
    {
        Task<string> Services(string folderPath, string property, string path);
    }
    internal sealed class SearchUserServices : ISearchUserServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        string _content = "text/plain";
        string baseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-SearchByMetadataAndPath/1.0";
        public SearchUserServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));
        }

        public async Task<string> Services(string folderPath, string property, string path)
        {
            var client = _httpClientFactory.CreateClient("token");
            string finalUrl = $"{baseUrl}?property={property}&path={path}";

            var content = new StringContent(folderPath, Encoding.UTF8, _content);

            var response = await client.PostAsync(finalUrl, content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
