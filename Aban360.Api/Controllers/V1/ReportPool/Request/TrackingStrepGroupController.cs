using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Requests.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Request
{
    [Route("v1/tracking-step-group")]
    public class TrackingStrepGroupController : BaseController
    {
        private readonly ITrackingStepGroupHandler _trackingStepGroupHandler;
        private readonly IReportGenerator _reportGenerator;
        public TrackingStrepGroupController(
            ITrackingStepGroupHandler trackingStepGroupHandler,
            IReportGenerator reportGenerator)
        {
            _trackingStepGroupHandler = trackingStepGroupHandler;
            _trackingStepGroupHandler.NotNull(nameof(trackingStepGroupHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingStepHeaderOutputDto, TrackingStepGroupDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTrackingStepGroupByZone([FromBody] TrackingInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<TrackingStepHeaderOutputDto, TrackingStepGroupDataOutputDto> result = await _trackingStepGroupHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, TrackingInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _trackingStepGroupHandler.Handle, CurrentUser, ReportLiterals.Tracking, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSti([FromBody] TrackingInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2020;
            ReportOutput<TrackingStepHeaderOutputDto, TrackingStepGroupDataOutputDto> result = await _trackingStepGroupHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
