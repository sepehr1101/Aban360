using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/tanker-water-distance-tariff")]
    public class TankerWaterDistanceTariffGetController : BaseController
    {
        private readonly ITankerWaterDistanceTariffGetHandler _getHandler;
        public TankerWaterDistanceTariffGetController(ITankerWaterDistanceTariffGetHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpPost,HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterDistanceTariffOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            TankerWaterDistanceTariffOutputDto result = await _getHandler.Handle(input, cancellationToken);

            return Ok(result);
        }
    }
}
