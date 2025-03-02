using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IOfferingUnitQueryService
    {
        Task<OfferingUnit> Get(short id);
        Task<ICollection<OfferingUnit>> Get();
    }
}
