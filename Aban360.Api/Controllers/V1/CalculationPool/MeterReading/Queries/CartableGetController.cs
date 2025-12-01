using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/cartable")]
    public class CartableGetController : BaseController
    {
        private readonly ICartableHandler _cartableGetHandler;
        public CartableGetController(ICartableHandler cartableGetHandler)
        {
            _cartableGetHandler = cartableGetHandler;
            _cartableGetHandler.NotNull(nameof(cartableGetHandler));
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterFlowCartableGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<MeterFlowCartableGetDto> result = await _cartableGetHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
