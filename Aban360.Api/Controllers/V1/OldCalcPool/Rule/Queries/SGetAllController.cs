using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/s")]
    public class SGetAllController : BaseController
    {
        private readonly ISGetAllHandler _sGetAllHandler;
        public SGetAllController(ISGetAllHandler sGetAllHandler)
        {
            _sGetAllHandler = sGetAllHandler;
            _sGetAllHandler.NotNull(nameof(sGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<SGetDto> result = await _sGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
