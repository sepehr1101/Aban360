using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public class CreateFolderController : BaseController
    {
        private readonly ICreateFolderHandler _createFolderHandler;
        public CreateFolderController(ICreateFolderHandler createFolderHandler)
        {
            _createFolderHandler = createFolderHandler;
            _createFolderHandler.NotNull(nameof(createFolderHandler));
        }

        [HttpGet]
        [Route("create-folder")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateFolder(string folderName, CancellationToken cancellation)
        {
            string uuid = await _createFolderHandler.Handle(folderName, cancellation);
            return Ok(uuid);
        }
    }
}
