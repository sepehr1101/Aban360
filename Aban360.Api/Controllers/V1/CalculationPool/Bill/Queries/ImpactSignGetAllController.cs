using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/impact-sign")]
    public class ImpactSignGetAllController : BaseController
    {
        private readonly IImpactSignGetAllHandler _impactSignGetAllHandler;
        public ImpactSignGetAllController(IImpactSignGetAllHandler impactSignGetAllHandler)
        {
            _impactSignGetAllHandler = impactSignGetAllHandler;
            _impactSignGetAllHandler.NotNull(nameof(impactSignGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ImpactSignGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ImpactSignGetDto> impactSigns = await _impactSignGetAllHandler.Handle(cancellationToken);
            return Ok(impactSigns);
        }
    }
}
