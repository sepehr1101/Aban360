using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/closed-summary")]
    public class ClosedSummaryController : BaseController
    {
        private readonly IClosedSummaryHandler _closedHandler;
        private readonly IReportGenerator _reportGenerator;
        public ClosedSummaryController(
            IClosedSummaryHandler closedHandler,
            IReportGenerator reportGenerator)
        {
            _closedHandler = closedHandler;
            _closedHandler.NotNull(nameof(closedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ClosedHeaderOutputDto, ClosedSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ClosedInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ClosedHeaderOutputDto, ClosedSummaryDataOutputDto> result = await _closedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ClosedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _closedHandler.Handle, CurrentUser, ReportLiterals.ClosedSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ClosedInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 711;
            ReportOutput<ClosedHeaderOutputDto, ClosedSummaryDataOutputDto> calculationDetails = await _closedHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
