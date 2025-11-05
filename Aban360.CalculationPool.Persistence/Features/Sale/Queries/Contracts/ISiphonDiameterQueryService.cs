using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface ISiphonDiameterQueryService
    {
        Task<IEnumerable<NumericDictionary>> Get();
    }
}
