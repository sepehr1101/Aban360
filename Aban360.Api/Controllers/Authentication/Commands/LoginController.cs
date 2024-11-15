using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.Api.Controllers.Authentication.Commands
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDNTCaptchaApiProvider _captchaApiProvider;
        private readonly IDNTCaptchaValidatorService _captchaValidatorService;
        private readonly DNTCaptchaOptions _captchaOptions;
        private readonly ICaptchaGetSingleHandler _captchaGetSingleHandler;

        public LoginController(
            IDNTCaptchaApiProvider captchaApiProvider,
            IDNTCaptchaValidatorService captchaValidatorService,
            IOptions<DNTCaptchaOptions> captchaOptions,
            ICaptchaGetSingleHandler captchaGetSingleHandler)
        {
            _captchaApiProvider = captchaApiProvider;
            _captchaApiProvider.NotNull(nameof(_captchaApiProvider));

            _captchaValidatorService = captchaValidatorService;
            _captchaValidatorService.NotNull(nameof(_captchaValidatorService));

            _captchaOptions = captchaOptions.Value;
            _captchaOptions.NotNull(nameof(_captchaOptions));

            _captchaGetSingleHandler = captchaGetSingleHandler;
            _captchaGetSingleHandler.NotNull(nameof(_captchaGetSingleHandler));
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
        public async Task<ActionResult<CaptchaApiResponse>> CreateCaptchaParams()
        {
            CaptchaDto captchaDto = await _captchaGetSingleHandler.Handle();

            var captcha = _captchaApiProvider.CreateDNTCaptcha(new DNTCaptchaTagHelperHtmlAttributes
            {
                //BackColor = "#f7f3f3",
                //FontName = "Tahoma",
                //FontSize = 18,
                //ForeColor = "#111111",
                //Language = Language.English,
                //DisplayMode = DisplayMode.SumOfTwoNumbersToWords,
                //Max = 90,
                //Min = 1,
                //Dir="ltr"
                BackColor = captchaDto.BackColor,
                Dir = "ltr",
                FontName = captchaDto.FontName,
                FontSize = captchaDto.FontSize,
                ForeColor = captchaDto.ForeColor,
                Placeholder = string.Empty,//captchaDto.Placeholder,
                RefreshButtonClass = captchaDto.RefreshButtonClass,
                Max= 99,
                Min=1,
                ShowRefreshButton=captchaDto.ShowRefreshButton,
                TextBoxClass = captchaDto.InputClass,
                ValidationMessageClass = captchaDto.ValidationMessageClass,
                ValidationErrorMessage= string.Empty, //captchaDto.ValidationErrorMessage,
                TextBoxTemplate=captchaDto.InputTemplate,
                TooManyRequestsErrorMessage=captchaDto.RateLimitMessage,
                CaptchaToken= captchaDto.HiddenTokenName,
                DisplayMode=DisplayMode.ShowDigits,
                Language=Language.English,
                UseRelativeUrls=false,
            });
            var response = new CaptchaApiResponse(captcha.DntCaptchaImgUrl, captcha.DntCaptchaId, captcha.DntCaptchaTextValue, captcha.DntCaptchaTokenValue);
            return response;
        }
    }
}
