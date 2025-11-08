using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IRepairDeleteHandler
    {
        Task Handle(RepairDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
