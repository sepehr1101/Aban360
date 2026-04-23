using Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Tracking.Queries
{
    [Route("v1/tracking")]
    public class SmsByTrackIdGetController : BaseController
    {
        private readonly ISmsByTrackIdGetHandler _smsByTrackIdHandler;
        private readonly ISmsByQueueIdGetHandler _smsByQueueIdHandler;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public SmsByTrackIdGetController(
            ISmsByTrackIdGetHandler smsByTrackIdHandler,
            ISmsByQueueIdGetHandler smsByQueueIdHandler,
            ISmsOldHandler smsOldHandler,
            IBackgroundJobClient backgroundJobClient)
        {
            _smsByTrackIdHandler = smsByTrackIdHandler;
            _smsByTrackIdHandler.NotNull(nameof(smsByTrackIdHandler));

            _smsByQueueIdHandler = smsByQueueIdHandler;
            _smsByQueueIdHandler.NotNull(nameof(smsByQueueIdHandler));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));
        }

        [HttpPost]
        [Route("display-sms")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayFlow([FromBody] GuidInput input, CancellationToken cancellationToken)
        {
            ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto> result = await _smsByTrackIdHandler.Handle(input.Input, cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("resend-sms")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendSms([FromBody] GuidInput input, CancellationToken cancellationToken)
        {
            TrackingSmsDataOutputDto result = await _smsByQueueIdHandler.Handle(input.Input, cancellationToken);
            _backgroundJobClient.Enqueue(() => _smsOldHandler.Send(result.Receiver, result.Message));
            return Ok(result);
        }
    }
}
