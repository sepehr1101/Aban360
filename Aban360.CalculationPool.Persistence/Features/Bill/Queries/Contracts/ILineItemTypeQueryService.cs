using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface ILineItemTypeQueryService
    {
        Task<LineItemType> Get(short id);
        Task<ICollection<LineItemType>> Get();
    }
}
