using Aban360.Api.Filters;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Application.Features.Sale.Services;
using Aban360.Api.Controllers.V1.CalculationPool.Sale;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale
{
    [Route("v1/tanker-water-tariff")]
    public class TankerWaterTariffController : BaseController
    {
        private readonly ITankerWaterOperationsService _tankerWaterOperationsService;
        public TankerWaterTariffController(ITankerWaterOperationsService tankerWaterOperationsService)
        {
            _tankerWaterOperationsService = tankerWaterOperationsService;
            _tankerWaterOperationsService.NotNull(nameof(tankerWaterOperationsService));
        }

        [HttpGet, HttpPost]
        [Route("set-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterSetPayInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPay([FromBody] TankerWaterSetPayInputDto input, CancellationToken cancellationToken)
        {
            await _tankerWaterOperationsService.TankerWaterSetPayAsync(input, cancellationToken);
            return Ok(input);
        }

        [HttpGet, HttpPost]
        [Route("calc")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterCalculationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation([FromBody] TankerWaterCalculationInputDto input, CancellationToken cancellationToken)
        {
            TankerWaterCalculationOutputDto result = await _tankerWaterOperationsService.TankerWaterCalculationAsync(input, cancellationToken);
            return Ok(result);
        }
    }
}
