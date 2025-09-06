using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries
{
    [Route("v1/open-km")]
    public class OpenKmBinaryDisplayerController : BaseController
    {
        private readonly IGetDocumentBinaryHandler _documentDisplayerHandler;
        public OpenKmBinaryDisplayerController(IGetDocumentBinaryHandler getDocumentBinaryHandler)
        {
            _documentDisplayerHandler = getDocumentBinaryHandler;
            _documentDisplayerHandler.NotNull(nameof(_documentDisplayerHandler));
        }

        [HttpGet]
        [Route("document-binary")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDocumentBinary(string documentId, [Optional]bool isThumbnail, CancellationToken cancellationToken)
        {
            byte[] binary = await _documentDisplayerHandler.Handle(documentId, isThumbnail, cancellationToken);
            return Ok(binary);
        }
    }
}
