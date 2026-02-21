using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using HttpClientToCurl;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Implementations
{
    internal sealed class OpenKmQueryService : IOpenKmQueryService
    {
        const string applicationJson = @"application/json";
        private readonly HttpClient _httpClient;
        private readonly OpenKmOptions _options;
        private readonly IMemoryCache _cache;

        private const string TokenCacheKey = "OpenKm_AccessToken";
        private const string GroupNameFolder = "okg%3Amoshtarakin_folder";
        private const string GroupNameFile = "okg%3Amoshtarakin_file";

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public OpenKmQueryService(
            IHttpClientFactory httpClientFactory,
            IOptions<OpenKmOptions> options,
            IMemoryCache cache)
        {
            httpClientFactory.NotNull(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient(HttpClientNames.Kaj);
            _httpClient.NotNull(nameof(_httpClient));

            _options = options.Value;
            _options.NotNull(nameof(_options));

            _cache = cache;
            _cache.NotNull(nameof(_cache));
        }

        private async Task<TokenResponse> GetToken()
        {
            if (_cache.TryGetValue(TokenCacheKey, out TokenResponse cachedToken))
            {
                return cachedToken;
            }

            var token = await RequestNewToken();

            // set cache with expiration slightly earlier than real expiry
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.ExpiresIn - 30)
            };

            _cache.Set(TokenCacheKey, token, cacheOptions);

            return token;
        }
        private async Task<TokenResponse> RequestNewToken()
        {
            const string GrantTypeKey = "grant_type";
            const string ClientCredentialsValue = "client_credentials";
            const string BasicScheme = "Basic";
            const string FormUrlEncoded = "application/x-www-form-urlencoded";

            // Prepare form data
            var formData = new Dictionary<string, string>
            {
                { GrantTypeKey, ClientCredentialsValue }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Encode username:password for Basic Auth
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.Username}:{_options.Password}"));
            request.Headers.Authorization = new AuthenticationHeaderValue(BasicScheme, credentials);

            // Explicit content type
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(FormUrlEncoded);

            // Send request
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TokenResponse>(_jsonOptions);
        }
        public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync()//todo: to Private
        {
            TokenResponse token = await GetToken();
            return new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
        }

        public async Task<FileListResponse> GetFilesDiscount(string id)
        {
            string fldId = $"{_options.BaseDiscountPath}{id}";
            return await GetChildren(fldId, false);
        }
        public async Task<FileListResponse> GetFilesByBillId(string billId)
        {
            string fldId = $"{_options.BaseDirectoryPath}{billId}";
            return await GetChildren(fldId, false);
        }
        private async Task<FileListResponse> GetChildren(string fldId, bool isDiscount)
        {
            var authHeader = await GetAuthenticationHeaderAsync();

            // Only append the relative path/query; BaseAddress comes from DI
            var requestUrl = $"{_options.GetFilesListEndpoint}?fldId={Uri.EscapeDataString(fldId)}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = authHeader;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidBillIdException(isDiscount ? ExceptionLiterals.InvalidDiscountFileName : ExceptionLiterals.BillIdNotFound);
            }
            return await response.Content.ReadFromJsonAsync<FileListResponse>(_jsonOptions);
        }

        public async Task<FileListResponse> GetFilesInDirectory(string fieldId)
        {
            var authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build endpoint from config
            string requestUrl = $"{_options.GetFilesListEndpoint}?{Uri.EscapeDataString(fieldId)}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FileListResponse>(_jsonOptions);
        }
        public async Task<byte[]> GetFileBinary(string documentId)
        {
            // Ensure Bearer token
            AuthenticationHeaderValue authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build URL from configuration
            string requestUrl = $"{_options.GetBinaryFileEndpoint}?docId={Uri.EscapeDataString(documentId)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            // Read as byte array instead of string
            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<byte[]> GetImageThumbnail(string documentId)
        {
            // Ensure Bearer token
            AuthenticationHeaderValue authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build URL from configuration
            string requestUrl = $"{_options.GetThumbnailEndpoint}?docId={Uri.EscapeDataString(documentId)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            // Read as byte array instead of string
            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<string> GetDownloadLink(string documentId, bool oneTimeUse)
        {
            var authHeader = await GetAuthenticationHeaderAsync();

            // Only append the relative path/query; BaseAddress comes from DI
            var requestUrl = $"{_options.GetDownloadLinkEndpoint}?uuid={documentId}&oneTimeUse={oneTimeUse.ToString().ToLower()}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = authHeader;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<MetaDataProperties> GetMetaDataProperties(string documentId)
        {
            var authHeader = await GetAuthenticationHeaderAsync();

            // Only append the relative path/query; BaseAddress comes from DI
            var requestUrl = $"{_options.GeMetadataEndpoint}?nodeId={documentId}&grpName={GroupNameFile}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = authHeader;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            //   MetaDataProperties result = await response.Content.ReadFromJsonAsync<MetaDataProperties>();

            string jsonResult = await response.Content.ReadAsStringAsync();
            MetaDataProperties metaDataProperties;

            if (jsonResult.Contains("[") && jsonResult.Contains("]"))
            {
                metaDataProperties = JsonSerializer.Deserialize<MetaDataProperties>(jsonResult);
            }
            else
            {
                PropertyGroupItem singleMetaData = JsonSerializer.Deserialize<MetaDataProperty>(jsonResult).RawMetaDatas;

                metaDataProperties = new MetaDataProperties()
                {
                    RawMetaDatas = new List<PropertyGroupItem>
                    {
                        singleMetaData
                    }
                };
            }

            return metaDataProperties;
        }

        public async Task<SearchDocumentsResponse> SearchDocuments(string folderPath, string property, string path)
        {
            // Ensure Bearer token
            var authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build URL from configuration
            string requestUrl = $"{_options.SearchByMetadataEndpoint}?property={Uri.EscapeDataString(property)}&path={Uri.EscapeDataString(path)}";

            // Post folderPath as JSON content
            var content = new StringContent(folderPath, Encoding.UTF8, applicationJson);

            var response = await _httpClient.PostAsync(requestUrl, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SearchDocumentsResponse>(_jsonOptions);
        }
        private void LogRequestToConsole(HttpRequestMessage requestMessage)
        {
            /*string curl = */_httpClient.GenerateCurlInFile(
            requestMessage,
            config =>
            {
                config.TurnOn = true;
                config.NeedAddDefaultHeaders = true;
            });
            int x = 2;
        }

        //Commands
        public async Task<bool> CheckFolderExists(string fldId)
        {            
            AuthenticationHeaderValue authHeader = await GetAuthenticationHeaderAsync();
            string requestUrl = $"{_options.PathExistsEndpoint}/{fldId}";
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>(_jsonOptions);
        }
        public async Task<AddFileDto> AddFile(string path, StreamContent content, string fileName)
        {           
            string docPath= nameof(docPath);

            string fullPath = $"{_options.BaseDirectoryPath}{path}";
            var requestUrl = $"{_options.AddFileEndpoint}";
            var authHeader = await GetAuthenticationHeaderAsync();            

            using var form = new MultipartFormDataContent();
            form.Add(content, nameof(content), fileName);
            form.Add(new StringContent(fullPath), docPath);

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Authorization = authHeader;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));           
            request.Content = form;
          
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            AddFileDto result = await response.Content.ReadFromJsonAsync<AddFileDto>(_jsonOptions);
            return result;
        }
        public async Task<string> AddFolderByBillId(string billId)
        {
            string fldId = $"{_options.BasePath}{billId}";
            return await AddFolder(fldId);
        }
        private async Task<string> AddFolder(string fullPath)
        {
            string url = $"{_options.AddFolderEndpoint}";
            string _content = "application/json";

            var jsonContent = new StringContent($"{fullPath}", Encoding.UTF8, _content);

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = jsonContent
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var authHeader = await GetAuthenticationHeaderAsync();
            request.Headers.Authorization = authHeader;
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            string? uuid = doc.RootElement.GetProperty("uuid").GetString();
            if (uuid == null)
            {
                return "invalid uuid";
            }
            await EditFile(uuid);

            return uuid;
        }
        public async Task AddOrUpdateMetadata(string body, string nodeId)
        {
            string accept = "application/xml";

            string requestUrl = $"{_options.AddMetadataEndpoint}";
            UriBuilder uriBuilder = new (requestUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["nodeId"] = nodeId;
            query["grpName"] = GroupNameFolder;
            uriBuilder.Query = query.ToString();
            string finalUrl = uriBuilder.ToString();

            var authHeader = await GetAuthenticationHeaderAsync();
            using var request = new HttpRequestMessage(HttpMethod.Put, requestUrl);
            request.Headers.Authorization = authHeader;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));

            StringContent content = new (body, Encoding.UTF8, accept);
            request.Content=content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
        }
        public async Task EditFile(string nodeId)
        {
            string accept = "application/json";
            string cookie = "Cookie";
            string cookieDate = "cookiesession1=678ADA5C33A30F49D180AB6CBD34D5FC";
            string baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateMetadata/1.0";
            string finalUrl = $"{baseUrl}?nodeId={nodeId}&grpName={GroupNameFolder}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));

            var authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            var response = await _httpClient.PutAsync(finalUrl, null);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}
