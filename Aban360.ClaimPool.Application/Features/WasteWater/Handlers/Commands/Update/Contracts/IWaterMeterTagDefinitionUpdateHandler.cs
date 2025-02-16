using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts
{
    public interface IWaterMeterTagDefinitionUpdateHandler
    {
        Task Handle(WaterMeterTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
