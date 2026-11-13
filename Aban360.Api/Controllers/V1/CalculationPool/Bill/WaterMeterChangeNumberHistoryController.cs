using Aban360.CalculationPool.Application.Features.Bill.Services;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    public class WaterMeterChangeNumberHistoryController : BaseController
    {
        private readonly IWaterMeterChangeNumberHistoryService _service;

        public WaterMeterChangeNumberHistoryController(IWaterMeterChangeNumberHistoryService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("~/v1/water-meter-change-number-history/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterChangeNumberHistoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeterChangeNumberHistorys = await _service.GetAllAsync(cancellationToken);
            return Ok(waterMeterChangeNumberHistorys);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/water-meter-change-number-history/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterChangeNumberHistoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(long id, CancellationToken cancellationToken)
        {
            var waterMeterChangeNumberHistorys = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(waterMeterChangeNumberHistorys);
        }
    }
}
