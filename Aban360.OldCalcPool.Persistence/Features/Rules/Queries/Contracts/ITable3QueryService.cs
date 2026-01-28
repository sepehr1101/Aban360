using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface ITable3QueryService
    {
        Task<IEnumerable<Table3GetDto>> Get(Table3InputDto input);
    }
}
