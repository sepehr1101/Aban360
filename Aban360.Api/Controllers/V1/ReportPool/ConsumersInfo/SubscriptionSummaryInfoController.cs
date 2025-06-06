﻿using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/subscription")]
    public class SubscriptionSummaryInfoController : BaseController
    {
        private readonly IConsumerSummaryQueryService _consumerSummeryQueryService;
        public SubscriptionSummaryInfoController(IConsumerSummaryQueryService consumerSummaryQueryService)
        {
            _consumerSummeryQueryService = consumerSummaryQueryService;
            _consumerSummeryQueryService.NotNull(nameof(_consumerSummeryQueryService));
        }

        [HttpPost]
        [Route("summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConsumerSummaryDto>), StatusCodes.Status200OK)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetSummaryInfo([FromBody] SearchInput searchInput)
        {
            ConsumerSummaryDto summary = await _consumerSummeryQueryService.GetInfo(searchInput.Input);
            return Ok(summary);
        }
    }
}
