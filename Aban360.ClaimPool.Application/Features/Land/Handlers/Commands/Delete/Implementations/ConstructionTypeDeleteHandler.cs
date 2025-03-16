using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class ConstructionTypeDeleteHandler : IConstructionTypeDeleteHandler
    {
        private readonly IConstructionTypeQueryService _queryService;
        private readonly IConstructionTypeCommandService _commandService;
        public ConstructionTypeDeleteHandler(
            IConstructionTypeQueryService queryService,
            IConstructionTypeCommandService commandService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _commandService = commandService;
            _commandService.NotNull(nameof(_commandService));
        }

        public async Task Handle(ConstructionTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            ConstructionType constructionType = await _queryService.Get(deleteDto.Id);
            await _commandService.Remove(constructionType);
        }
    }

}
