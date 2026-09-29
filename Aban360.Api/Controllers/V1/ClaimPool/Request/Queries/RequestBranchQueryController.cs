using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class RequestBranchQueryController : BaseController
    {
        private readonly IKartableRequestGetAllHandler _requestKartableGetAllHandler;
        private readonly IDisplayRequestHandler _displayRequestHandler;
        private readonly IDisplayRequestByTrackIdHandler _displayRequestByTrackIdHandler;
        private readonly IRequestBasicInfoGetHandler _requestBasicInfoGetHandler;
        private readonly IToSetAssessmentTimeGetByTrackIdHandler _toSetAssessmentTimeGetHandler;
        private readonly IToSetReAssessmentTimeGetByTrackIdHandler _toSetReAssessmentTimeGetHandler;
        private readonly IToCalulationConfirmHandler _toSetCalulationHandler;
        public RequestBranchQueryController(
            IKartableRequestGetAllHandler requestKartableGetAllHandler,
            IDisplayRequestHandler displayRequestHandler,
            IDisplayRequestByTrackIdHandler displayRequestByTrackIdHandler,
            IRequestBasicInfoGetHandler requestBasicInfoGetHandler,
            IToSetAssessmentTimeGetByTrackIdHandler toSetAssessmentTimeGetHandler,
            IToSetReAssessmentTimeGetByTrackIdHandler toSetReAssessmentTimeGetHandler,
            IToCalulationConfirmHandler toSetCalulationHandler)
        {
            _requestKartableGetAllHandler = requestKartableGetAllHandler;
            _requestKartableGetAllHandler.NotNull(nameof(requestKartableGetAllHandler));

            _displayRequestByTrackIdHandler = displayRequestByTrackIdHandler;
            _displayRequestByTrackIdHandler.NotNull(nameof(displayRequestByTrackIdHandler));

            _displayRequestHandler = displayRequestHandler;
            _displayRequestHandler.NotNull(nameof(displayRequestHandler));

            _requestBasicInfoGetHandler = requestBasicInfoGetHandler;
            _requestBasicInfoGetHandler.NotNull(nameof(requestBasicInfoGetHandler));

            _toSetAssessmentTimeGetHandler = toSetAssessmentTimeGetHandler;
            _toSetAssessmentTimeGetHandler.NotNull(nameof(toSetAssessmentTimeGetHandler));

            _toSetReAssessmentTimeGetHandler = toSetReAssessmentTimeGetHandler;
            _toSetReAssessmentTimeGetHandler.NotNull(nameof(toSetReAssessmentTimeGetHandler));

            _toSetCalulationHandler = toSetCalulationHandler;
            _toSetCalulationHandler.NotNull(nameof(toSetCalulationHandler));
        }


        [HttpGet]
        [Route("kartable")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestKartable(CancellationToken cancellationToken)
        {
            ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto> result = await _requestKartableGetAllHandler.Handle(CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("display")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayRequest([FromBody] ZoneIdAndTrackNumber inputDto, CancellationToken cancellationToken)
        {
            MoshtrakDataOutputDto result = await _displayRequestHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("display/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayRequestByTrackId(Guid trackId, CancellationToken cancellationToken)
        {
            MoshtrakDataOutputDto result = await _displayRequestByTrackIdHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("basic-info/{trackNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestBasicInfoDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBasicInfo(int trackNumber, CancellationToken cancellationToken)
        {
            RequestBasicInfoDataOutputDto result = await _requestBasicInfoGetHandler.Handle(trackNumber, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("set-assessment-time/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ToSetAssessmentTimeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentTimeInfo(Guid trackId, CancellationToken cancellationToken)
        {
            ToSetAssessmentTimeGetDto result = await _toSetAssessmentTimeGetHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("set-reAssessment-time/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ToSetReAssessmentTimeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReAssessmentTimeInfo(Guid trackId, CancellationToken cancellationToken)
        {
            ToSetReAssessmentTimeGetDto result = await _toSetReAssessmentTimeGetHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("calculation-confirm/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ToCalculationConfirmGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSetCalculationInfo(Guid trackId, CancellationToken cancellationToken)
        {
            ToCalculationConfirmGetDto result = await _toSetCalulationHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        //amount-confirm
    }
}
