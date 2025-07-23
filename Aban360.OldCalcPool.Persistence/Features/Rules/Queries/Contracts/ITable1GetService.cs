using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface ITable1GetService
    {
        Task<IEnumerable<Table1GetDto>> Get(int id);
    }
}
