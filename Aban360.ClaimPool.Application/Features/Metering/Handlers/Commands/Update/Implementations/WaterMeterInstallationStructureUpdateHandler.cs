using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterInstallationStructureUpdateHandler : IWaterMeterInstallationStructureUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterInstallationStructureQueryService _waterMeterInstallationStructureQueryService;
        public WaterMeterInstallationStructureUpdateHandler(
            IMapper mapper,
            IWaterMeterInstallationStructureQueryService waterMeterInstallationStructureQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterMeterInstallationStructureQueryService = waterMeterInstallationStructureQueryService;
            _waterMeterInstallationStructureQueryService.NotNull(nameof(_waterMeterInstallationStructureQueryService));
        }

        public async Task Handle(WaterMeterInstallationStructureUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var waterMeterInstallationStructure = await _waterMeterInstallationStructureQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, waterMeterInstallationStructure);
        }
    }
}
