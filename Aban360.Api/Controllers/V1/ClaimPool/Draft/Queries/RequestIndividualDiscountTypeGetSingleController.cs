using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Queries
{
    [Route("v1/request-individual-discount-type")]
    public class RequestIndividualDiscountTypeGetSingleController : BaseController
    {
        private readonly IRequestIndividualDiscountTypeGetSingleHandler _requestIndividualDiscountTypeGetSingleHandler;
        public RequestIndividualDiscountTypeGetSingleController(IRequestIndividualDiscountTypeGetSingleHandler requestIndividualDiscountTypeGetSingleHandler)
        {
            _requestIndividualDiscountTypeGetSingleHandler = requestIndividualDiscountTypeGetSingleHandler;
            _requestIndividualDiscountTypeGetSingleHandler.NotNull(nameof(requestIndividualDiscountTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIndividualDiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var requestIndividualDiscountTypes = await _requestIndividualDiscountTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(requestIndividualDiscountTypes);
        }
    }
}
