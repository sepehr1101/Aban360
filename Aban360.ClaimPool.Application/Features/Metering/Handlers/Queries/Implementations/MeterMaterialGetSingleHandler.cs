using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class MeterMaterialGetSingleHandler : IMeterMaterialGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterMaterialQueryService _meterMaterialQueryService;
        public MeterMaterialGetSingleHandler(
            IMapper mapper,
            IMeterMaterialQueryService meterMaterialQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterMaterialQueryService = meterMaterialQueryService;
            _meterMaterialQueryService.NotNull(nameof(meterMaterialQueryService));
        }

        public async Task<MeterMaterialGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            MeterMaterial meterMaterial = await _meterMaterialQueryService.Get(id);
            if (meterMaterial == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<MeterMaterialGetDto>(meterMaterial);
        }
    }
}
