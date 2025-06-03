using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/pending-payments")]
    public class PendingPaymentsController : BaseController
    {
        private readonly IPendingPaymentsHandler _pendingPaymentsHandler;
        public PendingPaymentsController(IPendingPaymentsHandler pendingPaymentsHandler)
        {
            _pendingPaymentsHandler = pendingPaymentsHandler;
            _pendingPaymentsHandler.NotNull(nameof(_pendingPaymentsHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(PendingPaymentsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto> pendingPayments = await _pendingPaymentsHandler.Handle(inputDto, cancellationToken);
            return Ok(pendingPayments);
        }
    }
}
