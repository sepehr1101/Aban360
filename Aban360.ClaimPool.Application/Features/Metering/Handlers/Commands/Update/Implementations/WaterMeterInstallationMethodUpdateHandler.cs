using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterInstallationMethodUpdateHandler : IWaterMeterInstallationMethodUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterInstallationMethodQueryService _waterMeterInstallationMethodQueryService;
        public WaterMeterInstallationMethodUpdateHandler(
            IMapper mapper,
            IWaterMeterInstallationMethodQueryService waterMeterInstallationMethodQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterMeterInstallationMethodQueryService = waterMeterInstallationMethodQueryService;
            _waterMeterInstallationMethodQueryService.NotNull(nameof(_waterMeterInstallationMethodQueryService));
        }

        public async Task Handle(WaterMeterInstallationMethodUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethod = await _waterMeterInstallationMethodQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, waterMeterInstallationMethod);
        }
    }
}
