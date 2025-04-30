using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts
{
    public interface IWaterMeterInstallationMethodDeleteHandler
    {
        Task Handle(WaterMeterInstallationMethodDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
