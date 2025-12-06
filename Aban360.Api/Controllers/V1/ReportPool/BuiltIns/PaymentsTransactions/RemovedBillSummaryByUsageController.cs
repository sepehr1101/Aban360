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
    [Route("v1/removed-bill-summary-by-usage")]
    public class RemovedBillSummaryByUsageController : BaseController
    {
        private readonly IRemovedBillSummaryByUsageHandler _removedBillHandler;
        private readonly IReportGenerator _reportGenerator;
        public RemovedBillSummaryByUsageController(
            IRemovedBillSummaryByUsageHandler removedBillHandler,
            IReportGenerator reportGenerator)
        {
            _removedBillHandler = removedBillHandler;
            _removedBillHandler.NotNull(nameof(_removedBillHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(RemovedBillRawInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto> removedBill = await _removedBillHandler.Handle(inputDto, cancellationToken);
            return Ok(removedBill);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, RemovedBillRawInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _removedBillHandler.Handle, CurrentUser, ReportLiterals.RemovedBillSummary + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(RemovedBillRawInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 441;
            ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto> result = await _removedBillHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
