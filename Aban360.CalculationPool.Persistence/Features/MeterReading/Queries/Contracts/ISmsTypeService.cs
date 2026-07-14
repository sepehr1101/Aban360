using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts
{
    public interface ISmsTypeService
    {
        Task Insert(string title);
        Task<NumericDictionary> Get(int id);
        Task<IEnumerable<NumericDictionary>> Get();
    }
}
