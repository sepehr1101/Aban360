using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class TrackingDetailGetByIdHandler : ITrackingDetailGetByIdHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        public TrackingDetailGetByIdHandler(ITrackingQueryService trackingQueryService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<TrackingOutputDto> Handle(Guid id, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(id);
            return trackingInfo;
        }
    }
}
