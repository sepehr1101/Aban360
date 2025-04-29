using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    internal sealed class WaterMeterInstallationStructureDeleteHandler : IWaterMeterInstallationStructureDeleteHandler
    {
        private readonly IWaterMeterInstallationStructureCommandService _waterMeterInstallationStructureCommandService;
        private readonly IWaterMeterInstallationStructureQueryService _waterMeterInstallationStructureQueryService;
        public WaterMeterInstallationStructureDeleteHandler(
            IWaterMeterInstallationStructureCommandService waterMeterInstallationStructureCommandService,
            IWaterMeterInstallationStructureQueryService waterMeterInstallationStructureQueryService)
        {
            _waterMeterInstallationStructureCommandService = waterMeterInstallationStructureCommandService;
            _waterMeterInstallationStructureCommandService.NotNull(nameof(_waterMeterInstallationStructureCommandService));

            _waterMeterInstallationStructureQueryService = waterMeterInstallationStructureQueryService;
            _waterMeterInstallationStructureQueryService.NotNull(nameof(_waterMeterInstallationStructureQueryService));
        }

        public async Task Handle(WaterMeterInstallationStructureDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var waterMeterInstallationStructure = await _waterMeterInstallationStructureQueryService.Get(deleteDto.Id);
            await _waterMeterInstallationStructureCommandService.Remove(waterMeterInstallationStructure);
        }
    }
}
