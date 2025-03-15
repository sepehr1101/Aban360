using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    internal sealed class MeterMaterialDeleteHandler : IMeterMaterialDeleteHandler
    {
        private readonly IMeterMaterialCommandService _meterMaterialCommandService;
        private readonly IMeterMaterialQueryService _meterMaterialQueryService;
        public MeterMaterialDeleteHandler(
            IMeterMaterialCommandService meterMaterialCommandService,
            IMeterMaterialQueryService meterMaterialQueryService)
        {
            _meterMaterialCommandService = meterMaterialCommandService;
            _meterMaterialCommandService.NotNull(nameof(meterMaterialCommandService));

            _meterMaterialQueryService = meterMaterialQueryService;
            _meterMaterialQueryService.NotNull(nameof(meterMaterialQueryService));
        }

        public async Task Handle(MeterMaterialDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            MeterMaterial meterMaterial = await _meterMaterialQueryService.Get(deleteDto.Id);
            if (meterMaterial == null)
            {
                throw new InvalidDataException();
            }
            await _meterMaterialCommandService.Remove(meterMaterial);
        }
    }
}
