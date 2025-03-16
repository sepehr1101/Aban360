using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class MeterUseTypeGetSingleHandler : IMeterUseTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterUseTypeQueryService _meterUseTypeQueryService;
        public MeterUseTypeGetSingleHandler(
            IMapper mapper,
            IMeterUseTypeQueryService meterUseTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterUseTypeQueryService = meterUseTypeQueryService;
            _meterUseTypeQueryService.NotNull(nameof(meterUseTypeQueryService));
        }

        public async Task<MeterUseTypeGetDto> Handle(MeterUseTypeEnum id, CancellationToken cancellationToken)
        {
            MeterUseType meterUseType = await _meterUseTypeQueryService.Get(id);
            if (meterUseType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<MeterUseTypeGetDto>(meterUseType);
        }
    }
}
