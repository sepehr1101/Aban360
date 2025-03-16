using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering-unit")]
    public class OfferingUnitGetAllController : BaseController
    {       
        private readonly IOfferingUnitGetAllHandler _offeringUnitGetAllHandler;
        public OfferingUnitGetAllController(
            IOfferingUnitGetAllHandler offeringUnitGetAllHandler)
        {
            _offeringUnitGetAllHandler = offeringUnitGetAllHandler;
            _offeringUnitGetAllHandler.NotNull(nameof(offeringUnitGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfferingUnitGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<OfferingUnitGetDto> offeringUnits = await _offeringUnitGetAllHandler.Handle(cancellationToken);
            return Ok(offeringUnits);
        }
    }
}
