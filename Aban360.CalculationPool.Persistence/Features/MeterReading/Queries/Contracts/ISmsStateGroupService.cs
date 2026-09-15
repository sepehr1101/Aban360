using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts
{
    public interface ISmsStateGroupService
    {
        Task<NumericDictionary> Get(int id);
        Task<NumericDictionary?> Get(string title, bool hasException);
        Task<IEnumerable<NumericDictionary>> Get();
    }
}
