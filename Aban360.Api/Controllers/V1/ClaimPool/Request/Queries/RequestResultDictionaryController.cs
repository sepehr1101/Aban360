using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class RequestResultDictionaryController : BaseController
    {
        private readonly ITrackingResultGetAllHandler _trackingResultGetAllHandler;
        public RequestResultDictionaryController(ITrackingResultGetAllHandler trackingResultGetAllHandler)
        {
            _trackingResultGetAllHandler = trackingResultGetAllHandler;
            _trackingResultGetAllHandler.NotNull(nameof(trackingResultGetAllHandler));
        }

        [HttpGet]
        [Route("result-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _trackingResultGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
