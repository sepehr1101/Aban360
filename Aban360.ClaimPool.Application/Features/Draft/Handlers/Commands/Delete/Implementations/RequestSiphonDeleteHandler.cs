using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestSiphonDeleteHandler : IRequestSiphonDeleteHandler
    {
        private readonly IRequestSiphonCommandService _requestSiphonCommandService;
        private readonly IRequestSiphonQueryService _requestSiphonQueryService;
        public RequestSiphonDeleteHandler(
            IRequestSiphonCommandService requestSiphonCommandService,
            IRequestSiphonQueryService requestSiphonQueryService)
        {
            _requestSiphonCommandService = requestSiphonCommandService;
            _requestSiphonCommandService.NotNull(nameof(_requestSiphonCommandService));

            _requestSiphonQueryService = requestSiphonQueryService;
            _requestSiphonQueryService.NotNull(nameof(_requestSiphonQueryService));
        }

        public async Task Handle(SiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestSiphon = await _requestSiphonQueryService.Get(deleteDto.Id);
            _requestSiphonCommandService.Remove(requestSiphon);
        }
    }
}
