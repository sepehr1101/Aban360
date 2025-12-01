namespace Aban360.OldCalcPool.Application.Caching
{
    using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
    using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
    using Microsoft.Extensions.Caching.Memory;

    public interface ITable1CacheService
    {
        Task<Table1GetDto> Get(int id);
        Task<Table1GetDto> GetByTown(int town);
    }

    public sealed class Table1CacheService : ITable1CacheService
    {
        const int _expireHours = 24;
        private readonly IMemoryCache _cache;
        private readonly ITable1GetService _table1GetService;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public Table1CacheService(IMemoryCache cache, ITable1GetService table1GetService)
        {
            _cache = cache;
            _table1GetService = table1GetService;

            // Configure default cache lifetime
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(_expireHours));
        }

        public async Task<Table1GetDto> Get(int id)
        {
            string key = $"Table1_Id_{id}";

            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _table1GetService.Get(id);
            });
        }

        public async Task<Table1GetDto> GetByTown(int town)
        {
            string key = $"Table1_Town_{town}";

            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _table1GetService.GetByTown(town);
            });
        }
    }

}
