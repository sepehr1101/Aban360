using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    internal sealed class WaterMeterInstallationMethodDeleteHandler : IWaterMeterInstallationMethodDeleteHandler
    {
        private readonly IWaterMeterInstallationMethodCommandService _waterMeterInstallationMethodCommandService;
        private readonly IWaterMeterInstallationMethodQueryService _waterMeterInstallationMethodQueryService;
        public WaterMeterInstallationMethodDeleteHandler(
            IWaterMeterInstallationMethodCommandService waterMeterInstallationMethodCommandService,
            IWaterMeterInstallationMethodQueryService waterMeterInstallationMethodQueryService)
        {
            _waterMeterInstallationMethodCommandService = waterMeterInstallationMethodCommandService;
            _waterMeterInstallationMethodCommandService.NotNull(nameof(_waterMeterInstallationMethodCommandService));

            _waterMeterInstallationMethodQueryService = waterMeterInstallationMethodQueryService;
            _waterMeterInstallationMethodQueryService.NotNull(nameof(_waterMeterInstallationMethodQueryService));
        }

        public async Task Handle(WaterMeterInstallationMethodDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethod = await _waterMeterInstallationMethodQueryService.Get(deleteDto.Id);
            await _waterMeterInstallationMethodCommandService.Remove(waterMeterInstallationMethod);
        }
    }
}
