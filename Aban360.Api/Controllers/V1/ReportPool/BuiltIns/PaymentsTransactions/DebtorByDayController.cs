using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/debtor-by-day")]
    public class DebtorByDayController : BaseController
    {
        private readonly IDebtorByDayHandler _debtorByDayHandler;
        public DebtorByDayController(IDebtorByDayHandler DebtorByDayHandler)
        {
            _debtorByDayHandler = DebtorByDayHandler;
            _debtorByDayHandler.NotNull(nameof(_debtorByDayHandler));
        }

        [HttpPost, HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDayDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIfo(DebtorByDayInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDayDataOutputDto> debtorByDay = await _debtorByDayHandler.Handle(inputDto, cancellationToken);
            return Ok(debtorByDay);
        }
    }
}
