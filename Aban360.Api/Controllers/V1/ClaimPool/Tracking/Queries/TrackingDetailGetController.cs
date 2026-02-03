using Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Tracking.Queries
{
    [Route("v1/tracking")]
    public class TrackingDetailGetController : BaseController
    {
        private readonly IRequestIsRegisteredDetailHandler _requestIsRegisteredHandler;
        public TrackingDetailGetController(IRequestIsRegisteredDetailHandler requestIsRegisteredHandler)
        {
            _requestIsRegisteredHandler = requestIsRegisteredHandler;
            _requestIsRegisteredHandler.NotNull(nameof(requestIsRegisteredHandler));
        }

        [HttpPost]
        [Route("display-detail")]
        public async Task<IActionResult> Detail([FromBody] TrackingDetailInputDto input, CancellationToken cancellationToken)
        {
            TrackingDetailGetDto TrackDetailInput = GetTrackDetail(input);
            switch (input.StateId)
            {
                case 0:
                    {
                        RequestIsRegisterdOutputDto result = await _requestIsRegisteredHandler.Handle(TrackDetailInput, cancellationToken);
                        return Ok(result);
                    }
                default: throw new InvalidCastException();
            }
        }
        private TrackingDetailGetDto GetTrackDetail(TrackingDetailInputDto input)
        {
            return new TrackingDetailGetDto(input.ZoneId, input.TrackNumber);
        }
    }
}
