using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.Common.Literals;
using System.Net.Http.Headers;
using System.Text;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Implementations
{
    //public interface IOpenKmCommandService
    //{
    //    Task<string> AddFolder(string folderPath);
    //}

    //internal sealed class OpenKmCommandService: IOpenKmCommandService
    //{
    //    private readonly HttpClient _httpClient;
    //    private readonly IOpenKmQueryService _openKmQuery;
    //    public OpenKmCommandService(
    //        IHttpClientFactory httpClientFactory,
    //        IOpenKmQueryService openKmQuery)
    //    {
    //        _httpClient = httpClientFactory.CreateClient(HttpClientNames.Kaj);
    //        _openKmQuery = openKmQuery;
    //    }

    //    public async Task AddFile(string serverPath, string localFilePath)
    //    {
    //        using var form = new MultipartFormDataContent();

    //        var fileStream = File.OpenRead(localFilePath);
    //        var fileContent = new StreamContent(fileStream);
    //        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

    //        form.Add(fileContent, "content", Path.GetFileName(localFilePath));

    //        form.Add(new StringContent(serverPath), "docPath");



    //        var response = await _httpClient.PostAsync("url", form);
    //        response.EnsureSuccessStatusCode();

    //        string result = await response.Content.ReadAsStringAsync();
    //    }
    //    public async Task<string> AddFolder(string folderPath)
    //    {
    //        string url = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateFolder/1.0";
    //        string _content = "application/json";

    //        var jsonContent = new StringContent($"{folderPath}", Encoding.UTF8, _content);

    //        // Ensure Bearer token
    //        AuthenticationHeaderValue authHeader = await _openKmQuery.GetAuthenticationHeaderAsync();
    //        _httpClient.DefaultRequestHeaders.Authorization = authHeader;

    //        var response = await _httpClient.PostAsync(url, jsonContent);
    //        response.EnsureSuccessStatusCode();
    //        string result = await response.Content.ReadAsStringAsync();

    //        return result;
    //    }
    //    public async Task AddOrUpdateMetadata(string body, string nodeId, string groupName)
    //    {
    //        string accept = "application/xml";
    //        string textPlain = "text/plain";
    //        string baseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-SetMetadata/1.0";

    //        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));

    //        var content = new StringContent(body, Encoding.UTF8, textPlain);
    //        string finalUrl = $"{baseUrl}?nodeId={nodeId}&grpName={groupName}";

    //        var response = await _httpClient.PutAsync(finalUrl, content);
    //        response.EnsureSuccessStatusCode();
    //        string result = await response.Content.ReadAsStringAsync();
    //    }
    //    public async Task EditFile(string nodeId, string groupName)
    //    {
    //        string accept = "application/xml";
    //        string cookie = "Cookie";
    //        string cookieDate = "cookiesession1=678ADA5C33A30F49D180AB6CBD34D5FC";
    //        string baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateMetadata/1.0";

    //        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
    //        //client.DefaultRequestHeaders.Add(cookie, cookieDate);

    //        string finalUrl = $"{baseUrl}?nodeId={nodeId}&grpName={groupName}";

    //        var response = await _httpClient.PostAsync(finalUrl, null);
    //        response.EnsureSuccessStatusCode();
    //        var result = await response.Content.ReadAsStringAsync();
    //    }
    //}
}
