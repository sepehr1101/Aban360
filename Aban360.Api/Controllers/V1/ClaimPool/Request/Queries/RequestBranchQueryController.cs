using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class RequestBranchQueryController : BaseController
    {
        private readonly IRequestDisplayService _requestDisplayService;
        public RequestBranchQueryController(IRequestDisplayService requestDisplayService)
        {
            _requestDisplayService = requestDisplayService;
            _requestDisplayService.NotNull(nameof(requestDisplayService));
        }

        [HttpGet]
        [Route("kartable")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestKartable(CancellationToken cancellationToken)
        {
            ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto> result = await _requestDisplayService.KartableRequestGetAllAsync(CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("display")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayRequest([FromBody] ZoneIdAndTrackNumber inputDto, CancellationToken cancellationToken)
        {
            MoshtrakDataOutputDto result = await _requestDisplayService.DisplayRequestAsync(inputDto.TrackNumber, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("display-by-track/{trackNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayRequest(int trackNumber, CancellationToken cancellationToken)
        {
            MoshtrakDataOutputDto result = await _requestDisplayService.DisplayRequestAsync(trackNumber, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("display/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayRequestByTrackId(Guid trackId, CancellationToken cancellationToken)
        {
            MoshtrakDataOutputDto result = await _requestDisplayService.DisplayRequestByTrackIdAsync(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("basic-info/{trackNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestBasicInfoDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBasicInfo(int trackNumber, CancellationToken cancellationToken)
        {
            RequestBasicInfoDataOutputDto result = await _requestDisplayService.RequestBasicInfoGetAsync(trackNumber, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("set-assessment-time/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ToSetAssessmentTimeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentTimeInfo(Guid trackId, CancellationToken cancellationToken)
        {
            ToSetAssessmentTimeGetDto result = await _requestDisplayService.ToSetAssessmentTimeGetByTrackIdAsync(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("set-reAssessment-time/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ToSetReAssessmentTimeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReAssessmentTimeInfo(Guid trackId, CancellationToken cancellationToken)
        {
            ToSetReAssessmentTimeGetDto result = await _requestDisplayService.ToSetReAssessmentTimeGetByTrackIdAsync(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("calculation-confirm/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ToCalculationConfirmGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSetCalculationInfo(Guid trackId, CancellationToken cancellationToken)
        {
            ToCalculationConfirmGetDto result = await _requestDisplayService.ToCalulationConfirmAsync(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("block-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<StringDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetBlocksDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<StringDictionary> result = _requestDisplayService.ReadingBlockListGetAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("kit-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetKitsDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = _requestDisplayService.KitCodeDictionaryGetAsync(cancellationToken);
            return Ok(result);
        }
    }
}
