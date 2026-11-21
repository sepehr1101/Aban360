using Microsoft.AspNetCore.Mvc;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Application.Features.BankReceipt.Services;
using Aban360.Api.Controllers.V1.PaymentPool.BankReceipt;

namespace Aban360.Api.Controllers.V1.PaymentPool.BankReceipt
{
    [Route("v1/bank-receipt/banks")]
    public sealed class BankReceiptBankGetAllController : BaseController
    {
        private readonly IBankReceiptService _bankReceiptService;
        public BankReceiptBankGetAllController(IBankReceiptService bankReceiptService)
        {
            _bankReceiptService = bankReceiptService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken) => Ok(await _bankReceiptService.BankReceiptBankGetAllAsync(cancellationToken));
    }
}
