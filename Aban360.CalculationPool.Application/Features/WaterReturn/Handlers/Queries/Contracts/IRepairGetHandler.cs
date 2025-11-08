using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Queries.Contracts
{
    public interface IRepairGetHandler
    {
        Task<RepairGetDto> Handle(SearchById input, CancellationToken cancellationToken);
    }
}
