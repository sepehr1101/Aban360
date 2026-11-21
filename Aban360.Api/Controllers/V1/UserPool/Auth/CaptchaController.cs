using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.BaseEntities;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Aban360.Api.Filters;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth
{
    [Route("v1/captcha")]
    public class CaptchaController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly ICaptchaService _captchaService;
        private readonly IUnitOfWork _c1_uow;
        private readonly IDNTCaptchaApiProvider _c7_captchaApiProvider;
        public CaptchaController(IUnitOfWork c0_uow, ICaptchaService captchaService, IUnitOfWork c1_uow, IDNTCaptchaApiProvider c7_captchaApiProvider)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(_c0_uow));
            _captchaService = captchaService;
            _captchaService.NotNull(nameof(captchaService));
            _c1_uow = c1_uow;
            _c1_uow.NotNull(nameof(_c1_uow));
            _c7_captchaApiProvider = c7_captchaApiProvider;
            _c7_captchaApiProvider.NotNull(nameof(c7_captchaApiProvider));
        }

        [Route("create")]
        [HttpPost, HttpPatch]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CaptchaCreateDto capthcaCreateDto, CancellationToken cancellationToken)
        {
            await _captchaService.CreateAsync(capthcaCreateDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(capthcaCreateDto);
        }

        [Route("update")]
        [HttpPost, HttpPatch]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CaptchaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken)
        {
            await _captchaService.UpdateAsync(capthcaUpdateDto, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
            return Ok(capthcaUpdateDto);
        }

        [HttpGet, HttpPost]
        [Route("dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<NumericDictionary>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromCaptchaDictionary(CancellationToken cancellationToken)
        {
            ICollection<NumericDictionary> dictionary = await _captchaService.GetDictionaryAsync(cancellationToken);
            return Ok(dictionary);
        }

        [HttpGet, HttpPost]
        [Route("mode")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CaptchaDisplayModeDto>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromCaptchaDisplayMode(CancellationToken cancellationToken)
        {
            ICollection<CaptchaDisplayModeDto> captchaDisplayModes = await _captchaService.GetDisplayModesAsync(cancellationToken);
            return Ok(captchaDisplayModes);
        }

        [Route("read/{id}")]
        [HttpGet, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaQueryDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromCaptchaGetSignleQuery(int id, CancellationToken cancellationToken)
        {
            CaptchaQueryDto captchaDtos = await _captchaService.GetSingleAsync(id, cancellationToken);
            return Ok(captchaDtos);
        }

        [HttpGet, HttpPost]
        [Route("lang")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CaptchaLanguageDto>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromCaptchaLanguage(CancellationToken cancellationToken)
        {
            ICollection<CaptchaLanguageDto> languages = await _captchaService.GetLanguagesAsync(cancellationToken);
            return Ok(languages);
        }

        [Route("read")]
        [HttpGet, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CaptchaQueryDto>>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromCaptchaList(CancellationToken cancellationToken)
        {
            ICollection<CaptchaQueryDto> captchaDtos = await _captchaService.GetAllAsync(cancellationToken);
            return Ok(captchaDtos);
        }

        [Route("params")]
        [HttpGet, HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaParams>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromCaptchaParameter(CancellationToken cancellationToken)
        {
            CaptchaActiveDto captchaDto = await _captchaService.GetSingleAsync(cancellationToken);
            var dntCaptchaParams = _c7_captchaApiProvider.CreateDNTCaptcha(new DNTCaptchaTagHelperHtmlAttributes { BackColor = captchaDto.BackColor, FontName = captchaDto.FontName, FontSize = captchaDto.FontSize, ForeColor = captchaDto.ForeColor, Language = (Language)captchaDto.LanguageId, DisplayMode = (DisplayMode)captchaDto.DisplayModeEnumId, Max = captchaDto.Max, Min = captchaDto.Min });
            CaptchaParams captchaParams = new CaptchaParams(dntCaptchaParams.DntCaptchaImgUrl, dntCaptchaParams.DntCaptchaTextValue);
            return Ok(captchaParams);
        }
    }
}
