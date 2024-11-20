using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("captcha")]
    [ApiController]
    public class CaptchaListController : BaseController
    {
        private readonly ICaptchaGetListHandler _captchaGetListHandler;
        public CaptchaListController(ICaptchaGetListHandler captchaGetListHandler)
        {
            _captchaGetListHandler = captchaGetListHandler;
        }

        [Route("read")]
        public async Task<IActionResult> Read([FromBody]CancellationToken cancellationToken)
        {
            var captchaDtos= await _captchaGetListHandler.Handle(cancellationToken);
            return Ok(captchaDtos);
        }
    }
}
