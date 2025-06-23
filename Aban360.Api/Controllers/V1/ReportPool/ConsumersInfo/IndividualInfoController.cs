using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/individual")]
    public class IndividualInfoController : BaseController
    {
        private readonly IIndividualsInfoGetHandler _individualsSummaryInfoGetHandler;
        public IndividualInfoController(IIndividualsInfoGetHandler individualsSummaryInfoGetHandler)
        {
            _individualsSummaryInfoGetHandler = individualsSummaryInfoGetHandler;
            _individualsSummaryInfoGetHandler.NotNull(nameof(individualsSummaryInfoGetHandler));
        }

        [HttpPost]
        [Route("info")]//todo: change url
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<IndividualsInfoDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            IEnumerable<IndividualsInfoDto> summary = await _individualsSummaryInfoGetHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }
    }
}
