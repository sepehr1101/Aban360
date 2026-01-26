using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface IAdjustmentFactorQueryService
    {
        Task<IEnumerable<AdjustmentFactorGetDto>> Get();
        Task<AdjustmentFactorGetDto> Get(int zoneId);
    }
}