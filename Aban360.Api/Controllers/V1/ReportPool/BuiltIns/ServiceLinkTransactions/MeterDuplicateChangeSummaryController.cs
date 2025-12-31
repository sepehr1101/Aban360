using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/meter-duplicate-change-summary")]
    public class MeterDuplicateChangeSummaryController : BaseController
    {
        private readonly IMeterDuplicateChangeSummaryHandler _meterDuplicateChangeSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public MeterDuplicateChangeSummaryController(
            IMeterDuplicateChangeSummaryHandler meterDuplicateChangeSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _meterDuplicateChangeSummaryHandler = meterDuplicateChangeSummaryHandler;
            _meterDuplicateChangeSummaryHandler.NotNull(nameof(meterDuplicateChangeSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet, HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Raw(MeterDuplicateChangeSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeSummaryDataOutputDto> result = await _meterDuplicateChangeSummaryHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> Excel(string connectionId, MeterDuplicateChangeSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterDuplicateChangeSummaryHandler.Handle, CurrentUser, ReportLiterals.MeterDuplicateChangeSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(MeterDuplicateChangeSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 721;
            ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeSummaryDataOutputDto> result = await _meterDuplicateChangeSummaryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
