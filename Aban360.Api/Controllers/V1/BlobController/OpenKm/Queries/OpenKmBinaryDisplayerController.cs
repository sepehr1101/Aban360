using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using Aban360.BlobPool.Application.Features.OpenKm.Services;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries
{
    [Route("v1/open-km")]
    public class OpenKmBinaryDisplayerController : BaseController
    {
        private readonly IOpenKmFilesService _openKmFilesService;
        public OpenKmBinaryDisplayerController(IOpenKmFilesService openKmFilesService)
        {
            _openKmFilesService = openKmFilesService;
            _openKmFilesService.NotNull(nameof(_openKmFilesService));
        }

        [HttpGet]
        [Route("document-binary")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDocumentBinary(string documentId, [Optional] bool isThumbnail, CancellationToken cancellationToken)
        {
            byte[] binary = await _openKmFilesService.GetDocumentBinaryAsync(documentId, isThumbnail, cancellationToken);
            return Ok(binary);
        }
    }
}
