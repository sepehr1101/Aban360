using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts
{
    public interface IWaterMeterTagDeleteHandler
    {
        Task Handle(WaterMeterTagDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
