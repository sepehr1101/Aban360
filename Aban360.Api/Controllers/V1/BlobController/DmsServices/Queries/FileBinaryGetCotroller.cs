using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Queries
{
    [Route("v1/dms-file-binary")]
    public class FileBinaryGetCotroller : BaseController
    {
        private readonly IFilesBinaryGetHandler _fileBinaryHandler;
        public FileBinaryGetCotroller(IFilesBinaryGetHandler fileBinaryHandler)
        {
            _fileBinaryHandler = fileBinaryHandler;
            _fileBinaryHandler.NotNull(nameof(fileBinaryHandler));
        }

        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> Get(string documentId)
        {
            await _fileBinaryHandler.Handle(documentId);
            return Ok();
        }
    }
}
