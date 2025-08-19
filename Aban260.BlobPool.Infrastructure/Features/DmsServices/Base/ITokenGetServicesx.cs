using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;
using System;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Base
{
    public interface ITokenGetServicesx
    {
        Task<string> Service();
    }
    internal sealed class TokenGetServicesx : ITokenGetServicesx
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpClientConfigureServices _configureServices;
        string Url = $"token";
        string Accept = $"token";
        string _basic = "Basic";
        string _basicToken = "UEtxbkJ1enVNRXM0aEFySV9CUGZZaWhKS1lNYTpZUlFFZjcyR29zc0oyZ0dtMmhFMU5PTVZhVDhh";

        string _token;
        DateTime _expireTime;
        public TokenGetServicesx(IHttpClientFactory httpClientFactory, IHttpClientConfigureServices configureServices)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

            _configureServices = configureServices;
            _configureServices.NotNull(nameof(configureServices));
        }
        public async Task<string> Service()
        {
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
            _token = tokenResponse.AccessToken;
            _expireTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpireTime);

            return tokenResponse.AccessToken;
        }
    }
}
