using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestTrackingDeleteHandler : IRequestTrackingDeleteHandler
    {
        private readonly IRequestTrackingCommandService _requestTrackingCommandService;
        private readonly IRequestTrackingQueryService _requestTrackingQueryService;
        public RequestTrackingDeleteHandler(
            IRequestTrackingCommandService requestTrackingCommandService,
            IRequestTrackingQueryService requestTrackingQueryService)
        {
            _requestTrackingCommandService = requestTrackingCommandService;
            _requestTrackingCommandService.NotNull(nameof(_requestTrackingCommandService));

            _requestTrackingQueryService = requestTrackingQueryService;
            _requestTrackingQueryService.NotNull(nameof(_requestTrackingQueryService));
        }

        public async Task Handle( RequestTrackingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestTracking = await _requestTrackingQueryService.Get(deleteDto.Id);
            requestTracking.RemoveLogInfo = "--";

            await _requestTrackingCommandService.Remove(requestTracking);
        }
    }
}
