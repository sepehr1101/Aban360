using Aban360.Common.Extensions;
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
        string url = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateFolder/1.0";
        string _content = "application/json";
        public FolderCreateServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));
        }

        public async Task Services(string folderPath)
        {
            var client = _httpClientFactory.CreateClient("token");

            var jsonContent = new StringContent($"\"{folderPath}\"", Encoding.UTF8, _content);

            var response = await client.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
        }
    }
}
