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

namespace Aban360.BrdigeApi.Controllers.V1
{
    public class WaterController : BaseController
    {
        private readonly ITankerWaterCalculationHandler _c0_calcHandler;
        private readonly ITankerWaterSetPayHandler _c1_tankerSetPayHandler;
        private readonly ISubscriptionEventHandler _c2_subscriptionEventHandler;
        public WaterController(ITankerWaterCalculationHandler c0_calcHandler, ITankerWaterSetPayHandler c1_tankerSetPayHandler, ISubscriptionEventHandler c2_subscriptionEventHandler)
        {
            _c0_calcHandler = c0_calcHandler;
            _c0_calcHandler.NotNull(nameof(c0_calcHandler));
            _c1_tankerSetPayHandler = c1_tankerSetPayHandler;
            _c1_tankerSetPayHandler.NotNull(nameof(c1_tankerSetPayHandler));
            _c2_subscriptionEventHandler = c2_subscriptionEventHandler;
            _c2_subscriptionEventHandler.NotNull(nameof(c2_subscriptionEventHandler));
        }

        [HttpGet, HttpPost]
        [Route("~/v1/water/tanker")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterCalculationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation([FromBody] TankerWaterCalculationInputDto input, CancellationToken cancellationToken)
        {
            TankerWaterCalculationOutputDto result = await _c0_calcHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/water/tanker-set-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterSetPayInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPay([FromBody] TankerWaterSetPayInputDto input, CancellationToken cancellationToken)
        {
            await _c1_tankerSetPayHandler.Handle(input, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("~/v1/water/events-summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventsSummaryInfo([FromBody] SubscriptionCardexInput searchInput, CancellationToken cancellationToken)
        {
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> items = await _c2_subscriptionEventHandler.Handle(searchInput, cancellationToken);
            return Ok(items);
        }
    }
}
