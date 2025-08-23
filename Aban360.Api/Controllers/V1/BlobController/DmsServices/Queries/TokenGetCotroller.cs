using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Queries
{
    [Route("v1/dms-token")]
    public class TokenGetCotroller : BaseController
    {
        private readonly ITokenGetHandler _tokenGetHandler;
        public TokenGetCotroller(ITokenGetHandler tokenGetHandler)
        {
            _tokenGetHandler = tokenGetHandler;
            _tokenGetHandler.NotNull(nameof(tokenGetHandler));
        }

        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> Get(string fieldId)
        {
            await _tokenGetHandler.Handle();
            return Ok();
        }
    }
}
