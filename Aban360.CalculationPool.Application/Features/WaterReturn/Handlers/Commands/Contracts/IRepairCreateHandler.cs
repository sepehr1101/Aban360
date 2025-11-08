using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IRepairCreateHandler
    {
        Task Handle(OfferingToCreateRepairDto createDto, CancellationToken cancellationToken);
    }
}
