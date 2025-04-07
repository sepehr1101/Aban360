using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestIndividualEstateDeleteHandler : IRequestIndividualEstateDeleteHandler
    {
        private readonly IRequestIndividualEstateCommandService _requestIndividualEstateCommandService;
        private readonly IRequestIndividualEstateQueryService _requestIndividualEstateQueryService;
        public RequestIndividualEstateDeleteHandler(
            IRequestIndividualEstateCommandService requestIndividualEstateCommandService,
            IRequestIndividualEstateQueryService requestIndividualEstateQueryService)
        {
            _requestIndividualEstateCommandService = requestIndividualEstateCommandService;
            _requestIndividualEstateCommandService.NotNull(nameof(_requestIndividualEstateCommandService));

            _requestIndividualEstateQueryService = requestIndividualEstateQueryService;
            _requestIndividualEstateQueryService.NotNull(nameof(_requestIndividualEstateQueryService));
        }

        public async Task Handle(IndividualEstateRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestIndividualEstate = await _requestIndividualEstateQueryService.Get(deleteDto.Id);
            _requestIndividualEstateCommandService.Remove(requestIndividualEstate);
        }
    }
}
