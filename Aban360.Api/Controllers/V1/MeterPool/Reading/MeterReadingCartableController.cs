using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Reading
{
    [Route("v1/meter-reading")]
    public class MeterReadingCartableController : BaseController
    {
        private readonly IReadingCartableGetHandler _readingCartableHandler;
        private readonly IReadingDetailGetHandler _readingDetailGetHandler;
        private readonly IReadingTrackingFlowGetHandler _readingTrackingFlowGetHandler;
        public MeterReadingCartableController(
            IReadingCartableGetHandler readingCartableHandler, 
            IReadingDetailGetHandler readingDetailGetHandler,
            IReadingTrackingFlowGetHandler readingTrackingFlowGetHandler)
        {
            _readingCartableHandler = readingCartableHandler;
            _readingCartableHandler.NotNull(nameof(readingCartableHandler));
            
            _readingDetailGetHandler = readingDetailGetHandler;
            _readingDetailGetHandler.NotNull(nameof(readingDetailGetHandler));

            _readingTrackingFlowGetHandler = readingTrackingFlowGetHandler;
            _readingTrackingFlowGetHandler.NotNull(nameof(readingTrackingFlowGetHandler));
        }

        [Route("cartable")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingCartableHeaderOutputDto, MeterReadingCartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCartable([FromBody] MeterReadingCartableInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingCartableHeaderOutputDto, MeterReadingCartableDataOutputDto> result = await _readingCartableHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
        
        [Route("detail/{trackingId}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DetailHeaderDto, DetailDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(Guid trackingId, CancellationToken cancellationToken)
        {
            ReportOutput<DetailHeaderDto, DetailDataDto> result = await _readingDetailGetHandler.Handle(trackingId, CurrentUser, cancellationToken);
            return Ok(result);
        }
        
        [Route("tracking-flow/{trackNumber}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DetailHeaderDto, DetailDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTrackingFlow(int trackNumber, CancellationToken cancellationToken)
        {
            ReportOutput<TrackingHeaderDto, TrackingDataDto> result = await _readingTrackingFlowGetHandler.Handle(trackNumber, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
