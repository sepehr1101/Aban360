using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.Common.Entities;
using Aban360.UserPool.Domain.Constants;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("login")]
    [ApiController]
    public class LoginController : BaseController
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PaceFirstStep([FromForm] FirstStepLoginInput loginInput)
        {
            if (!_captchaValidatorService.HasRequestValidCaptchaEntry())
            {
                return ClientError(MessageResources.CaptchaInvalid);
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
        public async Task<IActionResult> CreateCaptchaParams()
        {
            CaptchaDto captchaDto = await _captchaGetSingleHandler.Handle();
            var captcha = _captchaApiProvider.CreateDNTCaptcha(new DNTCaptchaTagHelperHtmlAttributes
            {
                BackColor = captchaDto.BackColor,
                Dir = captchaDto.Direction,
                FontName = captchaDto.FontName,
                FontSize = captchaDto.FontSize,
                ForeColor = captchaDto.ForeColor,
                Max = captchaDto.Max,
                Min = captchaDto.Min,
                //ShowRefreshButton=captchaDto.ShowRefreshButton,
                //TextBoxClass = captchaDto.InputClass,
                ValidationMessageClass = captchaDto.ValidationMessageClass,
                ValidationErrorMessage = string.Empty, //captchaDto.ValidationErrorMessage,
                //TextBoxTemplate=captchaDto.InputTemplate,
                //TooManyRequestsErrorMessage=captchaDto.RateLimitMessage,
                //CaptchaToken= captchaDto.HiddenTokenName,
                DisplayMode = (DisplayMode)captchaDto.DisplayModeEnumId,
                Language = (Language)captchaDto.LanguageId,
                UseRelativeUrls = false
            });
            var response = new CaptchaApiResponse(captcha.DntCaptchaImgUrl, captcha.DntCaptchaId, captcha.DntCaptchaTextValue, captcha.DntCaptchaTokenValue);
            return Ok(response);
        }
    }
}