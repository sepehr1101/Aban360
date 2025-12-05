using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Aban360.ReportPool.Application.Features.Caching
{
    public interface ITileScriptCacheService
    {
        Task<IEnumerable<TileScriptReportDto>> GetContent(string content, TileScriptContentReportInputDto input);
    }

    internal sealed class TileScriptCacheService : ITileScriptCacheService
    {
        const int _expireHours = 12;
        private readonly ITileScriptService _tileScriptService;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public TileScriptCacheService(
            ITileScriptService tileScriptService,
            IMemoryCache cache)
        {
            _tileScriptService = tileScriptService;
            _cache = cache;

            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_expireHours),
                SlidingExpiration = TimeSpan.FromMinutes(_expireHours/2),
                Priority = CacheItemPriority.Normal
            };
        }

        public async Task<IEnumerable<TileScriptReportDto>> GetContent(string content, TileScriptContentReportInputDto input)
        {
            string cacheKey = BuildCacheKey(content, input);

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _tileScriptService.GetContent(content, input);
            });
        }

        private string BuildCacheKey(string content, TileScriptContentReportInputDto input)
        {            
            return $"TileScript:{content}:{JsonSerializer.Serialize(input)}";
        }
    }
}
