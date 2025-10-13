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
    [Route("v1/water-meter-replacements-summary-by-change-cause")]
    public class WaterMeterReplacementsSummaryByChangeCauseController : BaseController
    {
        private readonly IWaterMeterReplacementsSummaryByChangeCauseHandler _waterMeterReplacementsSummaryByChangeCauseHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterMeterReplacementsSummaryByChangeCauseController(
            IWaterMeterReplacementsSummaryByChangeCauseHandler waterMeterReplacementsSummaryByChangeCauseHandler,
            IReportGenerator reportGenerator)
        {
            _waterMeterReplacementsSummaryByChangeCauseHandler = waterMeterReplacementsSummaryByChangeCauseHandler;
            _waterMeterReplacementsSummaryByChangeCauseHandler.NotNull(nameof(waterMeterReplacementsSummaryByChangeCauseHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterMeterReplacementsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto> waterMeterReplacementsSummaryByChangeCause = await _waterMeterReplacementsSummaryByChangeCauseHandler.Handle(input, cancellationToken);
            return Ok(waterMeterReplacementsSummaryByChangeCause);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterMeterReplacementsInputDto inputDto, CancellationToken cancellationToken)
        {
            string stateTitle = inputDto.IsChangeDate ? ReportLiterals.ChangeDate : ReportLiterals.RegisterDate;
            string reportTitle = ReportLiterals.WaterMeterReplacementsSummary(stateTitle) + ReportLiterals.ByChangeCause;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterMeterReplacementsSummaryByChangeCauseHandler.Handle, CurrentUser, reportTitle, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WaterMeterReplacementsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 259;
            ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto> waterMeterReplacements = await _waterMeterReplacementsSummaryByChangeCauseHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(waterMeterReplacements, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
