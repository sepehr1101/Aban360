using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/discount-type")]
    public class DiscountTypeGetAllController : BaseController
    {
        private readonly IDiscountTypeGetAllHandler _discountTypeGetAllHandler;
        public DiscountTypeGetAllController(IDiscountTypeGetAllHandler discountTypeGetAllHandler)
        {
            _discountTypeGetAllHandler = discountTypeGetAllHandler;
            _discountTypeGetAllHandler.NotNull(nameof(discountTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var discountTypes = await _discountTypeGetAllHandler.Handle( cancellationToken);
            return Ok(discountTypes);
        }
    }
}
