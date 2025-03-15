using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class ProfessionDeleteHandler : IProfessionDeleteHandler
    {
        private readonly IProfessionQueryService _queryService;
        private readonly IProfessionCommandService _commandService;
        public ProfessionDeleteHandler(
            IProfessionQueryService queryService,
            IProfessionCommandService commandService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(ProfessionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Profession profession = await _queryService.Get(deleteDto.Id);
            if (profession == null)
            {
                throw new InvalidDataException();
            }
            await _commandService.Remove(profession);

        }
    }
}
