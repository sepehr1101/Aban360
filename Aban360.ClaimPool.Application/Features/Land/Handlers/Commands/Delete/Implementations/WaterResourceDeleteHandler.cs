using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class WaterResourceDeleteHandler : IWaterResourceDeleteHandler
    {
        private readonly IWaterResourceCommandService _waterResourceCommandService;
        private readonly IWaterResourceQueryService _waterResourceQueryService;
        public WaterResourceDeleteHandler(
            IWaterResourceCommandService WaterResourceCommandService,
            IWaterResourceQueryService WaterResourceQueryService)
        {
            _waterResourceCommandService = WaterResourceCommandService;
            _waterResourceCommandService.NotNull(nameof(_waterResourceCommandService));

            _waterResourceQueryService = WaterResourceQueryService;
            _waterResourceQueryService.NotNull(nameof(_waterResourceQueryService));
        }

        public async Task Handle(WaterResourceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            WaterResource waterResource = await _waterResourceQueryService.Get(deleteDto.Id);
            await _waterResourceCommandService.Remove(waterResource);
        }
    }
}
