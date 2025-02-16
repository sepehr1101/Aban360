using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Implementations
{
    public class IndividualTagDefinitionDeleteHandler : IIndividualTagDefinitionDeleteHandler
    {
        private readonly IIndividualTagDefinitionCommandService _commandService;
        private readonly IIndividualTagDefinitionQueryService _queryService;
        public IndividualTagDefinitionDeleteHandler(
            IIndividualTagDefinitionCommandService commandService,
            IIndividualTagDefinitionQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var individualTagDefinition = await _queryService.Get(deleteDto.Id);
            if (individualTagDefinition == null)
            {
                throw new InvalidIdException();//todo : exception
            }
            await _commandService.Remove(individualTagDefinition);
        }
    }
}
