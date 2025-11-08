using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.CalculationPool.Persistence.Features.WaterReturn.Command.Contracts
{
    public interface IRepairCommandService
    {
        Task Create(RepairCreateDto input);
        Task Delete(RepairDeleteDto input);
        Task Update(RepairUpdateDto input);
    }
}
