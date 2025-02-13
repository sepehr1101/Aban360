using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    public class MeterUseTypeGetAllHandler : IMeterUseTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterUseTypeQueryService _meterUseTypeQueryService;
        public MeterUseTypeGetAllHandler(
            IMapper mapper,
            IMeterUseTypeQueryService meterUseTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterUseTypeQueryService = meterUseTypeQueryService;
            _meterUseTypeQueryService.NotNull(nameof(meterUseTypeQueryService));
        }

        public async Task<ICollection<MeterUseTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            var meterUseType = await _meterUseTypeQueryService.Get();
            if (meterUseType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<MeterUseTypeGetDto>>(meterUseType);
        }
    }
}
