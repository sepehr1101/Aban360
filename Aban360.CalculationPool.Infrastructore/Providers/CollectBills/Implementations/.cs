using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Buffers;

namespace Aban360.CalculationPool.Infrastructure.Providers.CollectBills.Implementations
{
    public interface ICollectBillsService
    {

    }
    internal sealed class CollectBillsService : ICollectBillsService
    {
        private readonly HttpClient _httpClient;
        private readonly CollectBillsOptions _options;
        private readonly IMemoryCache _catch;

        private const string _tokenCacheKey = "CollectBills_AccessToken";
        public CollectBillsService(
            IHttpClientFactory httpClientFactory,
            IOptions<CollectBillsOptions> options,
            IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientNames.CollectBills);
            _httpClient.NotNull(nameof(_httpClient));

            _options = options.Value;
            _options.NotNull(nameof(_options));
            
            _catch = cache;
            _catch.NotNull(nameof(_catch));
        }

        public async Task<CollectBillsLoginOutputDto> Login()
        {
            if (_catch.TryGetValue(_tokenCacheKey, out CollectBillsLoginOutputDto cachedToken))
            {
                return cachedToken;
            }
        }
        public async Task<CollectBillsLoginOutputDto> GetNewToken()
        {
          
        }

    }
}
