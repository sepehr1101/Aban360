using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Exceptions;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.PaymentPool.BankReceipt
{
    public class BankReceiptController : BaseController
    {
        private readonly IBankReceiptDeleteHandler _c0_handler;
        private readonly IBankReceiptFlowHandler _c1_handler;
        public BankReceiptController(IBankReceiptDeleteHandler c0_handler, IBankReceiptFlowHandler c1_handler)
        {
            _c0_handler = c0_handler;
            _c1_handler = c1_handler;
        }

        [HttpPost("~/v1/bank-receipt/delete-by-bank-date")]
        public async Task<IActionResult> Delete([FromBody] BankReceiptDeleteInputDto input, CancellationToken cancellationToken) => Ok(await _c0_handler.Handle(input, CurrentUser.UserId, cancellationToken));
        [HttpGet("~/v1/bank-receipt/files")]
        public async Task<IActionResult> List(CancellationToken cancellationToken, [FromQuery] int page = 1, [FromQuery] int pageSize = 20) => Ok(await _c1_handler.List(CurrentUser.UserId, page, pageSize, cancellationToken));
        [HttpPost("~/v1/bank-receipt/upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(11 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] short bankId, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                throw new BaseException("فایل را انتخاب کنید.");
            using var stream = file.OpenReadStream();
            var id = await _c1_handler.Upload(stream, file.FileName, bankId, CurrentUser.UserId, CurrentUser.Username, cancellationToken);
            return Ok(new { Id = id });
        }

        [HttpPost("~/v1/bank-receipt/{id:guid}/validate")]
        public async Task<IActionResult> Validate(Guid id, CancellationToken cancellationToken) => Ok(await _c1_handler.Validate(id, CurrentUser.UserId, cancellationToken));
        [HttpGet("~/v1/bank-receipt/{id:guid}/display")]
        public async Task<IActionResult> Display(Guid id, CancellationToken cancellationToken) => Ok(await _c1_handler.Display(id, CurrentUser.UserId, cancellationToken));
        [HttpPost("~/v1/bank-receipt/{id:guid}/reconcile")]
        public async Task<IActionResult> Reconcile(Guid id, CancellationToken cancellationToken) => Ok(await _c1_handler.Reconcile(id, CurrentUser.UserId, cancellationToken));
        [HttpPost("~/v1/bank-receipt/{id:guid}/confirm")]
        public async Task<IActionResult> Confirm(Guid id, [FromBody] BankReceiptConfirmDto input, CancellationToken cancellationToken) => Ok(new { UploaderId = await _c1_handler.Confirm(id, CurrentUser.UserId, input, cancellationToken) });
    }
}
