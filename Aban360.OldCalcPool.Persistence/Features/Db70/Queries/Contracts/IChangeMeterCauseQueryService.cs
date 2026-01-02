using Aban360.Common.BaseEntities;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts
{
    public interface IChangeMeterCauseQueryService
    {
        Task<NumericDictionary> Get(int id);
        Task<IEnumerable<NumericDictionary>> Get();
    }
}
