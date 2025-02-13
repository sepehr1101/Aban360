using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    public class MeterProducerGetAllHandler : IMeterProducerGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterProducerQueryService _meterProducerQueryService;
        public MeterProducerGetAllHandler(
            IMapper mapper,
            IMeterProducerQueryService meterProducerQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterProducerQueryService = meterProducerQueryService;
            _meterProducerQueryService.NotNull(nameof(meterProducerQueryService));
        }

        public async Task<ICollection<MeterProducerGetDto>> Handle(CancellationToken cancellationToken)
        {
            var meterProducer = await _meterProducerQueryService.Get();
            if (meterProducer == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<MeterProducerGetDto>>(meterProducer);
        }
    }

}
