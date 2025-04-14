using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestIndividualDiscountTypeDeleteHandler : IRequestIndividualDiscountTypeDeleteHandler
    {
        private readonly IRequestIndividualDiscountTypeCommandService _requestIndividualDiscountTypeCommandService;
        private readonly IRequestIndividualDiscountTypeQueryService _requestIndividualDiscountTypeQueryService;
        public RequestIndividualDiscountTypeDeleteHandler(
            IRequestIndividualDiscountTypeCommandService requestIndividualDiscountTypeCommandService,
            IRequestIndividualDiscountTypeQueryService requestIndividualDiscountTypeQueryService)
        {
            _requestIndividualDiscountTypeCommandService = requestIndividualDiscountTypeCommandService;
            _requestIndividualDiscountTypeCommandService.NotNull(nameof(_requestIndividualDiscountTypeCommandService));

            _requestIndividualDiscountTypeQueryService = requestIndividualDiscountTypeQueryService;
            _requestIndividualDiscountTypeQueryService.NotNull(nameof(_requestIndividualDiscountTypeQueryService));
        }

        public async Task Handle(RequestIndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var individualDiscountType = await _requestIndividualDiscountTypeQueryService.Get(deleteDto.Id);
            await _requestIndividualDiscountTypeCommandService.Remove(individualDiscountType);
        }
    }
}
