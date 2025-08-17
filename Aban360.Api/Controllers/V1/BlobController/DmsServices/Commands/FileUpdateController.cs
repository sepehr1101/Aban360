using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Commands.Update.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Commands
{
    [Route("v1/dms-file-in-folder")]
    public class FileUpdateController : BaseController
    {
        private readonly IFileUpdateHandler _fileUpdateHandler;
        public FileUpdateController(IFileUpdateHandler fileUpdateHandler)
        {
            _fileUpdateHandler = fileUpdateHandler;
            _fileUpdateHandler.NotNull(nameof(fileUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> create(string nodeId,string groupName)
        {
            await _fileUpdateHandler.Handle(nodeId,groupName);
            return Ok();
        }
    }
}
