using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/nerkh-12")]
    public class Nerkh12GetController : BaseController
    {
        private readonly INerkhGetHandler _nerkhGetHandler;
        public Nerkh12GetController(INerkhGetHandler nerkhGetHandler)
        {
            _nerkhGetHandler = nerkhGetHandler;
            _nerkhGetHandler.NotNull(nameof(nerkhGetHandler));
        }

        [HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NerkhGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            IEnumerable<NerkhGetDto> result = await _nerkhGetHandler.Handle(id, 12, cancellationToken);
            return Ok(result);
        }
    }
}
