using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/impact-sign")]
    public class ImpactSignGetSingleController : BaseController
    {
        private readonly IImpactSignGetSingleHandler _impactSignGetSingleHandler;
        public ImpactSignGetSingleController(IImpactSignGetSingleHandler impactSignGetSingleHandler)
        {
            _impactSignGetSingleHandler = impactSignGetSingleHandler;
            _impactSignGetSingleHandler.NotNull(nameof(impactSignGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ImpactSignGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var impactSigns = await _impactSignGetSingleHandler.Handle(id, cancellationToken);
            return Ok(impactSigns);
        }
    }
}
