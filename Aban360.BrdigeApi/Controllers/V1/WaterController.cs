using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.BrdigeApi.Controllers.V1.CalculationPool;
using Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo;
using Aban360.CalculationPool.Application.Features.Sale.Services;

namespace Aban360.BrdigeApi.Controllers.V1
{
    [Route("v1/water")]
    public class WaterController : BaseController
    {
        private readonly ITankerWaterOperationsService _tankerWaterOperationsService;
        private readonly ISubscriptionEventHandler _c2_subscriptionEventHandler;
        public WaterController(ITankerWaterOperationsService tankerWaterOperationsService, ISubscriptionEventHandler c2_subscriptionEventHandler)
        {
            _tankerWaterOperationsService = tankerWaterOperationsService;
            _tankerWaterOperationsService.NotNull(nameof(tankerWaterOperationsService));
            _c2_subscriptionEventHandler = c2_subscriptionEventHandler;
            _c2_subscriptionEventHandler.NotNull(nameof(c2_subscriptionEventHandler));
        }

        [HttpGet, HttpPost]
        [Route("tanker")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterCalculationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation([FromBody] TankerWaterCalculationInputDto input, CancellationToken cancellationToken)
        {
            TankerWaterCalculationOutputDto result = await _tankerWaterOperationsService.TankerWaterCalculationAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("tanker-set-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterSetPayInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPay([FromBody] TankerWaterSetPayInputDto input, CancellationToken cancellationToken)
        {
            await _tankerWaterOperationsService.TankerWaterSetPayAsync(input, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("events-summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventsSummaryInfo([FromBody] SubscriptionCardexInput searchInput, CancellationToken cancellationToken)
        {
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> items = await _c2_subscriptionEventHandler.Handle(searchInput, cancellationToken);
            return Ok(items);
        }
    }
}
