using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IOfferingGroupQueryService
    {
        Task<OfferingGroup> Get(short id);
        Task<ICollection<OfferingGroup>> Get();
    }
}
