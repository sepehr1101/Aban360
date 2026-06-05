using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public class RemoveFileController : BaseController
    {
        private readonly IRemoveFileHandler _removeFileHandler;
        public RemoveFileController(IRemoveFileHandler removeFileHandler)
        {
            _removeFileHandler = removeFileHandler;
            _removeFileHandler.NotNull(nameof(removeFileHandler));
        }

        [HttpPost, HttpDelete]
        [Route("remove-file")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFile([FromBody] RemoveFileDto removeFileDto, CancellationToken cancellation)
        {
            await _removeFileHandler.Handle(removeFileDto, cancellation);
            return Ok(removeFileDto);
        }
    }
}
