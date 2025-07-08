using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/debtor-by-day-detail")]
    public class DebtorByDayDetailController : BaseController
    {
        private readonly IDebtorByDayDetailHandler _debtorByDayHandler;
        public DebtorByDayDetailController(IDebtorByDayDetailHandler debtorByDayHandler)
        {
            _debtorByDayHandler = debtorByDayHandler;
            _debtorByDayHandler.NotNull(nameof(_debtorByDayHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDayDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(DebtorByDayInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDayDetailDataOutputDto> debtorByDay = await _debtorByDayHandler.Handle(inputDto, cancellationToken);
            return Ok(debtorByDay);
        }
    }
}
