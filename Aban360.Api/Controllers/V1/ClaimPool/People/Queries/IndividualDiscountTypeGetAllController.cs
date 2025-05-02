using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual-discount-type")]
    public class IndividualDiscountTypeGetAllController : BaseController
    {
        private readonly IIndividualDiscountTypeGetAllHandler _individualDiscountTypeGetAllHandler;
        public IndividualDiscountTypeGetAllController(IIndividualDiscountTypeGetAllHandler individualDiscountTypeGetAllHandler)
        {
            _individualDiscountTypeGetAllHandler = individualDiscountTypeGetAllHandler;
            _individualDiscountTypeGetAllHandler.NotNull(nameof(individualDiscountTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualDiscountTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var individualDiscountTypes = await _individualDiscountTypeGetAllHandler.Handle(cancellationToken);
            return Ok(individualDiscountTypes);
        }
    }
}
