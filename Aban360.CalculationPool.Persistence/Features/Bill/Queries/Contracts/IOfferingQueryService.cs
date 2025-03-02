using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IOfferingQueryService
    {
        Task<Offering> Get(short id);
        Task<ICollection<Offering>> Get();
    }
}
