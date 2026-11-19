using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Application.Features.OpenKm.Services;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries
{
    [Route("v1/open-km")]
    public class GetMetaDataPropertiesController : BaseController
    {
        private readonly IOpenKmFoldersMetadataService _openKmFoldersMetadataService;
        public GetMetaDataPropertiesController(IOpenKmFoldersMetadataService openKmFoldersMetadataService)
        {
            _openKmFoldersMetadataService = openKmFoldersMetadataService;
            _openKmFoldersMetadataService.NotNull(nameof(openKmFoldersMetadataService));
        }

        [HttpGet, HttpPost]
        [Route("metadata")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MetaDataOutput>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDirectoryTree(string input, CancellationToken cancellation)
        {
            ICollection<MetaDataOutput> result = await _openKmFoldersMetadataService.GetMetaDataPropertiesAsync(input, false, cancellation);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("file-title")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileTitle(string input, CancellationToken cancellation)
        {
            ICollection<MetaDataOutput> result = await _openKmFoldersMetadataService.GetMetaDataPropertiesAsync(input, true, cancellation);
            return Ok(result.First().ValueTitle);
        }
    }
}
