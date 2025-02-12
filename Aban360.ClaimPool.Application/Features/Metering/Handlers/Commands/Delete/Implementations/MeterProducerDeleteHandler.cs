using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    public class MeterProducerDeleteHandler : IMeterProducerDeleteHandler
    {
        private readonly IMeterProducerQueryService _meterProducerQueryService;
        private readonly IMeterProducerCommandService _meterProducerCommandService;
        public MeterProducerDeleteHandler(
            IMeterProducerQueryService meterProducerQueryService,
            IMeterProducerCommandService meterProducerCommandService)
        {
            _meterProducerQueryService = meterProducerQueryService;
            _meterProducerQueryService.NotNull(nameof(meterProducerQueryService));

            _meterProducerCommandService = meterProducerCommandService;
            _meterProducerCommandService.NotNull(nameof(meterProducerCommandService));
        }

        public async Task Handle(MeterProducerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var meterProducer = await _meterProducerQueryService.Get(deleteDto.Id);
            if (meterProducer == null)
            {
                throw new InvalidDataException();
            }
            await _meterProducerCommandService.Remove(meterProducer);
        }
    }

}
