using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public class AddFileController : BaseController
    {
        private readonly IAddFileHandler _addFileHandler;
        public AddFileController(IAddFileHandler AddFileHandler)
        {
            _addFileHandler = AddFileHandler;
            _addFileHandler.NotNull(nameof(AddFileHandler));
        }

        [HttpPost]
        [Route("add-file")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AddFileDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> AddFile(string billId, string localFilePath, CancellationToken cancellation)
        {
            AddFileDto result = await _addFileHandler.Handle(billId, localFilePath, cancellation);
            return Ok(result);
        }
    }
}
