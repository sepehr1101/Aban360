using Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Sms.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class SmsByTrackIdGetHandler : ISmsByTrackIdGetHandler
    {
        private readonly ISmsQueryService _smsQueryService;
        public SmsByTrackIdGetHandler(ISmsQueryService smsQueryService)
        {
            _smsQueryService = smsQueryService;
            _smsQueryService.NotNull(nameof(smsQueryService));
        }

        public async Task<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>> Handle(Guid trackId, CancellationToken cancellationToken)
        {
            return await _smsQueryService.Get(trackId);
        }
    }
}
