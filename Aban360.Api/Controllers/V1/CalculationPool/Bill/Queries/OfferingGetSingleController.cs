using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering")]
    public class OfferingGetSingleController : BaseController
    {      
        private readonly IOfferingGetSingleHandler _offeringGetSingleHandler;
        public OfferingGetSingleController(
            IOfferingGetSingleHandler offeringGetSingleHandler)
        {
            _offeringGetSingleHandler = offeringGetSingleHandler;
            _offeringGetSingleHandler.NotNull(nameof(offeringGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var offerings = await _offeringGetSingleHandler.Handle(id, cancellationToken);
            return Ok(offerings);
        }
    }
}
