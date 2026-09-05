using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
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
        private readonly ICartableByZoneIdGetHandler _cartableByZoneIdGetHandler;
        public CartableGetController(
            ICartableHandler cartableGetHandler,
            ICartableByZoneIdGetHandler cartableByZoneIdGetHandler)
        {
            _cartableGetHandler = cartableGetHandler;
            _cartableGetHandler.NotNull(nameof(cartableGetHandler));

            _cartableByZoneIdGetHandler = cartableByZoneIdGetHandler;
            _cartableByZoneIdGetHandler.NotNull(nameof(cartableByZoneIdGetHandler));
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterFlowCartableGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<MeterFlowCartableGetDto> result = await _cartableGetHandler.Handle(CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get-completed")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterFlowCartableGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompleted([FromBody] MeterFlowByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<MeterFlowCartableGetDto> result = await _cartableByZoneIdGetHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
