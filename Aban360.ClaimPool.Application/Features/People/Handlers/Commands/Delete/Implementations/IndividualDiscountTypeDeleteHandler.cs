using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Implementations
{
    internal sealed class IndividualDiscountTypeDeleteHandler : IIndividualDiscountTypeDeleteHandler
    {
        private readonly IIndividualDiscountTypeCommandService _individualDiscountTypeCommandService;
        private readonly IIndividualDiscountTypeQueryService _individualDiscountTypeQueryService;
        public IndividualDiscountTypeDeleteHandler(
            IIndividualDiscountTypeCommandService individualDiscountTypeCommandService,
            IIndividualDiscountTypeQueryService individualDiscountTypeQueryService)
        {
            _individualDiscountTypeCommandService = individualDiscountTypeCommandService;
            _individualDiscountTypeCommandService.NotNull(nameof(_individualDiscountTypeCommandService));

            _individualDiscountTypeQueryService = individualDiscountTypeQueryService;
            _individualDiscountTypeQueryService.NotNull(nameof(_individualDiscountTypeQueryService));
        }

        public async Task Handle(IndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var individualDiscountType = await _individualDiscountTypeQueryService.Get(deleteDto.Id);
            await _individualDiscountTypeCommandService.Remove(individualDiscountType);
        }
    }
}
