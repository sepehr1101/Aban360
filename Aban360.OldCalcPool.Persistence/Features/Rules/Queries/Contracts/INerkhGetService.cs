using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface INerkhGetService
    {
        Task<NerkhGetDto> Get(int id, int nerkh);
        Task<NerkhGetDto> Get(int id);
    }
}
