using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Queries
{
    [Route("v1/dms-files")]
    public class FilesGetCotroller : BaseController
    {
        private readonly IFilesGetHandler _filesGetHandler;
        public FilesGetCotroller(IFilesGetHandler filesGetHandler)
        {
            _filesGetHandler = filesGetHandler;
            _filesGetHandler.NotNull(nameof(filesGetHandler));
        }

        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> Get(string fieldId)
        {
            await _filesGetHandler.Handle(fieldId);
            return Ok();
        }
    }
}
