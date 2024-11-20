using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("captcha")]
    [ApiController]
    public class CaptchaParameterController : BaseController
    {
        private readonly IDNTCaptchaApiProvider _captchaApiProvider;
        private readonly ICaptchaGetListHandler _captchaGetListHandler;
        private readonly ICaptchaGetSingleHandler _captchaGetSingleHandler;
        
        public CaptchaParameterController(
            IDNTCaptchaApiProvider captchaApiProvider,
            ICaptchaGetSingleHandler captchaGetSingleHandler,
            ICaptchaGetListHandler captchaGetListHandler)
        {
            _captchaApiProvider = captchaApiProvider;
            _captchaApiProvider.NotNull(nameof(_captchaApiProvider));

            _captchaGetSingleHandler = captchaGetSingleHandler;
            _captchaGetSingleHandler.NotNull(nameof(_captchaGetSingleHandler));

            _captchaGetListHandler = captchaGetListHandler;
            _captchaGetListHandler.NotNull(nameof(_captchaGetListHandler));
        }

        [AllowAnonymous]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [Route("params")]
        public async Task<IActionResult> CreateCaptchaParams()
        {
            CaptchaQueryDto captchaDto = await _captchaGetSingleHandler.Handle();
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

        public async Task<IActionResult> Read()
        {
            var captchaDtos= await _captchaGetSingleHandler.Handle();
            return Ok(captchaDtos);
        }

        public async Task<IActionResult> Update([FromBody] CapthcaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken)
        {
            await _cap
        }
    }
}
