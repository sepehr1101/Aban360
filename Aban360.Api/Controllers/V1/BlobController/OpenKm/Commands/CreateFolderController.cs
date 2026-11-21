using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Application.Features.OpenKm.Services;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public class CreateFolderController : BaseController
    {
        private readonly IOpenKmFoldersMetadataService _openKmFoldersMetadataService;
        public CreateFolderController(IOpenKmFoldersMetadataService openKmFoldersMetadataService)
        {
            _openKmFoldersMetadataService = openKmFoldersMetadataService;
            _openKmFoldersMetadataService.NotNull(nameof(openKmFoldersMetadataService));
        }

        [HttpGet]
        [Route("create-folder")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateFolder(string folderName, CancellationToken cancellation)
        {
            string uuid = await _openKmFoldersMetadataService.CreateFolderAsync(folderName, cancellation);
            return Ok(uuid);
        }
    }
}
