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
    [Route("v1/debtor-by-day-summary")]
    public class DebtorByDaySummaryController : BaseController
    {
        private readonly IDebtorByDaySummaryHandler _debtorByDayHandler;
        private readonly IReportGenerator _reportGenerator;
        public DebtorByDaySummaryController(
            IDebtorByDaySummaryHandler debtorByDayHandler,
            IReportGenerator reportGenerator)
        {
            _debtorByDayHandler = debtorByDayHandler;
            _debtorByDayHandler.NotNull(nameof(_debtorByDayHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(DebtorByDayInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto> debtorByDay = await _debtorByDayHandler.Handle(inputDto, cancellationToken);
            return Ok(debtorByDay);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, DebtorByDayInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _debtorByDayHandler.Handle, CurrentUser, ReportLiterals.DebtorByDaySummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(DebtorByDayInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 518;
            ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto> result = await _debtorByDayHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
