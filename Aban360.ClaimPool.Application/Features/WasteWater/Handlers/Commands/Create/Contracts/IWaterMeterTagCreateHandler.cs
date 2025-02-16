using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts
{
    public interface IWaterMeterTagCreateHandler
    {
        Task Handle(WaterMeterTagCreateDto createDto, CancellationToken cancellationToken);
    }
}
