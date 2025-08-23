using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Commands.Create.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Commands
{
    [Route("v1/dms-folder")]
    public class FolderCreateCotroller : BaseController
    {
        private readonly IFolderCreateHandler _folderCreateHandler;
        public FolderCreateCotroller(IFolderCreateHandler folderCreateHandler)
        {
            _folderCreateHandler = folderCreateHandler;
            _folderCreateHandler.NotNull(nameof(folderCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> create(string folderPath)
        {
            await _folderCreateHandler.Handle(folderPath);
            return Ok();
        }
    }
}
