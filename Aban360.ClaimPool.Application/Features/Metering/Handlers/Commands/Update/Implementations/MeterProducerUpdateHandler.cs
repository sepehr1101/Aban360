using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class MeterProducerUpdateHandler : IMeterProducerUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterProducerQueryService _meterProducerQueryService;
        public MeterProducerUpdateHandler(
            IMapper mapper,
            IMeterProducerQueryService meterProducerQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterProducerQueryService = meterProducerQueryService;
            _meterProducerQueryService.NotNull(nameof(meterProducerQueryService));
        }

        public async Task Handle(MeterProducerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            MeterProducer meterProducer = await _meterProducerQueryService.Get(updateDto.Id);
            if (meterProducer == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, meterProducer);
        }
    }

}
