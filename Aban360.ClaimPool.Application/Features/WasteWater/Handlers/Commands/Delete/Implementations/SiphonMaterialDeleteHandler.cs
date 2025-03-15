using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Implementations
{
    internal sealed class SiphonMaterialDeleteHandler : ISiphonMaterialDeleteHandler
    {
        private readonly ISiphonMaterialCommandService _commandService;
        private readonly ISiphonMaterialQueryService _queryService;
        public SiphonMaterialDeleteHandler(
            ISiphonMaterialCommandService commandService,
            ISiphonMaterialQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(SiphonMaterialDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            SiphonMaterial siphonMaterial = await _queryService.Get(deleteDto.Id);
            await _commandService.Remove(siphonMaterial);
        }
    }
}
