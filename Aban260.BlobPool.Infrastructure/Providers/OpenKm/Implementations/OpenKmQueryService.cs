using System.Net.Http.Headers;
using System.Text;
using Aban360.Common.Literals;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Implementations
{
    internal sealed class OpenKmQueryService
    {
        private readonly HttpClient _httpClient;
        public OpenKmQueryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientNames.Kaj);
        }

        public async Task<string> GetToken(string username, string password)
        {
            const string GrantTypeKey = "grant_type";
            const string ClientCredentialsValue = "client_credentials";
            const string TokenEndpoint = "token";
            const string BasicScheme = "Basic";
            const string FormUrlEncoded = "application/x-www-form-urlencoded";

            // Prepare form data
            var formData = new Dictionary<string, string>
            {
                { GrantTypeKey, ClientCredentialsValue }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Encode username:password for Basic Auth
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            request.Headers.Authorization = new AuthenticationHeaderValue(BasicScheme, credentials);

            // Explicit content type
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(FormUrlEncoded);

            // Send request
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> SearchDocuments(string folderPath, string property, string path)
        {
            string _content = "text/plain";
            string baseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-SearchByMetadataAndPath/1.0";
                       
            string finalUrl = $"{baseUrl}?property={property}&path={path}";

            var content = new StringContent(folderPath, Encoding.UTF8, _content);

            var response = await _httpClient.PostAsync(finalUrl, content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<string> GetFilesInDirectory(string fieldId)
        {
            string _accept = "application/json";
            string _content = "application/json";
            string baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetFilesList/1.0/";           

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));

            var values = new
            {
                fldId = fieldId
            };
            // var content=new StringContent(JsonSerializer.Serialize(values),Encoding.UTF8,_content);//?
            string encodedFldId = Uri.EscapeDataString(fieldId);
            string finalUrl = $"{baseUrl}?fldId={encodedFldId}";

            var response = await _httpClient.GetAsync(finalUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<string> GetFileBinary(string documentId)
        {
            string _baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetBinaryFile/1.0/";
            string finalUrl = $"{_baseUrl}?docId={documentId}";

            var response = await _httpClient.GetAsync(finalUrl);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<string> GetDownloadLink(string uuid, bool oneTimeUse)
        {
            string _BaseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetDownloadLinkFile/1.0";
            string _accept = "application/xml";
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            //client.DefaultRequestHeaders.Add("cache-control", "no-cache");

            string url = $"{_BaseUrl}?uuid={uuid}&oneTimeUse={oneTimeUse.ToString().ToLower()}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
