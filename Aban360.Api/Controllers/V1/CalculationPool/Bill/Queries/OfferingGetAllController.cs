using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering")]
    public class OfferingGetAllController : BaseController
    {       
        private readonly IOfferingGetAllHandler _offeringGetAllHandler;
        public OfferingGetAllController(
            IOfferingGetAllHandler offeringGetAllHandler)
        {
            _offeringGetAllHandler = offeringGetAllHandler;
            _offeringGetAllHandler.NotNull(nameof(offeringGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfferingGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<OfferingGetDto> offerings = await _offeringGetAllHandler.Handle(cancellationToken);
            return Ok(offerings);
        }
    }
}