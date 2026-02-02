using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Tracking.Queries
{
    [Route("v1/tracking")]
    public class TrackingDisplayFlowController : BaseController
    {
        private readonly ITrackingFlowGetHandler _trackingFlowHandler;
        public TrackingDisplayFlowController(ITrackingFlowGetHandler trackingFlowHandler)
        {
            _trackingFlowHandler=trackingFlowHandler;
            _trackingFlowHandler.NotNull(nameof(trackingFlowHandler));
        }

        [HttpPost]
        [Route("display-flow")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayFlow([FromBody] SearchByIdInput input, CancellationToken cancellationToken)
        {
            ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto> result = await _trackingFlowHandler.Handle(input.Id, cancellationToken);
            return Ok(result);
        }
    }
}
