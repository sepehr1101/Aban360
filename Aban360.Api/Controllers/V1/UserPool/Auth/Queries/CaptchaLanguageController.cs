using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations;
using Aban360.UserPool.Domain.Features.Auth.Entities;
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

        [HttpGet, HttpPost]
        [Route("lang")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CaptchaLanguageDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var languages = await _languageQueryHandler.Handle(cancellationToken);
            return Ok(languages);
        }
    }
}
