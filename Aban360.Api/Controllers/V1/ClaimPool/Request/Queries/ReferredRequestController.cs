using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class ReferredRequestController : BaseController
    {
        private readonly IReferredRequestGetHandler _referredRequestGetHandler;
        public ReferredRequestController(IReferredRequestGetHandler referredRequestGetHandler)
        {
            _referredRequestGetHandler = referredRequestGetHandler;
            _referredRequestGetHandler.NotNull(nameof(referredRequestGetHandler));
        }

        [HttpPost, HttpGet]
        [Route("referred")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReferred(CancellationToken cancellationToken)
        {
            ReportOutput<TrackingKartableHeaderOutputDto, TrackingOutputDto> result = await _referredRequestGetHandler.Handle(CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
