using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/captcha")]
    public class CaptchaDisplayModeController : BaseController
    {
        private readonly ICaptchaDisplayModeQueryHandler _displayModeHandler;
        public CaptchaDisplayModeController(ICaptchaDisplayModeQueryHandler captchaDisplayModeQueryHandler)
        {
            _displayModeHandler = captchaDisplayModeQueryHandler;
            _displayModeHandler.NotNull(nameof(captchaDisplayModeQueryHandler));
        }

        [HttpGet, HttpPost]
        [Route("mode")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CaptchaDisplayModeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            ICollection<CaptchaDisplayModeDto> captchaDisplayModes = await _displayModeHandler.Handle(cancellationToken);
            return Ok(captchaDisplayModes);
        }
    }
}
