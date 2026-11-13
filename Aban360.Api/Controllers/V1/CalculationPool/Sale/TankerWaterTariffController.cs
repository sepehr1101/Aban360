using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale
{
    public class TankerWaterTariffController : BaseController
    {
        private readonly ITankerWaterSetPayHandler _c0_tankerSetPayHandler;
        private readonly ITankerWaterCalculationHandler _c1_calcHandler;
        public TankerWaterTariffController(ITankerWaterSetPayHandler c0_tankerSetPayHandler, ITankerWaterCalculationHandler c1_calcHandler)
        {
            _c0_tankerSetPayHandler = c0_tankerSetPayHandler;
            _c0_tankerSetPayHandler.NotNull(nameof(c0_tankerSetPayHandler));
            _c1_calcHandler = c1_calcHandler;
            _c1_calcHandler.NotNull(nameof(c1_calcHandler));
        }

        [HttpGet, HttpPost]
        [Route("~/v1/tanker-water-tariff/set-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterSetPayInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPay([FromBody] TankerWaterSetPayInputDto input, CancellationToken cancellationToken)
        {
            await _c0_tankerSetPayHandler.Handle(input, cancellationToken);
            return Ok(input);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/tanker-water-tariff/calc")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterCalculationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation([FromBody] TankerWaterCalculationInputDto input, CancellationToken cancellationToken)
        {
            TankerWaterCalculationOutputDto result = await _c1_calcHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
