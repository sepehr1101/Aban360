using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Implementations
{
    internal sealed class SiphonTypeDeleteHandler : ISiphonTypeDeleteHandler
    {
        private readonly ISiphonTypeCommandService _commandService;
        private readonly ISiphonTypeQueryService _queryService;
        public SiphonTypeDeleteHandler(
            ISiphonTypeCommandService commandService,
            ISiphonTypeQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(SiphonTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            SiphonType siphonType = await _queryService.Get(deleteDto.Id);
            await _commandService.Remove(siphonType);
        }
    }
}
