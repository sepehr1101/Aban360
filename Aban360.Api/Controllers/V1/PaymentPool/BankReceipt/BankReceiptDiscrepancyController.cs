using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.BankReceipt
{
    [Route("v1/bank-receipt/discrepancy")]
    public sealed class BankReceiptDiscrepancyController : BaseController
    {
        private readonly IBankReceiptDiscrepancyHandler _handler;
        public BankReceiptDiscrepancyController(IBankReceiptDiscrepancyHandler handler) { _handler = handler; }
        [HttpPost("report")]
        public async Task<IActionResult> Report([FromBody] BankReceiptDiscrepancyReportInputDto input, CancellationToken cancellationToken)
            => Ok(await _handler.Report(input, CurrentUser.UserId, cancellationToken));

        [HttpPost("{importId:guid}/{lineNumber:int}/description")]
        public async Task<IActionResult> Describe(Guid importId, int lineNumber,
            [FromBody] BankReceiptDiscrepancyDescriptionDto input, CancellationToken cancellationToken)
        {
            await _handler.Describe(importId, lineNumber, input, CurrentUser.UserId, cancellationToken);
            return Ok(new { ImportId = importId, LineNumber = lineNumber });
        }
    }
}
