using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class EstateBoundTypeDeleteHandler : IEstateBoundTypeDeleteHandler
    {
        private readonly IEstateBoundTypeQueryService _queryService;
        private readonly IEstateBoundTypeCommandService _commandService;
        public EstateBoundTypeDeleteHandler(
           IEstateBoundTypeQueryService queryService,
            IEstateBoundTypeCommandService commandService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _commandService = commandService;
            _commandService.NotNull(nameof(_commandService));
        }

        public async Task Handle(EstateBoundTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            EstateBoundType estateBoundType = await _queryService.Get(deleteDto.Id);
            if (estateBoundType == null)
            {
                throw new InvalidDataException();
            }
            await _commandService.Remove(estateBoundType);
        }
    }
}
