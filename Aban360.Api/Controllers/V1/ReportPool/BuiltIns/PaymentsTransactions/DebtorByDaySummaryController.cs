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
        public DebtorByDaySummaryController(IDebtorByDaySummaryHandler debtorByDayHandler)
        {
            _debtorByDayHandler = debtorByDayHandler;
            _debtorByDayHandler.NotNull(nameof(_debtorByDayHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(DebtorByDayInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto> debtorByDay = await _debtorByDayHandler.Handle(inputDto, cancellationToken);
            return Ok(debtorByDay);
        }
    }
}
