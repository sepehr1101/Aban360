using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IFileInFolderCreateServices
    {
        Task Services(string serverPath, string localFilePath);
    }
    internal sealed class FileInFolderCreateServices : IFileInFolderCreateServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenGetServices _tokenServices;
        string url = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-AddFile/1.0/";
        string bearer ="Bearer";
        string docPath = "docPath";
        string content = "content";
        public FileInFolderCreateServices(IHttpClientFactory httpClientFactory, ITokenGetServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

            _tokenServices = tokenServices;
            _tokenServices.NotNull(nameof(tokenServices));
        }

        public async Task Services(string serverPath, string localFilePath)
        {
            var client = _httpClientFactory.CreateClient();
            string token = await _tokenServices.Service();

            client.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue(bearer, token);

            using var form = new MultipartFormDataContent();

            var fileBytes = await File.ReadAllBytesAsync(localFilePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            form.Add(fileContent, content, Path.GetFileName(localFilePath));
            form.Add(new StringContent(serverPath), docPath);

            var response = await client.PostAsync(url, form);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
        }
    }
}
