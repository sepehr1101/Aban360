using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Implementations
{
    public class IndividualTagDeleteHandler : IIndividualTagDeleteHandler
    {
        private readonly IIndividualTagCommandService _individualTagCommandService;
        private readonly IIndividualTagQueryService _IndividualTagQueryService;
        public IndividualTagDeleteHandler(
            IIndividualTagCommandService IndividualTagCommandService,
            IIndividualTagQueryService IndividualTagQueryService)
        {
            _individualTagCommandService = IndividualTagCommandService;
            _individualTagCommandService.NotNull(nameof(IndividualTagCommandService));

            _IndividualTagQueryService = IndividualTagQueryService;
            _IndividualTagQueryService.NotNull(nameof(IndividualTagQueryService));
        }

        public async Task Handle(IndividualTagDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var IndividualTag = await _IndividualTagQueryService.Get(deleteDto.Id);
            if (IndividualTag == null)
            {
                throw new InvalidIdException();
            }
            await _individualTagCommandService.Remove(IndividualTag);
        }
    }
}
