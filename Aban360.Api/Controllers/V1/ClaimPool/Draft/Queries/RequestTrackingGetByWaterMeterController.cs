using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Queries
{
    [Route("v1/request-tracking")]
    public class RequestTrackingGetByWaterMeterController : BaseController
    {
        private readonly IRequestTrackingGetByWaterMeterIdHandler _requestTrackingGetByWaterMeterHandler;
        public RequestTrackingGetByWaterMeterController(IRequestTrackingGetByWaterMeterIdHandler requestTrackingGetByWaterMeterHandler)
        {
            _requestTrackingGetByWaterMeterHandler = requestTrackingGetByWaterMeterHandler;
            _requestTrackingGetByWaterMeterHandler.NotNull(nameof(requestTrackingGetByWaterMeterHandler));
        }

        [HttpPost, HttpGet]
        [Route("by-water-meter/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByWaterMeter(int id, CancellationToken cancellationToken)
        {
            var RequestTrackings = await _requestTrackingGetByWaterMeterHandler.Handle(id, cancellationToken);
            return Ok(RequestTrackings);
        }
    }
}
