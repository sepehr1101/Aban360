using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class WaterMeterInstallationMethodGetAllHandler : IWaterMeterInstallationMethodGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterInstallationMethodQueryService _waterMeterInstallationMethodQueryService;
        public WaterMeterInstallationMethodGetAllHandler(
            IMapper mapper,
            IWaterMeterInstallationMethodQueryService waterMeterInstallationMethodQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterMeterInstallationMethodQueryService = waterMeterInstallationMethodQueryService;
            _waterMeterInstallationMethodQueryService.NotNull(nameof(_waterMeterInstallationMethodQueryService));
        }

        public async Task<ICollection<WaterMeterInstallationMethodGetDto>> Handle(CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethod = await _waterMeterInstallationMethodQueryService.Get();
            return _mapper.Map<ICollection<WaterMeterInstallationMethodGetDto>>(waterMeterInstallationMethod);
        }
    }
}
