using Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Tracking.Queries
{
    [Route("v1/tracking")]
    public class SmsByTrackIdGetController : BaseController
    {
        private readonly ISmsByTrackIdGetHandler _smsByTrackIdHandler;
        public SmsByTrackIdGetController(ISmsByTrackIdGetHandler smsByTrackIdHandler)
        {
            _smsByTrackIdHandler = smsByTrackIdHandler;
            _smsByTrackIdHandler.NotNull(nameof(smsByTrackIdHandler));
        }

        [HttpPost]
        [Route("display-sms")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayFlow([FromBody] GuidInput input, CancellationToken cancellationToken)
        {
            ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto> result = await _smsByTrackIdHandler.Handle(input.Input, cancellationToken);
            return Ok(result);
        }
    }
}
