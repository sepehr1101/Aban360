using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts
{
    public interface IWaterMeterInstallationMethodUpdateHandler
    {
        Task Handle(WaterMeterInstallationMethodUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
