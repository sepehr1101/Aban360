using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface INerkhGetByConsumptionService
    {
        Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadGetDto>, IEnumerable<ZaribGetDto>, int)> Get(NerkhByConsumptionInputDto input);
        Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadGetDto>, IEnumerable<ZaribGetDto>, int)> GetWithAggregatedNerkh(NerkhByConsumptionInputDto input);
    }
}
