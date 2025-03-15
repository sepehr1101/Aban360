using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class MeterProducerGetSingleHandler : IMeterProducerGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterProducerQueryService _meterProducerQueryService;
        public MeterProducerGetSingleHandler(
            IMapper mapper,
            IMeterProducerQueryService meterProducerQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterProducerQueryService = meterProducerQueryService;
            _meterProducerQueryService.NotNull(nameof(meterProducerQueryService));
        }

        public async Task<MeterProducerGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var meterProducer = await _meterProducerQueryService.Get(id);
            if (meterProducer == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<MeterProducerGetDto>(meterProducer);
        }
    }

}
