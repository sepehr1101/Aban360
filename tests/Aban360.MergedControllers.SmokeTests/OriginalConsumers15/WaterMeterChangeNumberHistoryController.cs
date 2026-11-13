// Frozen consumer before service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace BeforeServiceConsumers15.Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    public class WaterMeterChangeNumberHistoryController : BaseController
    {
        private readonly IWaterMeterChangeNumberHistoryGetAllHandler _c0_waterMeterChangeNumberHistoryGetAllHandler;
        private readonly IWaterMeterChangeNumberHistoryGetSingleHandler _c1_waterMeterChangeNumberHistoryGetSingleHandler;
        public WaterMeterChangeNumberHistoryController(IWaterMeterChangeNumberHistoryGetAllHandler c0_waterMeterChangeNumberHistoryGetAllHandler, IWaterMeterChangeNumberHistoryGetSingleHandler c1_waterMeterChangeNumberHistoryGetSingleHandler)
        {
            _c0_waterMeterChangeNumberHistoryGetAllHandler = c0_waterMeterChangeNumberHistoryGetAllHandler;
            _c0_waterMeterChangeNumberHistoryGetAllHandler.NotNull(nameof(c0_waterMeterChangeNumberHistoryGetAllHandler));
            _c1_waterMeterChangeNumberHistoryGetSingleHandler = c1_waterMeterChangeNumberHistoryGetSingleHandler;
            _c1_waterMeterChangeNumberHistoryGetSingleHandler.NotNull(nameof(c1_waterMeterChangeNumberHistoryGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("~/v1/water-meter-change-number-history/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterChangeNumberHistoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeterChangeNumberHistorys = await _c0_waterMeterChangeNumberHistoryGetAllHandler.Handle(cancellationToken);
            return Ok(waterMeterChangeNumberHistorys);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/water-meter-change-number-history/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterChangeNumberHistoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(long id, CancellationToken cancellationToken)
        {
            var waterMeterChangeNumberHistorys = await _c1_waterMeterChangeNumberHistoryGetSingleHandler.Handle(id, cancellationToken);
            return Ok(waterMeterChangeNumberHistorys);
        }
    }
}
