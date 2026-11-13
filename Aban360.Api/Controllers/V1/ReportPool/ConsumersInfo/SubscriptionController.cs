using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionEventHandler _c0_subscriptionEventHandler;
        private readonly ISubscriptionSummaryInfoGetHandler _c1_consumerSummeryHandler;
        public SubscriptionController(ISubscriptionEventHandler c0_subscriptionEventHandler, ISubscriptionSummaryInfoGetHandler c1_consumerSummaryHandler)
        {
            _c0_subscriptionEventHandler = c0_subscriptionEventHandler;
            _c0_subscriptionEventHandler.NotNull(nameof(c0_subscriptionEventHandler));
            _c1_consumerSummeryHandler = c1_consumerSummaryHandler;
            _c1_consumerSummeryHandler.NotNull(nameof(_c1_consumerSummeryHandler));
        }

        [HttpPost]
        [Route("~/v1/subscription/events-summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventsSummaryInfo([FromBody] SubscriptionCardexInput searchInput, CancellationToken cancellationToken)
        {
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> items = await _c0_subscriptionEventHandler.Handle(searchInput, cancellationToken);
            return Ok(items);
        }

        [HttpPost]
        [Route("~/v1/subscription/sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport([FromBody] SubscriptionCardexInput searchInput, CancellationToken cancellationToken)
        {
            int reportCode = 230;
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> calculationDetails = await _c0_subscriptionEventHandler.Handle(searchInput, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode, true);
            return Ok(reportId);
        }

        //with-last-db
        [HttpPost]
        [Route("~/v1/subscription/events-summary-lastdb")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventsSummaryInfo_LastDb([FromBody] SubscriptionCardexInput searchInput, CancellationToken cancellationToken)
        {
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> items = await _c0_subscriptionEventHandler.HandleWithLastDb(searchInput, cancellationToken);
            return Ok(items);
        }

        [HttpPost]
        [Route("~/v1/subscription/sti-lastdb")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport_LastDb([FromBody] SubscriptionCardexInput searchInput, CancellationToken cancellationToken)
        {
            int reportCode = 230;
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> calculationDetails = await _c0_subscriptionEventHandler.HandleWithLastDb(searchInput, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("~/v1/subscription/summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConsumerSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummaryInfo([FromBody] SearchInput searchInput, CancellationToken cancellation)
        {
            ConsumerSummaryDto summary = await _c1_consumerSummeryHandler.Handle(searchInput.Input, cancellation);
            return Ok(summary);
        }

        [HttpGet]
        [Route("~/v1/subscription/summary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConsumerSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummaryInfoByQuery(string billId, CancellationToken cancellation)
        {
            ConsumerSummaryDto summary = await _c1_consumerSummeryHandler.Handle(billId, cancellation);
            return Ok(summary);
        }
    }
}
