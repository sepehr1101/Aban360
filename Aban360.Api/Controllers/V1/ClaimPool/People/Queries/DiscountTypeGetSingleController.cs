using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/discount-type")]
    public class DiscountTypeGetSingleController : BaseController
    {
        private readonly IDiscountTypeGetSingleHandler _discountTypeGetSingleHandler;
        public DiscountTypeGetSingleController(IDiscountTypeGetSingleHandler discountTypeGetSingleHandler)
        {
            _discountTypeGetSingleHandler = discountTypeGetSingleHandler;
            _discountTypeGetSingleHandler.NotNull(nameof(discountTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(DiscountTypeEnum id, CancellationToken cancellationToken)
        {
            var discountTypes = await _discountTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(discountTypes);
        }
    }
}
