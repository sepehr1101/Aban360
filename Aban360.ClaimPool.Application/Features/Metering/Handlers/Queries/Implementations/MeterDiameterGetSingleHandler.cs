using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    public class MeterDiameterGetSingleHandler : IMeterDiameterGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterDiameterQueryService _meterDiameterQueryService;
        public MeterDiameterGetSingleHandler(
            IMapper mapper,
            IMeterDiameterQueryService meterDiameterQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterDiameterQueryService = meterDiameterQueryService;
            _meterDiameterQueryService.NotNull(nameof(meterDiameterQueryService));

        }

        public async Task<MeterDiameterGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var meterDiameter = await _meterDiameterQueryService.Get(id);
            if (meterDiameter == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<MeterDiameterGetDto>(meterDiameter);
        }
    }
}

