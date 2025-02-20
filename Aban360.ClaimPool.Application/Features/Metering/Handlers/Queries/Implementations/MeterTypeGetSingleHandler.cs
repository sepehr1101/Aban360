using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    public class MeterTypeGetSingleHandler : IMeterTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterTypeQueryService _meterTypeQueryService;
        public MeterTypeGetSingleHandler(IMapper mapper,
            IMeterTypeQueryService meterTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterTypeQueryService = meterTypeQueryService;
            _meterTypeQueryService.NotNull(nameof(meterTypeQueryService));
        }

        public async Task<MeterTypeGetDto> Handle(short id,CancellationToken cancellationToken)
        {
            var meterType = await _meterTypeQueryService.Get(id);
            if (meterType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<MeterTypeGetDto>(meterType);
        }
    }
}
