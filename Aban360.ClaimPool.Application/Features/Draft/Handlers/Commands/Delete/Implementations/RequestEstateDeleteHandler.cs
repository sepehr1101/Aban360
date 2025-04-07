using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestEstateDeleteHandler : IRequestEstateDeleteHandler
    {
        private readonly IRequestEstateCommandService _requestEstateCommandService;
        private readonly IRequestEstateQueryService _requestEstateQueryService;
        public RequestEstateDeleteHandler(
            IRequestEstateCommandService requestEstateCommandService,
            IRequestEstateQueryService requestEstateQueryService)
        {
            _requestEstateCommandService = requestEstateCommandService;
            _requestEstateCommandService.NotNull(nameof(_requestEstateCommandService));

            _requestEstateQueryService = requestEstateQueryService;
            _requestEstateQueryService.NotNull(nameof(_requestEstateQueryService));
        }

        public async Task Handle(EstateRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestEstate = await _requestEstateQueryService.Get(deleteDto.Id);
            _requestEstateCommandService.Remove(requestEstate);
        }
    }
}
