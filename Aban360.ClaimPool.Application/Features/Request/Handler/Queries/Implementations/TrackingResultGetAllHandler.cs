using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class TrackingResultGetAllHandler : ITrackingResultGetAllHandler
    {
        private readonly IT64QueryService _trackingResultQueryService;
        public TrackingResultGetAllHandler(IT64QueryService trackingResultQueryService)
        {
            _trackingResultQueryService = trackingResultQueryService;
            _trackingResultQueryService.NotNull(nameof(trackingResultQueryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> trackingResultsDictionary = await _trackingResultQueryService.Get();
            return trackingResultsDictionary;
        }
    }
}
