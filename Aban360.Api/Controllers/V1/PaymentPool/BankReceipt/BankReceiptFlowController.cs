using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.BankReceipt
{
    [Route("v1/bank-receipt")]
    public sealed class BankReceiptFlowController : BaseController
    {
        private readonly IBankReceiptFlowHandler _handler;
        public BankReceiptFlowController(IBankReceiptFlowHandler handler) { _handler = handler; }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(11 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] short bankId, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0) throw new BaseException("فایل را انتخاب کنید.");
            using var stream = file.OpenReadStream();
            var id = await _handler.Upload(stream, file.FileName, bankId, CurrentUser.UserId, CurrentUser.Username, cancellationToken);
            return Ok(new { Id = id });
        }
        [HttpPost("{id:guid}/validate")]
        public async Task<IActionResult> Validate(Guid id, CancellationToken cancellationToken)
            => Ok(await _handler.Validate(id, CurrentUser.UserId, cancellationToken));

        [HttpGet("{id:guid}/display")]
        public async Task<IActionResult> Display(Guid id, CancellationToken cancellationToken)
            => Ok(await _handler.Display(id, CurrentUser.UserId, cancellationToken));

        [HttpPost("{id:guid}/reconcile")]
        public async Task<IActionResult> Reconcile(Guid id, CancellationToken cancellationToken)
            => Ok(await _handler.Reconcile(id, CurrentUser.UserId, cancellationToken));

        [HttpPost("{id:guid}/confirm")]
        public async Task<IActionResult> Confirm(Guid id, [FromBody] BankReceiptConfirmDto input, CancellationToken cancellationToken)
            => Ok(new { UploaderId = await _handler.Confirm(id, CurrentUser.UserId, input, cancellationToken) });
    }
}
