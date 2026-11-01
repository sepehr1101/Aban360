using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface IOfferingQueryService
    {
        Task<NumericDictionary> Get(short id);
        Task<IEnumerable<NumericDictionary>> Get(IEnumerable<short> ids);
    }
}
