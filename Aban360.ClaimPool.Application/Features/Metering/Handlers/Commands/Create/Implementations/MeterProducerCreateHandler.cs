using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    internal sealed class MeterProducerCreateHandler : IMeterProducerCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterProducerCommandService _meterProducerCommandService;
        public MeterProducerCreateHandler(
            IMapper mapper,
            IMeterProducerCommandService meterProducerCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterProducerCommandService = meterProducerCommandService;
            _meterProducerCommandService.NotNull(nameof(meterProducerCommandService));
        }

        public async Task Handle(MeterProducerCreateDto createDto, CancellationToken cancellationToken)
        {
            MeterProducer meterProducer = _mapper.Map<MeterProducer>(createDto);
            await _meterProducerCommandService.Add(meterProducer);
        }
    }

}
