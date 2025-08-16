using Aban360.Common.Extensions;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IGetTokenServices
    {
        Task<string> Service();
    }
    internal sealed class GetTokenServices : IGetTokenServices
    {
        private readonly IHttpClientFactory _httpClientFactoryFactory;
        static string _token;//todo: remove static
        static DateTime _expireTime;

        string url = $"https://esb.abfaisfahan.com:8243/token";
        string _basicToken = "UEtxbkJ1enVNRXM0aEFySV9CUGZZaWhKS1lNYTpZUlFFZjcyR29zc0oyZ0dtMmhFMU5PTVZhVDhh";
        string _basic = "Basic";
        public GetTokenServices(IHttpClientFactory httpClientFactoryFactory)
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

            var content = new FormUrlEncodedContent(body);
            var respone = await client.PostAsync(url, content);
            respone.EnsureSuccessStatusCode();

            var jsonResponse = await respone.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<GetTokenDto>(jsonResponse);
            _token = tokenResponse.AccessToken;
            _expireTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpireTime);

            return tokenResponse.AccessToken;
        }
    }
    public record GetTokenDto
    {
        public int ExpireTime { get; set; }
        public string AccessToken { get; set; }
    }
}
