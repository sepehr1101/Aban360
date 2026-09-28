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
    public class RequestBranchInfoController : BaseController
    {
        private readonly IKartableRequestGetAllHandler _requestKartableGetAllHandler;
        private readonly IToSetAssessmentTimeGetByTrackIdHandler _toSetAssessmentTimeGetHandler;
        private readonly IToSetReAssessmentTimeGetByTrackIdHandler _toSetReAssessmentTimeGetHandler;
        private readonly IToCalulationConfirmHandler _toSetCalulationHandler;
        public RequestBranchInfoController(
               IKartableRequestGetAllHandler requestKartableGetAllHandler,
               IToSetAssessmentTimeGetByTrackIdHandler toSetAssessmentTimeGetHandler,
               IToSetReAssessmentTimeGetByTrackIdHandler toSetReAssessmentTimeGetHandler,
               IToCalulationConfirmHandler toSetCalulationHandler)
        {
            _requestKartableGetAllHandler = requestKartableGetAllHandler;
            _requestKartableGetAllHandler.NotNull(nameof(requestKartableGetAllHandler));

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
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSetCalculationInfo(Guid trackId, CancellationToken cancellationToken)
        {
            MoshtrakDataOutputDto result = await _toSetCalulationHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }
       
        //amount-confirm
    }
}
