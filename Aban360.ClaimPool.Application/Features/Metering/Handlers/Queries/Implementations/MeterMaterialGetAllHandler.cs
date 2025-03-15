using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    public class MeterMaterialGetAllHandler : IMeterMaterialGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterMaterialQueryService _meterMaterialQueryService;
        public MeterMaterialGetAllHandler(
            IMapper mapper,
            IMeterMaterialQueryService meterMaterialQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterMaterialQueryService = meterMaterialQueryService;
            _meterMaterialQueryService.NotNull(nameof(meterMaterialQueryService));
        }

        public async Task<ICollection<MeterMaterialGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<MeterMaterial> meterMaterial = await _meterMaterialQueryService.Get();
            if (meterMaterial == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<MeterMaterialGetDto>>(meterMaterial);
        }
    }
}
