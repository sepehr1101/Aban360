using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/intividual-discount-type")]
    public class IndividualDiscountTypeGetSingleController : BaseController
    {
        private readonly IIndividualDiscountTypeGetSingleHandler _individualDiscountTypeGetSingleHandler;
        public IndividualDiscountTypeGetSingleController(IIndividualDiscountTypeGetSingleHandler individualDiscountTypeGetSingleHandler)
        {
            _individualDiscountTypeGetSingleHandler = individualDiscountTypeGetSingleHandler;
            _individualDiscountTypeGetSingleHandler.NotNull(nameof(individualDiscountTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var individualDiscountTypes = await _individualDiscountTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(individualDiscountTypes);
        }
    }
}
