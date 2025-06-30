using Aban360.BrdigeApi.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer")]
    public class CustomerSummaryInfoController : BaseController
    {
        private readonly IConsumerSummaryQueryService _consumerSummeryQueryService;
        private readonly ILatestDebtService _latestDebtService;

        public CustomerSummaryInfoController(
            IConsumerSummaryQueryService consumerSummaryQueryService,
            ILatestDebtService latestDebtService)
        {
            _consumerSummeryQueryService = consumerSummaryQueryService;
            _consumerSummeryQueryService.NotNull(nameof(_consumerSummeryQueryService));

            _latestDebtService = latestDebtService;
            _latestDebtService.NotNull(nameof(_latestDebtService));
        }

        [HttpPost]
        [Route("summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConsumerSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummaryInfo([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            ConsumerSummaryDto summary = await _consumerSummeryQueryService.GetInfo(searchInput.Input);
            return Ok(summary);
        }

        [HttpPost]
        [Route("latest-debt")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LatestDebtDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLatestDebt([FromBody] SearchInput searchInput)
        {
            LatestDebtDto latestDebtDto= await _latestDebtService.GetLatestDebt(searchInput.Input);
            return Ok(latestDebtDto);
        }

    }
}
