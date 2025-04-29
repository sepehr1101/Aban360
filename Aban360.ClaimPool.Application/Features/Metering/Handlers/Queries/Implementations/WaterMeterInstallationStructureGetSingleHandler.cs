using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class WaterMeterInstallationStructureGetSingleHandler : IWaterMeterInstallationStructureGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterInstallationStructureQueryService _waterMeterInstallationStructureQueryService;
        public WaterMeterInstallationStructureGetSingleHandler(
            IMapper mapper,
            IWaterMeterInstallationStructureQueryService waterMeterInstallationStructureQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterMeterInstallationStructureQueryService = waterMeterInstallationStructureQueryService;
            _waterMeterInstallationStructureQueryService.NotNull(nameof(_waterMeterInstallationStructureQueryService));
        }

        public async Task<WaterMeterInstallationStructureGetDto> Handle(WaterMeterInstallationStructureEnum id, CancellationToken cancellationToken)
        {
            var waterMeterInstallationStructure = await _waterMeterInstallationStructureQueryService.Get(id);
            return _mapper.Map<WaterMeterInstallationStructureGetDto>(waterMeterInstallationStructure);
        }
    }
}
