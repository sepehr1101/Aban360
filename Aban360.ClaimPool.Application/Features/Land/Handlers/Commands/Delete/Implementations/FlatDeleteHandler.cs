using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    public class FlatDeleteHandler : IFlatDeleteHandler
    {
        private readonly IFlatQueryService _queryService;
        private readonly IFlatCommandService _commandService;
        public FlatDeleteHandler(
            IFlatQueryService queryService,
            IFlatCommandService commandService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(FlatDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Flat flat = await _queryService.Get(deleteDto.Id);
            if (flat == null)
            {
                throw new InvalidDataException();
            }
            await _commandService.Remove(flat);

        }
    }
}
