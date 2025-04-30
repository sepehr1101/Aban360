using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    internal sealed class WaterMeterInstallationMethodCreateHandler : IWaterMeterInstallationMethodCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterInstallationMethodCommandService _waterMeterInstallationMethodCommandService;
        public WaterMeterInstallationMethodCreateHandler(
            IMapper mapper,
            IWaterMeterInstallationMethodCommandService waterMeterInstallationMethodCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterMeterInstallationMethodCommandService = waterMeterInstallationMethodCommandService;
            _waterMeterInstallationMethodCommandService.NotNull(nameof(_waterMeterInstallationMethodCommandService));
        }

        public async Task Handle(WaterMeterInstallationMethodCreateDto createDto, CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethod = _mapper.Map<WaterMeterInstallationMethod>(createDto);
            await _waterMeterInstallationMethodCommandService.Add(waterMeterInstallationMethod);
        }
    }
}
