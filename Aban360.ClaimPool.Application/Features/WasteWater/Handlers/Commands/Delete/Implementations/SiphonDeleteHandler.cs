using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Implementations
{
    internal sealed class SiphonDeleteHandler : ISiphonDeleteHandler
    {
        private readonly ISiphonCommandService _commandService;
        private readonly ISiphonQueryService _queryService;
        public SiphonDeleteHandler(
            ISiphonCommandService commandService,
            ISiphonQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(SiphonDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Siphon siphon = await _queryService.Get(deleteDto.Id);
            await _commandService.Remove(siphon);
        }
    }
}
