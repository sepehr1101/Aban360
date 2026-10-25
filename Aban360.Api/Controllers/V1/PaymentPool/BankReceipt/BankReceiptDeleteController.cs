using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.BankReceipt
{
    [Route("v1/bank-receipt")]
    public sealed class BankReceiptDeleteController : BaseController
    {
        private readonly IBankReceiptDeleteHandler _handler;
        public BankReceiptDeleteController(IBankReceiptDeleteHandler handler) { _handler = handler; }
        [HttpPost("delete-by-bank-date")]
        public async Task<IActionResult> Delete([FromBody] BankReceiptDeleteInputDto input, CancellationToken cancellationToken)
            => Ok(await _handler.Handle(input, CurrentUser.UserId, cancellationToken));
    }
}
