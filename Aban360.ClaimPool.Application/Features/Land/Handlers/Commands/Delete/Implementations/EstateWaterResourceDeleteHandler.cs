using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class EstateWaterResourceDeleteHandler : IEstateWaterResourceDeleteHandler
    {
        private readonly IEstateWaterResourceCommandService _estateWaterResourceCommandService;
        private readonly IEstateWaterResourceQueryService _estateWaterResourceQueryService;
        public EstateWaterResourceDeleteHandler(
            IEstateWaterResourceCommandService EstateWaterResourceCommandService,
            IEstateWaterResourceQueryService EstateWaterResourceQueryService)
        {
            _estateWaterResourceCommandService = EstateWaterResourceCommandService;
            _estateWaterResourceCommandService.NotNull(nameof(_estateWaterResourceCommandService));

            _estateWaterResourceQueryService = EstateWaterResourceQueryService;
            _estateWaterResourceQueryService.NotNull(nameof(_estateWaterResourceQueryService));
        }

        public async Task Handle(EstateWaterResourceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            EstateWaterResource estateWaterResource = await _estateWaterResourceQueryService.Get(deleteDto.Id);
            await _estateWaterResourceCommandService.Remove(estateWaterResource);
        }
    }
}
