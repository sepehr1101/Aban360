using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Application.Features.OpenKm.Services;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries
{
    [Route("v1/open-km")]
    public class GetDownloadLinkController : BaseController
    {
        private readonly IOpenKmFilesService _openKmFilesService;
        public GetDownloadLinkController(IOpenKmFilesService openKmFilesService)
        {
            _openKmFilesService = openKmFilesService;
            _openKmFilesService.NotNull(nameof(openKmFilesService));
        }

        [HttpGet]
        [Route("download-link")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDirectoryTree(string uuid, bool oneTimeUse, CancellationToken cancellation)
        {
            string result = await _openKmFilesService.GetDownloadLinkAsync(uuid, oneTimeUse, cancellation);
            return Ok(result);
        }
    }
}
