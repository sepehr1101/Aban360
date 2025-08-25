using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Queries
{
    [Route("v1/dms-download-link")]
    public class DownloadLinkGetCotroller : BaseController
    {
        private readonly IDownloadLinkHandler _downloadLinkHandler;
        public DownloadLinkGetCotroller(IDownloadLinkHandler downloadLinkHandler)
        {
            _downloadLinkHandler = downloadLinkHandler;
            _downloadLinkHandler.NotNull(nameof(downloadLinkHandler));
        }

        [HttpPost]
        [Route("get")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string userId, bool OnTimeuse)
        {
            await _downloadLinkHandler.Handle(userId,OnTimeuse);
            return Ok();
        }
    }
}
