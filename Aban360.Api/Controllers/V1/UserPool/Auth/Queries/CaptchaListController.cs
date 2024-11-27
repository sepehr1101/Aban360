using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Read(CancellationToken cancellationToken)
        {
            ICollection<CaptchaListQueryDto> captchaDtos = await _captchaGetListHandler.Handle(cancellationToken);
            return Ok(captchaDtos);
        }
    }
}
