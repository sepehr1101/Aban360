using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Aban360.BlobPool.Application.Features.OpenKm.Services;
using Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands
{
    [Route("v1/open-km")]
    public class AddOrUpdateMetaDataController : BaseController
    {
        private readonly IOpenKmFoldersMetadataService _openKmFoldersMetadataService;
        public AddOrUpdateMetaDataController(IOpenKmFoldersMetadataService openKmFoldersMetadataService)
        {
            _openKmFoldersMetadataService = openKmFoldersMetadataService;
            _openKmFoldersMetadataService.NotNull(nameof(openKmFoldersMetadataService));
        }

        [HttpPost]
        [Route("add-update-meta-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdateMetaData(AddOrUpdateMetaDataDto inputDto, string uuid, CancellationToken cancellation)
        {
            await _openKmFoldersMetadataService.AddOrUpdateMetaDataAsync(inputDto, uuid, cancellation);
            return Ok(uuid);
        }
    }
}
