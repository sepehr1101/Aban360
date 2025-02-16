using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/captcha")]
    [ApiController]
    public class CaptchaParameterController : BaseController
    {
        private readonly ICaptchaGetSingleHandler _captchaGetSingleHandler;
        private readonly IDNTCaptchaApiProvider _captchaApiProvider;
        public CaptchaParameterController(
            ICaptchaGetSingleHandler captchaGetSingleHandler,
            IDNTCaptchaApiProvider captchaApiProvider)
        {
            _captchaGetSingleHandler = captchaGetSingleHandler;
            _captchaGetSingleHandler.NotNull(nameof(captchaGetSingleHandler));

            _captchaApiProvider = captchaApiProvider;
            _captchaApiProvider.NotNull(nameof(captchaApiProvider));
        }

        [Route("params")]
        [HttpGet, HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaParams>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Read(CancellationToken cancellationToken)
        {
            CaptchaActiveDto captchaDto = await _captchaGetSingleHandler.Handle(cancellationToken);
            var dntCaptchaParams = _captchaApiProvider.CreateDNTCaptcha(new DNTCaptchaTagHelperHtmlAttributes
            {
                BackColor = captchaDto.BackColor,
                FontName = captchaDto.FontName,
                FontSize = captchaDto.FontSize,
                ForeColor = captchaDto.ForeColor,
                Language = (Language)captchaDto.LanguageId,
                DisplayMode = (DisplayMode)captchaDto.DisplayModeEnumId,
                Max = captchaDto.Max,
                Min = captchaDto.Min
            });
            CaptchaParams captchaParams = new CaptchaParams(dntCaptchaParams.DntCaptchaImgUrl, dntCaptchaParams.DntCaptchaTextValue);
            return Ok(captchaParams);
        }
    }
}
