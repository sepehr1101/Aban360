using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts
{
    public interface IOfferingQueryService
    {
        Task<NumericDictionary> Get(SearchByIdInput input);
        Task<IEnumerable<NumericDictionary>> Get();
    }
}
