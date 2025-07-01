using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/violation")]
    public class ViolationInfoController : BaseController
    {
        private readonly IViolationInfoGetHandler _violationSummaryInfoGetHandler;
        public ViolationInfoController(IViolationInfoGetHandler violationSummaryInfoGetHandler)
        {
            _violationSummaryInfoGetHandler = violationSummaryInfoGetHandler;
            _violationSummaryInfoGetHandler.NotNull(nameof(violationSummaryInfoGetHandler));
        }

        [HttpPost]
        [Route("info")]//todo: change url
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ViolationInfoDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            IEnumerable<ViolationInfoDto> summary = await _violationSummaryInfoGetHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }
    }
}
