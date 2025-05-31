using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Queries
{
    [Route("v1/request-tracking")]
    public class RequestTrackingGetAllController : BaseController
    {
        private readonly IRequestTrackingGetAllHandler _requestTrackingGetAllHandler;
        public RequestTrackingGetAllController(IRequestTrackingGetAllHandler requestTrackingGetAllHandler)
        {
            _requestTrackingGetAllHandler = requestTrackingGetAllHandler;
            _requestTrackingGetAllHandler.NotNull(nameof(requestTrackingGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RequestTrackingGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var RequestTrackings = await _requestTrackingGetAllHandler.Handle(cancellationToken);
            return Ok(RequestTrackings);
        }
    }
}
