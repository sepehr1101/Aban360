using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/captcha")]
    public class CaptchaLanguageController : BaseController
    {
        private readonly ICaptchaLanguageQueryHandler _languageQueryHandler;
        public CaptchaLanguageController(ICaptchaLanguageQueryHandler captchaLanguageQueryHandler)
        {
            _languageQueryHandler = captchaLanguageQueryHandler;
            _languageQueryHandler.NotNull(nameof(captchaLanguageQueryHandler));
        }

        [HttpGet]
        [Route("lang")]
        public async Task<IActionResult> GetCaptchaLanguages(CancellationToken cancellationToken)
        {
            var languages = await _languageQueryHandler.Handle(cancellationToken);
            return Ok(languages);
        }
    }
}
