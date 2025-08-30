using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aban260.BlobPool.Infrastructure.Features.DmsServices.Base;
using System.Text.Json;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Implementations
{
    internal sealed class OpenKmQueryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpClientConfigureServices _configureServices;
        string _token;
        DateTime _expireTime;

        public async Task<string> GetToken()
        {
            string Url = $"token";
            string Accept = $"token";
            string _basic = "Basic";
            string _basicToken = "UEtxbkJ1enVNRXM0aEFySV9CUGZZaWhKS1lNYTpZUlFFZjcyR29zc0oyZ0dtMmhFMU5PTVZhVDhh";

            var body = new Dictionary<string, string>()
            {
                {"grant_type","client_credentials" }
            };

            var client = _httpClientFactory.CreateClient();
            HttpClientParameters clientParams = new(_basicToken)
            {
                AuthenticationType = _basic,
                BodyData = body,
            };
            _configureServices.Services(client, clientParams);



            var content = new FormUrlEncodedContent(body);
            var respone = await client.PostAsync(Url, content);
            respone.EnsureSuccessStatusCode();

            var jsonResponse = await respone.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<GetTokenDto>(jsonResponse);
            _token = tokenResponse.access_token;
            _expireTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpireTime);

            return tokenResponse.access_token;
        }
        public async Task<string> SearchDocuments(string folderPath, string property, string path)
        {
            string _content = "text/plain";
            string baseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-SearchByMetadataAndPath/1.0";

            var client = _httpClientFactory.CreateClient("token");
            string finalUrl = $"{baseUrl}?property={property}&path={path}";

            var content = new StringContent(folderPath, Encoding.UTF8, _content);

            var response = await client.PostAsync(finalUrl, content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<string> GetFilesInDirectory(string fieldId)
        {
            string _accept = "application/json";
            string _content = "application/json";
            string baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetFilesList/1.0/";
            var client = _httpClientFactory.CreateClient("token");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));

            var values = new
            {
                fldId = fieldId
            };
            // var content=new StringContent(JsonSerializer.Serialize(values),Encoding.UTF8,_content);//?
            string encodedFldId = Uri.EscapeDataString(fieldId);
            string finalUrl = $"{baseUrl}?fldId={encodedFldId}";

            var response = await client.GetAsync(finalUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<string> GetFileBinary(string documentId)
        {
            string _baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetBinaryFile/1.0/";

            var client = _httpClientFactory.CreateClient("token");
            string finalUrl = $"{_baseUrl}?docId={documentId}";

            var response = await client.GetAsync(finalUrl);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<string> GetDownloadLink(string uuid, bool oneTimeUse)
        {
            string _BaseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetDownloadLinkFile/1.0";
            string _accept = "application/xml";

            var client = _httpClientFactory.CreateClient("token");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");

            string url = $"{_BaseUrl}?uuid={uuid}&oneTimeUse={oneTimeUse.ToString().ToLower()}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
