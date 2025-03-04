using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface ILineItemTypeGroupQueryService
    {
        Task<LineItemTypeGroup> Get(short id);
        Task<ICollection<LineItemTypeGroup>> Get();
    }
}
