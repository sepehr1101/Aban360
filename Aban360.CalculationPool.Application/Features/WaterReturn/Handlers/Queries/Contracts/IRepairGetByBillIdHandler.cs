using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Queries.Contracts
{
    public interface IRepairGetByBillIdHandler
    {
        Task<IEnumerable<RepairGetDto>> Handle(SearchInput input, CancellationToken cancellationToken);
    }
}
