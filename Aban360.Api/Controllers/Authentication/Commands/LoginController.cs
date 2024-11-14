using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.Api.Controllers.Authentication.Commands
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDNTCaptchaApiProvider _captchaApiProvider;
        private readonly IDNTCaptchaValidatorService _captchaValidatorService;
        private readonly DNTCaptchaOptions _captchaOptions;

        public LoginController(
            IDNTCaptchaApiProvider captchaApiProvider,
            IDNTCaptchaValidatorService captchaValidatorService,
            IOptions<DNTCaptchaOptions> captchaOptions)
        {
            _captchaApiProvider = captchaApiProvider;
            _captchaApiProvider.NotNull();

            _captchaValidatorService = captchaValidatorService;
            _captchaValidatorService.NotNull();

            _captchaOptions = captchaOptions.Value;
            _captchaOptions.NotNull();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("first-step")]
        //[ProducesResponseType(typeof(LoginOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> PaceFirstStep([FromForm] FirstStepLoginInput loginInput)
        {
            if (!_captchaValidatorService.HasRequestValidCaptchaEntry())
            {
                return BadRequest(_captchaOptions.CaptchaComponent);
            }
            return Ok(_captchaOptions.CaptchaComponent);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("second-step")]
        //[ProducesResponseType(typeof(LoginOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> PaceSecondStep()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [Route("captcha")]
        public ActionResult<CaptchaApiResponse> CreateCaptchaParams()
        {
            // Note: For security reasons, a JavaScript client shouldn't be able to provide these attributes directly.
            // Otherwise an attacker will be able to change them and make them easier!
            var captcha = _captchaApiProvider.CreateDNTCaptcha(new DNTCaptchaTagHelperHtmlAttributes
            {
                BackColor = "#f7f3f3",
                FontName = "Tahoma",
                FontSize = 18,
                ForeColor = "#111111",
                Language = Language.English,
                DisplayMode = DisplayMode.SumOfTwoNumbers,
                Max = 90,
                Min = 1,
                Dir="ltr"                
            });
            var response = new CaptchaApiResponse(captcha.DntCaptchaImgUrl, captcha.DntCaptchaId, captcha.DntCaptchaTextValue, captcha.DntCaptchaTokenValue);
            return response;
        }
    }
}
