using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts
{
    public interface IWaterMeterUpdateHandler
    {
        Task Handle(WaterMeterUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
