using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts
{
    public interface IWaterMeterTagUpdateHandler
    {
        Task Handle(WaterMeterTagUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
