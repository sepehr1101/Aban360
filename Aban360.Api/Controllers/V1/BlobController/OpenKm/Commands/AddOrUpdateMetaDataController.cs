using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public class AddOrUpdateMetaDataController : BaseController
    {
        private readonly IAddOrUpdateMetaDataHandler _addOrUpdateMetaDataHandler;
        public AddOrUpdateMetaDataController(IAddOrUpdateMetaDataHandler AddOrUpdateMetaDataHandler)
        {
            _addOrUpdateMetaDataHandler = AddOrUpdateMetaDataHandler;
            _addOrUpdateMetaDataHandler.NotNull(nameof(AddOrUpdateMetaDataHandler));
        }

        [HttpPost]
        [Route("add-update-meta-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdateMetaData(AddOrUpdateMetaDataDto inputDto, string uuid, CancellationToken cancellation)
        {
            await _addOrUpdateMetaDataHandler.Handle(inputDto, uuid, cancellation);
            return Ok(uuid);
        }
    }
}
