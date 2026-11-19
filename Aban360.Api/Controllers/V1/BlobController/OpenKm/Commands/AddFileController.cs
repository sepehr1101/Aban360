using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Application.Features.OpenKm.Services;
using Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public class AddFileController : BaseController
    {
        private readonly IOpenKmFilesService _openKmFilesService;
        public AddFileController(IOpenKmFilesService openKmFilesService)
        {
            _openKmFilesService = openKmFilesService;
            _openKmFilesService.NotNull(nameof(openKmFilesService));
        }

        [HttpPost]
        [Route("add-file")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AddFileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddFile([FromForm] AddFormFileInput input, CancellationToken cancellation)
        {
            AddFileDto result = await _openKmFilesService.AddFileAsync(input, CurrentUser, cancellation);
            return Ok(result);
        }

        [HttpPost]
        [Route("add-file-discount")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AddFileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddFileDiscount([FromForm] AddDiscountFileInput input, CancellationToken cancellation)
        {
            AddFileDto result = await _openKmFilesService.AddFileAsync(input, CurrentUser, cancellation);
            return Ok(result);
        }

        [HttpPost]
        [Route("add-reading-image")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(21 * 1024 * 1024)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AddFileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddReadingImage([FromForm] AddReadingImageInput input, [FromServices] IReadingArchiveService handler, CancellationToken cancellation)
        {
            return Ok(await handler.Add(input, CurrentUser, cancellation));
        }

        [HttpPost]
        [Route("add-file-base64")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AddFileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddFile(AddBase64FileInput input, CancellationToken cancellation)
        {
            AddFileDto result = await _openKmFilesService.AddFileAsync(input, CurrentUser, cancellation);
            return Ok(result);
        }
    }
}
