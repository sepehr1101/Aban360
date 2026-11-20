using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class ReferredRequestController : BaseController
    {
        private readonly IRequestCartableService _referredRequestGetHandler;
        public ReferredRequestController(IRequestCartableService referredRequestGetHandler)
        {
            _referredRequestGetHandler = referredRequestGetHandler;
            _referredRequestGetHandler.NotNull(nameof(referredRequestGetHandler));
        }

        [HttpPost, HttpGet]
        [Route("referred")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReferred(CancellationToken cancellationToken)
        {
            ReportOutput<TrackingKartableHeaderOutputDto, TrackingOutputDto> result = await _referredRequestGetHandler.ReferredRequestGetAsync(CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
