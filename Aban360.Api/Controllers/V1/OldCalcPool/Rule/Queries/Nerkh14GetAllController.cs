using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/nerkh-14")]
    public class Nerkh14GetAllController : BaseController
    {
        private readonly INerkhGetAllHandler _nerkhGetAllHandler;
        public Nerkh14GetAllController(INerkhGetAllHandler nerkhGetAllHandler)
        {
            _nerkhGetAllHandler = nerkhGetAllHandler;
            _nerkhGetAllHandler.NotNull(nameof(nerkhGetAllHandler));
        }

        [HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NerkhGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NerkhGetDto> result = await _nerkhGetAllHandler.Handle(14, cancellationToken);
            return Ok(result);
        }
    }
}
