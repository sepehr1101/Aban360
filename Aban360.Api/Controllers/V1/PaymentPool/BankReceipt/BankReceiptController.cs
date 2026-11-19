using Aban360.Api.Filters;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Services;
using Aban360.Api.Controllers.V1.PaymentPool.BankReceipt;

namespace Aban360.Api.Controllers.V1.PaymentPool.BankReceipt
{
    [Route("v1/bank-receipt")]
    public class BankReceiptController : BaseController
    {
        private readonly IBankReceiptService _bankReceiptService;
        public BankReceiptController(IBankReceiptService bankReceiptService)
        {
            _bankReceiptService = bankReceiptService;
        }

        [HttpPost("delete-by-bank-date")]
        public async Task<IActionResult> Delete([FromBody] BankReceiptDeleteInputDto input, CancellationToken cancellationToken) => Ok(await _bankReceiptService.BankReceiptDeleteAsync(input, CurrentUser.UserId, cancellationToken));
        [HttpGet("files")]
        public async Task<IActionResult> List(CancellationToken cancellationToken, [FromQuery] int page = 1, [FromQuery] int pageSize = 20) => Ok(await _bankReceiptService.BankReceiptFlowListAsync(CurrentUser.UserId, page, pageSize, cancellationToken));
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(11 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] short bankId, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                throw new BaseException("فایل را انتخاب کنید.");
            using var stream = file.OpenReadStream();
            var id = await _bankReceiptService.BankReceiptFlowUploadAsync(stream, file.FileName, bankId, CurrentUser.UserId, CurrentUser.Username, cancellationToken);
            return Ok(new { Id = id });
        }

        [HttpPost("{id:guid}/validate")]
        public async Task<IActionResult> Validate(Guid id, CancellationToken cancellationToken) => Ok(await _bankReceiptService.BankReceiptFlowValidateAsync(id, CurrentUser.UserId, cancellationToken));
        [HttpGet("{id:guid}/display")]
        public async Task<IActionResult> Display(Guid id, CancellationToken cancellationToken) => Ok(await _bankReceiptService.BankReceiptFlowDisplayAsync(id, CurrentUser.UserId, cancellationToken));
        [HttpPost("{id:guid}/reconcile")]
        public async Task<IActionResult> Reconcile(Guid id, CancellationToken cancellationToken) => Ok(await _bankReceiptService.BankReceiptFlowReconcileAsync(id, CurrentUser.UserId, cancellationToken));
        [HttpPost("{id:guid}/confirm")]
        public async Task<IActionResult> Confirm(Guid id, [FromBody] BankReceiptConfirmDto input, CancellationToken cancellationToken) => Ok(new { UploaderId = await _bankReceiptService.BankReceiptFlowConfirmAsync(id, CurrentUser.UserId, input, cancellationToken) });
    }
}
