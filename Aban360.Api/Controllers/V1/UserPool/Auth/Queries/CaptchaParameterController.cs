using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("captcha")]
    [ApiController]
    public class CaptchaParameterController : BaseController
    {
        private readonly ICaptchaGetSingleHandler _captchaGetSingleHandler;
        public CaptchaParameterController(ICaptchaGetSingleHandler captchaGetSingleHandler)
        {
            _captchaGetSingleHandler = captchaGetSingleHandler;
        }

        [Route("params")]
        public async Task<IActionResult> Read(CancellationToken cancellationToken)
        {
            CaptchaSingleQueryDto captchaDtos = await _captchaGetSingleHandler.Handle(cancellationToken);
            return Ok(captchaDtos);
        }
    }
}
