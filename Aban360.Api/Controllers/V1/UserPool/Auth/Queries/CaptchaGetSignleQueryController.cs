using Aban360.Common.Categories.ApiResponse;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/captcha")]
    [ApiController]
    public class CaptchaGetSignleQueryController : BaseController
    {
        private readonly ICaptchaGetSingleHandler _captchaGetSingleHandler;
        public CaptchaGetSignleQueryController(ICaptchaGetSingleHandler captchaGetSingleHandler)
        {
            _captchaGetSingleHandler = captchaGetSingleHandler;
        }

        [Route("read/{id}")]
        [HttpGet, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaQueryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Read(int id,CancellationToken cancellationToken)
        {
           CaptchaQueryDto captchaDtos = await _captchaGetSingleHandler.Handle(id,cancellationToken);
            return Ok(captchaDtos);
        }
    }
}
