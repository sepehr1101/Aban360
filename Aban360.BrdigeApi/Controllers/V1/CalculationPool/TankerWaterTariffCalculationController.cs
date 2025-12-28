using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/water")]
    public class TankerWaterTariffCalculationController : BaseController
    {
        private readonly ITankerWaterCalculationHandler _calcHandler;
        public TankerWaterTariffCalculationController(ITankerWaterCalculationHandler calcHandler)
        {
            _calcHandler = calcHandler;
            _calcHandler.NotNull(nameof(calcHandler));
        }

        [HttpGet, HttpPost]
        [Route("tanker")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterCalculationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation([FromBody] TankerWaterCalculationInputDto input, CancellationToken cancellationToken)
        {
            TankerWaterCalculationOutputDto result = await _calcHandler.Handle(input, cancellationToken);

            return Ok(result);
        }
    }
}
