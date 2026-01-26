using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface IAjustmentFactorQueryService
    {
        Task<IEnumerable<AjustmentFactorGetDto>> Get();
        Task<AjustmentFactorGetDto> Get(int zoneId);
    }
}