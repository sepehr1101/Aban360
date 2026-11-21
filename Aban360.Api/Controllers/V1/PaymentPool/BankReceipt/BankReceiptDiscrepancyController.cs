using Microsoft.AspNetCore.Mvc;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Application.Features.BankReceipt.Services;
using Aban360.Api.Controllers.V1.PaymentPool.BankReceipt;

namespace Aban360.Api.Controllers.V1.PaymentPool.BankReceipt
{
    [Route("v1/bank-receipt/discrepancy")]
    public sealed class BankReceiptDiscrepancyController : BaseController
    {
        private readonly IBankReceiptService _bankReceiptService;
        public BankReceiptDiscrepancyController(IBankReceiptService bankReceiptService)
        {
            _bankReceiptService = bankReceiptService;
        }

        [HttpPost("report")]
        public async Task<IActionResult> Report([FromBody] BankReceiptDiscrepancyReportInputDto input, CancellationToken cancellationToken) => Ok(await _bankReceiptService.BankReceiptDiscrepancyReportAsync(input, CurrentUser.UserId, cancellationToken));
        [HttpPost("{importId:guid}/{lineNumber:int}/description")]
        public async Task<IActionResult> Describe(Guid importId, int lineNumber, [FromBody] BankReceiptDiscrepancyDescriptionDto input, CancellationToken cancellationToken)
        {
            await _bankReceiptService.BankReceiptDiscrepancyDescribeAsync(importId, lineNumber, input, CurrentUser.UserId, cancellationToken);
            return Ok(new { ImportId = importId, LineNumber = lineNumber });
        }
    }
}
