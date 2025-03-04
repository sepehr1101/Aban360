using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering-unit")]
    public class OfferingUnitGetSingleController : BaseController
    {       
        private readonly IOfferingUnitGetSingleHandler _offeringUnitGetSingleHandler;
        public OfferingUnitGetSingleController(
            IOfferingUnitGetSingleHandler offeringUnitGetSingleHandler)
        {
            _offeringUnitGetSingleHandler = offeringUnitGetSingleHandler;
            _offeringUnitGetSingleHandler.NotNull(nameof(offeringUnitGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingUnitGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var offeringUnits = await _offeringUnitGetSingleHandler.Handle(id, cancellationToken);
            return Ok(offeringUnits);
        }
    }
}
