using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class RequestResultDictionaryController : BaseController
    {
        private readonly IRequestDictionariesService _trackingResultGetAllHandler;
        public RequestResultDictionaryController(IRequestDictionariesService trackingResultGetAllHandler)
        {
            _trackingResultGetAllHandler = trackingResultGetAllHandler;
            _trackingResultGetAllHandler.NotNull(nameof(trackingResultGetAllHandler));
        }

        [HttpGet]
        [Route("result-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _trackingResultGetAllHandler.TrackingResultGetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
