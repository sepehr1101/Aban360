using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries
{
    [Route("v1/open-km")]
    public class GetDownloadLinkController : BaseController
    {
        private readonly IGetDownloadLinkHandler _getDownloadLinkHandler;
        public GetDownloadLinkController(IGetDownloadLinkHandler getDownloadLinkHandler)
        {
            _getDownloadLinkHandler = getDownloadLinkHandler;
            _getDownloadLinkHandler.NotNull(nameof(getDownloadLinkHandler));
        }

        [HttpGet]
        [Route("download-link")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDirectoryTree(string uuid, bool oneTimeUse, CancellationToken cancellation)
        {
            string result = await _getDownloadLinkHandler.Handle(uuid, oneTimeUse, cancellation);
            return Ok(result);
        }
    }
}
