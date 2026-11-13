// Frozen consumer before service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace BeforeServiceConsumers15.Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/discount-type")]
    public class DiscountTypeController : BaseController
    {
        private readonly IDiscountTypeGetAllHandler _discountTypeGetAllHandler;
        private readonly IDiscountTypeGetSingleHandler _discountTypeGetSingleHandler;

        public DiscountTypeController(
            IDiscountTypeGetAllHandler discountTypeGetAllHandler,
            IDiscountTypeGetSingleHandler discountTypeGetSingleHandler)
        {
            _discountTypeGetAllHandler = discountTypeGetAllHandler;
            _discountTypeGetAllHandler.NotNull(nameof(discountTypeGetAllHandler));
            _discountTypeGetSingleHandler = discountTypeGetSingleHandler;
            _discountTypeGetSingleHandler.NotNull(nameof(discountTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([Optional] bool isReturn, CancellationToken cancellationToken)
        {
            var discountTypes = await _discountTypeGetAllHandler.Handle(isReturn, cancellationToken);
            return Ok(discountTypes);
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
