using System.Net.Http.Headers;
using System.Text;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Implementations
{
    internal sealed class OpenKmCommandService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public async Task AddFile(string serverPath, string localFilePath)
        {
            var client = _httpClientFactory.CreateClient("token");

            using var form = new MultipartFormDataContent();

            var fileStream = File.OpenRead(localFilePath);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            form.Add(fileContent, "content", Path.GetFileName(localFilePath));

            form.Add(new StringContent(serverPath), "docPath");



            var response = await client.PostAsync(url, form);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
        }
        public async Task<string> AddFolder(string folderPath)
        {
            string url = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateFolder/1.0";
            string _content = "application/json";

            var client = _httpClientFactory.CreateClient("token");

            var jsonContent = new StringContent($"{folderPath}", Encoding.UTF8, _content);

            var response = await client.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
        public async Task AddOrUpdateMetadata(string body, string nodeId, string groupName)
        {
            string accept = "application/xml";
            string textPlain = "text/plain";
            string baseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-SetMetadata/1.0";

            var client = _httpClientFactory.CreateClient("token");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));

            var content = new StringContent(body, Encoding.UTF8, textPlain);
            string finalUrl = $"{baseUrl}?nodeId={nodeId}&grpName={groupName}";

            var response = await client.PutAsync(finalUrl, content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
        }
        public async Task EditFile(string nodeId, string groupName)
        {
            string accept = "application/xml";
            string cookie = "Cookie";
            string cookieDate = "cookiesession1=678ADA5C33A30F49D180AB6CBD34D5FC";
            string baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateMetadata/1.0";

            var client = _httpClientFactory.CreateClient("token");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            client.DefaultRequestHeaders.Add(cookie, cookieDate);

            string finalUrl = $"{baseUrl}?nodeId={nodeId}&grpName={groupName}";

            var response = await client.PostAsync(finalUrl, null);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}
