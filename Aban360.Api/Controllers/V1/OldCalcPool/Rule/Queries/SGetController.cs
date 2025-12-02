using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/s")]
    public class SGetController : BaseController
    {
        private readonly ISGetHandler _sGetHandler;
        public SGetController(ISGetHandler sGetHandler)
        {
            _sGetHandler = sGetHandler;
            _sGetHandler.NotNull(nameof(sGetHandler));
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
        {
            IEnumerable<SGetDto> result = await _sGetHandler.Handle(fromDateJalali, toDateJalali, cancellationToken);
            return Ok(result);
        }
    }
}
