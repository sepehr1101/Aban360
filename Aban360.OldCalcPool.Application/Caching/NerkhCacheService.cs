using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Aban360.OldCalcPool.Application.Caching
{
    public interface INerkhCacheService
    {
        Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int)> Get(NerkhByConsumptionInputDto input);

        Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int, IEnumerable<NerkhGetDto>)> GetWithAggregatedNerkh(NerkhByConsumptionInputDto input);
    }
    public sealed class NerkhCacheService : INerkhCacheService
    {
        const int _expireDays= 30;
        private readonly IMemoryCache _cache;
        private readonly INerkhGetByConsumptionService _inner;

        public NerkhCacheService(IMemoryCache cache, INerkhGetByConsumptionService inner)
        {
            _cache = cache;
            _inner = inner;
        }

        public async Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int)>
            Get(NerkhByConsumptionInputDto input)
        {
            string key = GetCacheKey("NerkhGet", input);

            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_expireDays);
                return await _inner.Get(input);
            });
        }

        public async Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int, IEnumerable<NerkhGetDto>)>
            GetWithAggregatedNerkh(NerkhByConsumptionInputDto input)
        {
            //return await _inner.GetWithAggregatedNerkh(input);//TODO: use cache sometimes
            string key = GetCacheKey("NerkhGetAggr", input);

            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(_expireDays);
                return await _inner.GetWithAggregatedNerkh(input);
            });
        }

        private string GetCacheKey(string prefix, NerkhByConsumptionInputDto input)
        {
            int upperBound = (int)Math.Ceiling(input.AverageConsumption);
            int lowerBound = upperBound - 1;
            return $"{prefix}_{input.ZoneId}_{input.UsageId}_{lowerBound}_{upperBound}_{input.PreviousDateJalali}_{input.CurrentDateJalali}";
        }
    }
}
