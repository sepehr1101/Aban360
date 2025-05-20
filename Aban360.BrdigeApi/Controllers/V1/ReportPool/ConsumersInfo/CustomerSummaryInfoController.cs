using Aban360.BrdigeApi.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer")]
    public class CustomerSummaryInfoController : BaseController
    {
        private readonly IConsumerSummaryQueryService _consumerSummeryQueryService;
        public CustomerSummaryInfoController(IConsumerSummaryQueryService consumerSummaryQueryService)
        {
            _consumerSummeryQueryService = consumerSummaryQueryService;
            _consumerSummeryQueryService.NotNull(nameof(_consumerSummeryQueryService));
        }

        [HttpPost]
        [Route("summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConsumerSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummaryInfo([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            ConsumerSummaryDto summary = await _consumerSummeryQueryService.GetInfo(searchInput.Input);
            return Ok(summary);
        }
    }
}
