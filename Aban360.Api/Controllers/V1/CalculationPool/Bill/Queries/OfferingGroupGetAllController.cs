using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering-group")]
    public class OfferingGroupGetAllController : BaseController
    {
        private readonly IOfferingGroupGetAllHandler _offeringGroupGetAllHandler;
        public OfferingGroupGetAllController(
            IOfferingGroupGetAllHandler offeringGroupGetAllHandler)
        {
            _offeringGroupGetAllHandler = offeringGroupGetAllHandler;
            _offeringGroupGetAllHandler.NotNull(nameof(offeringGroupGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfferingGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var offeringGroups = await _offeringGroupGetAllHandler.Handle(cancellationToken);
            return Ok(offeringGroups);
        }
    }
}
