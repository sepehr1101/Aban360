using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/water-meter-change-number-history")]
    public class WaterMeterChangeNumberHistoryGetSingleController : BaseController
    {
        private readonly IWaterMeterChangeNumberHistoryGetSingleHandler _waterMeterChangeNumberHistoryGetSingleHandler;
        public WaterMeterChangeNumberHistoryGetSingleController(IWaterMeterChangeNumberHistoryGetSingleHandler waterMeterChangeNumberHistoryGetSingleHandler)
        {
            _waterMeterChangeNumberHistoryGetSingleHandler = waterMeterChangeNumberHistoryGetSingleHandler;
            _waterMeterChangeNumberHistoryGetSingleHandler.NotNull(nameof(waterMeterChangeNumberHistoryGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterChangeNumberHistoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(long id, CancellationToken cancellationToken)
        {
            var waterMeterChangeNumberHistorys = await _waterMeterChangeNumberHistoryGetSingleHandler.Handle(id, cancellationToken);
            return Ok(waterMeterChangeNumberHistorys);
        }
    }

}
