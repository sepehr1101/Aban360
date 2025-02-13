using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    public class MeterMaterialCreateHandler : IMeterMaterialCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterMaterialCommandService _meterMaterialCommandService;
        public MeterMaterialCreateHandler(
            IMapper mapper,
            IMeterMaterialCommandService meterMaterialCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterMaterialCommandService = meterMaterialCommandService;
            _meterMaterialCommandService.NotNull(nameof(_meterMaterialCommandService));
        }

        public async Task Handle(MeterMaterialCreateDto ceateDto, CancellationToken cancellationToken)
        {
            var meterMaterial = _mapper.Map<MeterMaterial>(ceateDto);
            await _meterMaterialCommandService.Add(meterMaterial);
        }
    }
}
