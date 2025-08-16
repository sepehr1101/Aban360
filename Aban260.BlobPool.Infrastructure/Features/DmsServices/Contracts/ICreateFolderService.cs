using System.Net.Http.Headers;
using System.Text;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface ICreateFolderService
    {
        Task CreateFolderAsync(string folderPath);
    }

    internal sealed class CreateFolderService: ICreateFolderService
    {
        private readonly HttpClient _httpClient;
        private readonly IGetToken _tokenProvider;
        string url = $"'https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateFolder/1.0";
        public CreateFolderService(HttpClient httpClient, IGetToken tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        public async Task CreateFolderAsync(string folderPath)
        {
            string token = await _tokenProvider.Service();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent($"\"{folderPath}\"", Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
        }
    }
}
