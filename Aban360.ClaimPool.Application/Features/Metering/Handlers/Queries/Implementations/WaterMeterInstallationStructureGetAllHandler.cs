using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class WaterMeterInstallationStructureGetAllHandler : IWaterMeterInstallationStructureGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterInstallationStructureQueryService _waterMeterInstallationStructureQueryService;
        public WaterMeterInstallationStructureGetAllHandler(
            IMapper mapper,
            IWaterMeterInstallationStructureQueryService waterMeterInstallationStructureQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterMeterInstallationStructureQueryService = waterMeterInstallationStructureQueryService;
            _waterMeterInstallationStructureQueryService.NotNull(nameof(_waterMeterInstallationStructureQueryService));
        }

        public async Task<ICollection<WaterMeterInstallationStructureGetDto>> Handle(CancellationToken cancellationToken)
        {
            var waterMeterInstallationStructure = await _waterMeterInstallationStructureQueryService.Get();
            return _mapper.Map<ICollection<WaterMeterInstallationStructureGetDto>>(waterMeterInstallationStructure);
        }
    }
}
