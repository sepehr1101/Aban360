using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Implementations
{
    public class IndividualEstateDeleteHandler : IIndividualEstateDeleteHandler
    {
        private readonly IIndividualEstateCommandService _commandService;
        private readonly IIndividualEstateQueryService _queryService;
        public IndividualEstateDeleteHandler(
            IIndividualEstateCommandService commandService,
            IIndividualEstateQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualEstateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var individualEstate = await _queryService.Get(deleteDto.Id);
            if (individualEstate == null)
            {
                throw new InvalidDataException();
            }
            await _commandService.Remove(individualEstate);
        }
    }
}
