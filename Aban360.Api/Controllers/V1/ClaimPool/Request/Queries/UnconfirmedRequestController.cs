using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class UnconfirmedRequestController : BaseController
    {
        private readonly IRequestCartableService _unconfirmedRequestGetByZoneId;
        public UnconfirmedRequestController(IRequestCartableService unconfirmedRequestGetByZoneId)
        {
            _unconfirmedRequestGetByZoneId = unconfirmedRequestGetByZoneId;
            _unconfirmedRequestGetByZoneId.NotNull(nameof(unconfirmedRequestGetByZoneId));
        }

        [HttpPost, HttpGet]
        [Route("unconfirmed/{zoneId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnconfirmedRequestHeaderOutputDto, UnconfirmedRequestDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UnconfirmedByZone(int zoneId, CancellationToken cancellationToken)
        {
            ReportOutput<UnconfirmedRequestHeaderOutputDto, UnconfirmedRequestDataOutputDto> result = await _unconfirmedRequestGetByZoneId.UnconfirmedRequestGetByZoneIdAsync(zoneId, cancellationToken);
            return Ok(result);
        }
    }
}
