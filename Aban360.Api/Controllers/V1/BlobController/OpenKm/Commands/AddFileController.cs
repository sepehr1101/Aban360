using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
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
        public async Task<IActionResult> AddFile(AddFormFileInput input, CancellationToken cancellation)
        {
            AddFileDto result = await _addFileHandler.Handle(input, cancellation);
            return Ok(result);
        }

        [HttpPost]
        [Route("add-file-base64")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AddFileDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> AddFile(AddBase64FileInput input, CancellationToken cancellation)
        {
            AddFileDto result = await _addFileHandler.Handle(input, cancellation);
            return Ok(result);
        }
    }
}
