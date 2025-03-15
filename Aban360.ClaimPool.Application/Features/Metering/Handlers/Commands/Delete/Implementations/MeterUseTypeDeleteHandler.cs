using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    internal sealed class MeterUseTypeDeleteHandler : IMeterUseTypeDeleteHandler
    {
        private readonly IMeterUseTypeQueryService _meterUseTypeQueryService;
        private readonly IMeterUseTypeCommandService _meterUseTypeCommandService;
        public MeterUseTypeDeleteHandler(
            IMeterUseTypeQueryService meterUseTypeQueryService,
            IMeterUseTypeCommandService meterUseTypeCommandService)
        {
            _meterUseTypeQueryService = meterUseTypeQueryService;
            _meterUseTypeQueryService.NotNull(nameof(meterUseTypeQueryService));

            _meterUseTypeCommandService = meterUseTypeCommandService;
            _meterUseTypeCommandService.NotNull(nameof(meterUseTypeCommandService));
        }

        public async Task Handle(MeterUseTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            MeterUseType meterUseType = await _meterUseTypeQueryService.Get(deleteDto.Id);
            if (meterUseType == null)
            {
                throw new InvalidDataException();
            }
            await _meterUseTypeCommandService.Remove(meterUseType);
        }
    }
}
