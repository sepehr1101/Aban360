using Aban360.Common.Extensions;
using System.Net.Http.Headers;
using System.Text;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IFolderCreateServices
    {
        Task Services(string folderPath);
    }
    internal sealed class FolderCreateServices: IFolderCreateServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenGetServices _tokenProvider;
        string url = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateFolder/1.0";
        string bearer ="Bearer";
        public FolderCreateServices(IHttpClientFactory httpClientFactory, ITokenGetServices tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

            _tokenProvider = tokenProvider;
            _tokenProvider.NotNull(nameof(tokenProvider));
        }

        public async Task Services(string folderPath)
        {
            var client = _httpClientFactory.CreateClient();
            string token = await _tokenProvider.Service();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(bearer, token);

            var jsonContent = new StringContent($"\"{folderPath}\"", Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
        }
    }
}
