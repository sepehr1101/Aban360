using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Implementations
{
    internal sealed class IndividualEstateRelationTypeDeleteHandler : IIndividualEstateRelationTypeDeleteHandler
    {
        private readonly IIndividualEstateRelationTypeCommandService _commandService;
        private readonly IIndividualEstateRelationTypeQueryService _queryService;
        public IndividualEstateRelationTypeDeleteHandler(
            IIndividualEstateRelationTypeCommandService commandService,
            IIndividualEstateRelationTypeQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualEstateRelationTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            IndividualEstateRelationType individualEstateRelationType = await _queryService.Get(deleteDto.Id);
            await _commandService.Remove(individualEstateRelationType);
        }
    }
}
