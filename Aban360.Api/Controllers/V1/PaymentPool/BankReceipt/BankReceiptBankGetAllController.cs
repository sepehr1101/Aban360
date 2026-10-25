using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.BankReceipt
{
    [Route("v1/bank-receipt/banks")]
    public sealed class BankReceiptBankGetAllController : BaseController
    {
        private readonly IBankReceiptBankGetAllHandler _handler;
        public BankReceiptBankGetAllController(IBankReceiptBankGetAllHandler handler) { _handler = handler; }
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
            => Ok(await _handler.Handle(cancellationToken));
    }
}
