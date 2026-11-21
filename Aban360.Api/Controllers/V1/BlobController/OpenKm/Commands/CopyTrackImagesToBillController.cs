using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.Common.Categories.ApiResponse;
using Aban360.BlobPool.Application.Features.OpenKm.Services;
using Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public sealed class CopyTrackImagesToBillController : BaseController
    {
        private readonly IOpenKmFoldersMetadataService _openKmFoldersMetadataService;
        public CopyTrackImagesToBillController(IOpenKmFoldersMetadataService openKmFoldersMetadataService)
        {
            _openKmFoldersMetadataService = openKmFoldersMetadataService;
        }

        [HttpPost("copy-track-images-to-bill")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CopyTrackImagesToBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Copy([FromBody] CopyTrackImagesToBillInputDto input, CancellationToken cancellationToken)
        {
            var result = await _openKmFoldersMetadataService.CopyTrackImagesToBillAsync(input, CurrentUser, cancellationToken);
            string message = result.IsComplete ? "کپی تصاویر تکمیل شد" : "کپی تصاویر کامل نشد؛ جزئیات فایل‌ها را بررسی کنید";
            if (result.Warnings.Count > 0)
                message += "؛ ثبت گزارش عملیات با خطا مواجه شد";
            return Ok(result, message);
        }
    }
}
