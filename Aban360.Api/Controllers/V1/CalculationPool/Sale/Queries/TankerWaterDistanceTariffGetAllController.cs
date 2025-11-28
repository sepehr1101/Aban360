using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/tanker-water-distance-tariff")]
    public class TankerWaterDistanceTariffGetAllController : BaseController
    {
        private readonly ITankerWaterDistanceTariffGetAllHandler _getHandler;
        public TankerWaterDistanceTariffGetAllController(ITankerWaterDistanceTariffGetAllHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TankerWaterDistanceTariffOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<TankerWaterDistanceTariffOutputDto> result = await _getHandler.Handle(cancellationToken);

            return Ok(result);
        }
    }
}
