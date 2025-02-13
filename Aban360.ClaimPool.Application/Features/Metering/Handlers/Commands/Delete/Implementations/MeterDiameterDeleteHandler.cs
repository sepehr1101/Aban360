using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    public class MeterDiameterDeleteHandler : IMeterDiameterDeleteHandler
    {
        private readonly IMeterDiameterQueryService _meterDiameterQueryService;
        private readonly IMeterDiameterCommandService _meterDiameterCommandService;
        public MeterDiameterDeleteHandler(
            IMeterDiameterQueryService meterDiameterQueryService,
            IMeterDiameterCommandService meterDiameterCommandService)
        {
            _meterDiameterQueryService = meterDiameterQueryService;
            _meterDiameterQueryService.NotNull(nameof(meterDiameterQueryService));

            _meterDiameterCommandService = meterDiameterCommandService;
            _meterDiameterCommandService.NotNull(nameof(meterDiameterCommandService));
        }

        public async Task Handle(MeterDiameterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var meterDiameter = await _meterDiameterQueryService.Get(deleteDto.Id);
            if (meterDiameter == null)
            {
                throw new InvalidDataException();
            }
            await _meterDiameterCommandService.Remove(meterDiameter);

        }
    }
}

