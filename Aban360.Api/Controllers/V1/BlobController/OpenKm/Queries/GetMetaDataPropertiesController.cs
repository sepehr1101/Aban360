using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries
{
    [Route("v1/open-km")]
    public class GetMetaDataPropertiesController : BaseController
    {
        private readonly IGetMetaDataPropertiesHandler _getMetaDataPropertiesHandler;
        public GetMetaDataPropertiesController(IGetMetaDataPropertiesHandler getMetaDataPropertiesHandler)
        {
            _getMetaDataPropertiesHandler = getMetaDataPropertiesHandler;
            _getMetaDataPropertiesHandler.NotNull(nameof(getMetaDataPropertiesHandler));
        }

        [HttpGet]
        [Route("metadata")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MetaDataProperties>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDirectoryTree(string input, CancellationToken cancellation)
        {
            MetaDataProperties result = await _getMetaDataPropertiesHandler.Handle(input, cancellation);
            return Ok(result);
        }
    }
}
