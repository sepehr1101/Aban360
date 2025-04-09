using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestIndividualDeleteHandler : IRequestIndividualDeleteHandler
    {
        private readonly IRequestIndividualCommandService _requestIndividualCommandService;
        private readonly IRequestIndividualQueryService _requestIndividualQueryService;
        public RequestIndividualDeleteHandler(
            IRequestIndividualCommandService requestIndividualCommandService,
            IRequestIndividualQueryService requestIndividualQueryService)
        {
            _requestIndividualCommandService = requestIndividualCommandService;
            _requestIndividualCommandService.NotNull(nameof(_requestIndividualCommandService));

            _requestIndividualQueryService = requestIndividualQueryService;
            _requestIndividualQueryService.NotNull(nameof(_requestIndividualQueryService));
        }

        public async Task Handle(IndividualRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestIndividual = await _requestIndividualQueryService.Get(deleteDto.Id);
            _requestIndividualCommandService.Remove(requestIndividual);
        }
    }
}
