using Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Sms.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class SmsByQueueIdGetHandler : ISmsByQueueIdGetHandler
    {
        private readonly IQueueQueryService _smsQueryService;
        public SmsByQueueIdGetHandler(IQueueQueryService smsQueryService)
        {
            _smsQueryService = smsQueryService;
            _smsQueryService.NotNull(nameof(smsQueryService));
        }

        public async Task< TrackingSmsDataOutputDto> Handle(Guid id, CancellationToken cancellationToken)
        {
            return await _smsQueryService.Get(id);
        }
    }
}
