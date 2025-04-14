using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Queries
{
    [Route("v1/request-individual-discount-type")]
    public class RequestIndividualDiscountTypeGetAllController : BaseController
    {
        private readonly IRequestIndividualDiscountTypeGetAllHandler _requestIndividualDiscountTypeGetAllHandler;
        public RequestIndividualDiscountTypeGetAllController(IRequestIndividualDiscountTypeGetAllHandler requestIndividualDiscountTypeGetAllHandler)
        {
            _requestIndividualDiscountTypeGetAllHandler = requestIndividualDiscountTypeGetAllHandler;
            _requestIndividualDiscountTypeGetAllHandler.NotNull(nameof(requestIndividualDiscountTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RequestIndividualDiscountTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var requestIndividualDiscountTypes = await _requestIndividualDiscountTypeGetAllHandler.Handle(cancellationToken);
            return Ok(requestIndividualDiscountTypes);
        }
    }
}
