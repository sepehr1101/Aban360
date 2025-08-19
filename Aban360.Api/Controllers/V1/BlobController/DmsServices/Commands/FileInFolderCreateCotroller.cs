using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Commands.Create.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Commands
{
    [Route("v1/dms-file-in-folder")]
    public class FileInFolderCreateCotroller:BaseController
    {
        private readonly IFileInFolderCreateHandler _fileInFolderCreateHandler;
        public FileInFolderCreateCotroller(IFileInFolderCreateHandler fileInFolderCreateHandler)
        {
            _fileInFolderCreateHandler = fileInFolderCreateHandler;
            _fileInFolderCreateHandler.NotNull(nameof(fileInFolderCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> create(string serverPath, string localFilePath)
        {
            await _fileInFolderCreateHandler.Handle(serverPath, localFilePath);
            return Ok();
        }
    }
}
