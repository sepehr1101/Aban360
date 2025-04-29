using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts
{
    public interface IWaterMeterInstallationStructureUpdateHandler
    {
        Task Handle(WaterMeterInstallationStructureUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
