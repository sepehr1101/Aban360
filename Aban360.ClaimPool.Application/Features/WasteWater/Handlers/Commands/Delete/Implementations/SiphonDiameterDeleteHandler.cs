using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Implementations
{
    internal sealed class SiphonDiameterDeleteHandler : ISiphonDiameterDeleteHandler
    {
        private readonly ISiphonDiameterCommandService _commandService;
        private readonly ISiphonDiameterQueryService _queryService;
        public SiphonDiameterDeleteHandler(
            ISiphonDiameterCommandService commandService,
            ISiphonDiameterQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(SiphonDiameterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            SiphonDiameter siphonDiameter = await _queryService.Get(deleteDto.Id);
            await _commandService.Remove(siphonDiameter);
        }
    }
}
