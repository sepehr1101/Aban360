using Aban360.Api.Filters;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Application.Features.OpenKm.Services;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public class RemoveFileController : BaseController
    {
        private readonly IOpenKmFilesService _openKmFilesService;
        public RemoveFileController(IOpenKmFilesService openKmFilesService)
        {
            _openKmFilesService = openKmFilesService;
            _openKmFilesService.NotNull(nameof(openKmFilesService));
        }

        [HttpPost, HttpDelete]
        [Route("remove-file")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        [TypeFilter(typeof(EndpointAuthorizationFilter))]
        public async Task<IActionResult> RemoveFile([FromBody] RemoveFileDto removeFileDto, CancellationToken cancellation)
        {
            await _openKmFilesService.RemoveFileAsync(removeFileDto, CurrentUser, cancellation);
            return Ok(removeFileDto);
        }
    }
}
