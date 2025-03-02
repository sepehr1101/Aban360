using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    public class GuildDeleteHandler : IGuildDeleteHandler
    {
        private readonly IGuildQueryService _queryService;
        private readonly IGuildCommandService _commandService;
        public GuildDeleteHandler(
            IGuildQueryService queryService,
            IGuildCommandService commandService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(GuildDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var guild = await _queryService.Get(deleteDto.Id);
            if (guild == null)
            {
                throw new InvalidDataException();
            }
            await _commandService.Remove(guild);

        }
    }
}
