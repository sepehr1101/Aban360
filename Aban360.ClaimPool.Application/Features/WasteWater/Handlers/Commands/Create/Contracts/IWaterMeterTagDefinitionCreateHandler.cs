using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts
{
    public interface IWaterMeterTagDefinitionCreateHandler
    {
        Task Handle(WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken);
    }
}
