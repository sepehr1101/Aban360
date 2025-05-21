using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestIndividualDeleteHandler : IRequestIndividualDeleteHandler
    {
        private readonly IRequestIndividualCommandService _requestIndividualCommandService;
        private readonly IRequestIndividualQueryService _requestIndividualQueryService;
        private readonly IRequestIndividualEstateQueryService _requestIndividualEstateQueryService;
        private readonly IRequestIndividualEstateCommandService _requestIndividualEstateCommandService;
        private readonly IRequestIndividualTagCommandService _requestIndividualTagCommandService;
        private readonly IRequestIndividualTagQueryService _requestIndividualTagQueryService;
        public RequestIndividualDeleteHandler(
            IRequestIndividualCommandService requestIndividualCommandService,
            IRequestIndividualQueryService requestIndividualQueryService,
            IRequestIndividualEstateQueryService requestIndividualEstateQueryService,
            IRequestIndividualEstateCommandService requestIndividualEstateCommandService,
            IRequestIndividualTagCommandService requestIndividualTagCommandService,
            IRequestIndividualTagQueryService requestIndividualTagQueryService)
        {
            _requestIndividualCommandService = requestIndividualCommandService;
            _requestIndividualCommandService.NotNull(nameof(_requestIndividualCommandService));

            _requestIndividualQueryService = requestIndividualQueryService;
            _requestIndividualQueryService.NotNull(nameof(_requestIndividualQueryService));

            _requestIndividualEstateQueryService = requestIndividualEstateQueryService;
            _requestIndividualEstateQueryService.NotNull(nameof(_requestIndividualEstateQueryService));

            _requestIndividualEstateCommandService = requestIndividualEstateCommandService;
            _requestIndividualEstateCommandService.NotNull(nameof(_requestIndividualEstateCommandService));
            
            _requestIndividualTagCommandService= requestIndividualTagCommandService;
            _requestIndividualTagCommandService.NotNull(nameof(_requestIndividualTagCommandService));

            _requestIndividualTagQueryService= requestIndividualTagQueryService;
            _requestIndividualTagQueryService.NotNull(nameof(_requestIndividualTagQueryService));
        }

        public async Task Handle(IndividualRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestIndividual = await _requestIndividualQueryService.Get(deleteDto.Id);
            _requestIndividualCommandService.Remove(requestIndividual);
        }
    }
}
