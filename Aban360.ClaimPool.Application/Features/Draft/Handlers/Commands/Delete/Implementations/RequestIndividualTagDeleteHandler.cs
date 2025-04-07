using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestIndividualTagDeleteHandler : IRequestIndividualTagDeleteHandler
    {
        private readonly IRequestIndividualTagCommandService _requestIndividualTagCommandService;
        private readonly IRequestIndividualTagQueryService _requestIndividualTagQueryService;
        public RequestIndividualTagDeleteHandler(
            IRequestIndividualTagCommandService requestIndividualTagCommandService,
            IRequestIndividualTagQueryService requestIndividualTagQueryService)
        {
            _requestIndividualTagCommandService = requestIndividualTagCommandService;
            _requestIndividualTagCommandService.NotNull(nameof(_requestIndividualTagCommandService));

            _requestIndividualTagQueryService = requestIndividualTagQueryService;
            _requestIndividualTagQueryService.NotNull(nameof(_requestIndividualTagQueryService));
        }

        public async Task Handle(IndividualTagRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestIndividualTag = await _requestIndividualTagQueryService.Get(deleteDto.Id);
            _requestIndividualTagCommandService.Remove(requestIndividualTag);
        }
    }
}
