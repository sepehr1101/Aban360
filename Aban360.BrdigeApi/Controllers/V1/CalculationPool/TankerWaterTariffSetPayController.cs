using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/water")]
    public class TankerWaterTariffSetPayController : BaseController
    {
        private readonly ITankerWaterSetPayHandler _tankerSetPayHandler;
        public TankerWaterTariffSetPayController(ITankerWaterSetPayHandler tankerSetPayHandler)
        {
            _tankerSetPayHandler = tankerSetPayHandler;
            _tankerSetPayHandler.NotNull(nameof(tankerSetPayHandler));
        }

        [HttpGet, HttpPost]
        [Route("tanker-set-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterSetPayInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPay([FromBody] TankerWaterSetPayInputDto input, CancellationToken cancellationToken)
        {
            await _tankerSetPayHandler.Handle(input, cancellationToken);
            return Ok(input);
        }
    }
}
