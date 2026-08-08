using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface ICollectBillsQueryService
    {
        Task<IEnumerable<CollectBillsDataDto>> Get();
    }
}
