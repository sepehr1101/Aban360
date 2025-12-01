namespace Aban360.OldCalcPool.Application.Caching
{
    using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
    using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
    using Microsoft.Extensions.Caching.Memory;

    public interface IZaribCCacheService
    {
        Task<ZaribCQueryDto> GetZaribC(string from, string to);
        Task<ZaribCQueryDto> GetZaribC(string currentDateJalali);
        Task<IEnumerable<ZaribCQueryDto>> GetZaribC();
    }

    public sealed class ZaribCCacheService : IZaribCCacheService
    {
        const int _expireHours = 24;
        private readonly IMemoryCache _cache;
        private readonly IZaribCQueryService _queryService;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public ZaribCCacheService(IMemoryCache cache, IZaribCQueryService queryService)
        {
            _cache = cache;
            _queryService = queryService;

            // You can tune this as needed.
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_expireHours));
        }

        public async Task<ZaribCQueryDto> GetZaribC(string from, string to)
        {
            string key = $"ZaribC_{from}_{to}";

            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _queryService.GetZaribC(from, to);
            });
        }

        public async Task<ZaribCQueryDto> GetZaribC(string currentDateJalali)
        {
            string key = $"ZaribC_{currentDateJalali}";

            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _queryService.GetZaribC(currentDateJalali);
            });
        }

        public async Task<IEnumerable<ZaribCQueryDto>> GetZaribC()
        {
            string key = "ZaribC_All";

            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _queryService.GetZaribC();
            });
        }
    }

}
