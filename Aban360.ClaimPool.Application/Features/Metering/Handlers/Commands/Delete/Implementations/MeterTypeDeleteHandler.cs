using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    internal sealed class MeterTypeDeleteHandler : IMeterTypeDeleteHandler
    {
        private readonly IMeterTypeQueryService _meterTypeQueryService;
        private readonly IMeterTypeCommandService _meterTypeCommandService;
        public MeterTypeDeleteHandler(
            IMeterTypeQueryService meterTypeQueryService,
            IMeterTypeCommandService meterTypeCommandService)
        {
            _meterTypeQueryService = meterTypeQueryService;
            _meterTypeQueryService.NotNull(nameof(meterTypeQueryService));

            _meterTypeCommandService = meterTypeCommandService;
            _meterTypeCommandService.NotNull(nameof(meterTypeCommandService));
        }

        public async Task Handle(MeterTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            MeterType meterType = await _meterTypeQueryService.Get(deleteDto.Id);
            if (meterType == null)
            {
                throw new InvalidDataException();
            }
            await _meterTypeCommandService.Remove(meterType);
        }
    }
}
