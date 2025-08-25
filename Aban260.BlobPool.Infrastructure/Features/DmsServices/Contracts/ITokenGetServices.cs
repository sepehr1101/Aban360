using Aban360.Common.Extensions;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface ITokenGetServices
    {
        Task<string> Service();
    }
    internal sealed class TokenGetServices : ITokenGetServices
    {
        private readonly IHttpClientFactory _httpClientFactoryFactory;
        static string _token;//todo: remove static
        static DateTime _expireTime;

        string url = $"https://esb.abfaisfahan.com:8243/token";
        string _basicToken = "YkNPSGNGUXJmY3F2UG03OTJkdFVDNXdfZkxRYTpFTmxkUThBTVJHWVNxVk5meFN6SGI5a2VyQXdh";
        string _basic = "Basic";
        public TokenGetServices(IHttpClientFactory httpClientFactoryFactory)
        {
            _httpClientFactoryFactory = httpClientFactoryFactory;
            _httpClientFactoryFactory.NotNull(nameof(httpClientFactoryFactory));
        }

        public async Task<string> Service()
        {
            if (!string.IsNullOrEmpty(_token) && _expireTime > DateTime.UtcNow)
            {
                return _token;
            }
            var client = _httpClientFactoryFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(_basic, _basicToken);

            var body = new Dictionary<string, string>()
            {
                {"grant_type","client_credentials" }
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls|SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 ;

            var content = new FormUrlEncodedContent(body);
            var respone = await client.PostAsync(url, content);
            respone.EnsureSuccessStatusCode();

            var jsonResponse = await respone.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<GetTokenDto>(jsonResponse);
            _token = tokenResponse.access_token;
            _expireTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpireTime);

            return tokenResponse.access_token;
        }
    }
    public record GetTokenDto
    {
        public int ExpireTime { get; set; }
        public string access_token { get; set; }
    }
}
