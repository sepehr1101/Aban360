using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/water-meter-change-number-history")]
    public class WaterMeterChangeNumberHistoryGetAllController : BaseController
    {
        private readonly IWaterMeterChangeNumberHistoryGetAllHandler _waterMeterChangeNumberHistoryGetAllHandler;
        public WaterMeterChangeNumberHistoryGetAllController(IWaterMeterChangeNumberHistoryGetAllHandler waterMeterChangeNumberHistoryGetAllHandler)
        {
            _waterMeterChangeNumberHistoryGetAllHandler = waterMeterChangeNumberHistoryGetAllHandler;
            _waterMeterChangeNumberHistoryGetAllHandler.NotNull(nameof(waterMeterChangeNumberHistoryGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterChangeNumberHistoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeterChangeNumberHistorys = await _waterMeterChangeNumberHistoryGetAllHandler.Handle(cancellationToken);
            return Ok(waterMeterChangeNumberHistorys);
        }
    }

}
