using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class MeterMaterialUpdateHandler : IMeterMaterialUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterMaterialQueryService _meterMaterialQueryService;
        public MeterMaterialUpdateHandler(
            IMapper mapper,
            IMeterMaterialQueryService meterMaterialQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterMaterialQueryService = meterMaterialQueryService;
            _meterMaterialQueryService.NotNull(nameof(meterMaterialQueryService));
        }

        public async Task Handle(MeterMaterialUpdateDto updateDto, CancellationToken cancellationToken)
        {
            MeterMaterial meterMaterial = await _meterMaterialQueryService.Get(updateDto.Id);
            if (meterMaterial == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, meterMaterial);
        }
    }
}
