using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/estate")]
    public class EstateInfoController : BaseController
    {
        private readonly IEstatesInfoGetHandler _estatesSummaryInfoGetHandler;
        public EstateInfoController(IEstatesInfoGetHandler estatesSummaryInfoGetHandler)
        {
            _estatesSummaryInfoGetHandler = estatesSummaryInfoGetHandler;
            _estatesSummaryInfoGetHandler.NotNull(nameof(estatesSummaryInfoGetHandler));
        }

        [HttpPost]
        [Route("info")]//todo: change url
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<EstatesInfoDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            IEnumerable<EstatesInfoDto> summary = await _estatesSummaryInfoGetHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }
    }
}
