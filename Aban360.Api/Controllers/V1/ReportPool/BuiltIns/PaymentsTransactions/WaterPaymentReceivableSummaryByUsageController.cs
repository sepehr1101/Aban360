using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/water-payment-receivable-summary-by-usage")]
    public class WaterPaymentReceivableSummaryByUsageController : BaseController
    {
        private readonly IWaterPaymentReceivableSummaryHandler _waterPaymentReceivable;
        private readonly IReportGenerator _reportGenerator;
        public WaterPaymentReceivableSummaryByUsageController(
            IWaterPaymentReceivableSummaryHandler waterPaymentReceivable,
            IReportGenerator reportGenerator)
        {
            _waterPaymentReceivable = waterPaymentReceivable;
            _waterPaymentReceivable.NotNull(nameof(_waterPaymentReceivable));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterPaymentReceivableInputDto inputDto, CancellationToken cancellationToken)
        {
            inputDto.IsZone = false;
            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto> waterPaymentReceivable = await _waterPaymentReceivable.Handle(inputDto, cancellationToken);
            return Ok(waterPaymentReceivable);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterPaymentReceivableInputDto inputDto, CancellationToken cancellationToken)
        {
            inputDto.IsZone = false;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterPaymentReceivable.Handle, CurrentUser, ReportLiterals.WaterPaymentReceivableSummary+ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(WaterPaymentReceivableInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 571;
            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto> result = await _waterPaymentReceivable.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
