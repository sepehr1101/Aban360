using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IRepairUpdateHandler
    {
        Task Handle(RepairUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
