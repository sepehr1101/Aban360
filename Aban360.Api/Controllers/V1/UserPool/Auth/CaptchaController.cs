using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.UserPool.Auth
{
    public class CaptchaController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly ICaptchaCreateHandler _c0_captchaCreateHandler;
        private readonly IUnitOfWork _c1_uow;
        private readonly ICaptchaUpdateHandler _c1_captchaUpdateHandler;
        private readonly ICaptchaDictionaryHandler _c2_captchaDictionaryHandler;
        private readonly ICaptchaDisplayModeQueryHandler _c3_displayModeHandler;
        private readonly ICaptchaGetSingleHandler _c4_captchaGetSingleHandler;
        private readonly ICaptchaLanguageQueryHandler _c5_languageQueryHandler;
        private readonly ICaptchaGetListHandler _c6_captchaGetListHandler;
        private readonly ICaptchaGetSingleHandler _c7_captchaGetSingleHandler;
        private readonly IDNTCaptchaApiProvider _c7_captchaApiProvider;
        public CaptchaController(IUnitOfWork c0_uow, ICaptchaCreateHandler c0_captchaCreateHandler, IUnitOfWork c1_uow, ICaptchaUpdateHandler c1_captchaUpdateHandler, ICaptchaDictionaryHandler c2_captchaDictionaryHandler, ICaptchaDisplayModeQueryHandler c3_captchaDisplayModeQueryHandler, ICaptchaGetSingleHandler c4_captchaGetSingleHandler, ICaptchaLanguageQueryHandler c5_captchaLanguageQueryHandler, ICaptchaGetListHandler c6_captchaGetListHandler, ICaptchaGetSingleHandler c7_captchaGetSingleHandler, IDNTCaptchaApiProvider c7_captchaApiProvider)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(_c0_uow));
            _c0_captchaCreateHandler = c0_captchaCreateHandler;
            _c0_captchaCreateHandler.NotNull(nameof(c0_captchaCreateHandler));
            _c1_uow = c1_uow;
            _c1_uow.NotNull(nameof(_c1_uow));
            _c1_captchaUpdateHandler = c1_captchaUpdateHandler;
            _c1_captchaUpdateHandler.NotNull(nameof(c1_captchaUpdateHandler));
            _c2_captchaDictionaryHandler = c2_captchaDictionaryHandler;
            _c2_captchaDictionaryHandler.NotNull(nameof(c2_captchaDictionaryHandler));
            _c3_displayModeHandler = c3_captchaDisplayModeQueryHandler;
            _c3_displayModeHandler.NotNull(nameof(c3_captchaDisplayModeQueryHandler));
            _c4_captchaGetSingleHandler = c4_captchaGetSingleHandler;
            _c5_languageQueryHandler = c5_captchaLanguageQueryHandler;
            _c5_languageQueryHandler.NotNull(nameof(c5_captchaLanguageQueryHandler));
            _c6_captchaGetListHandler = c6_captchaGetListHandler;
            _c7_captchaGetSingleHandler = c7_captchaGetSingleHandler;
            _c7_captchaGetSingleHandler.NotNull(nameof(c7_captchaGetSingleHandler));
            _c7_captchaApiProvider = c7_captchaApiProvider;
            _c7_captchaApiProvider.NotNull(nameof(c7_captchaApiProvider));
        }

        [Route("~/v1/captcha/create")]
        [HttpPost, HttpPatch]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CaptchaCreateDto capthcaCreateDto, CancellationToken cancellationToken)
        {
            await _c0_captchaCreateHandler.Handle(capthcaCreateDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(capthcaCreateDto);
        }

        [Route("~/v1/captcha/update")]
        [HttpPost, HttpPatch]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CaptchaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken)
        {
            await _c1_captchaUpdateHandler.Handle(capthcaUpdateDto, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
            return Ok(capthcaUpdateDto);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/captcha/dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<NumericDictionary>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromCaptchaDictionary(CancellationToken cancellationToken)
        {
            ICollection<NumericDictionary> dictionary = await _c2_captchaDictionaryHandler.Handle(cancellationToken);
            return Ok(dictionary);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/captcha/mode")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CaptchaDisplayModeDto>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromCaptchaDisplayMode(CancellationToken cancellationToken)
        {
            ICollection<CaptchaDisplayModeDto> captchaDisplayModes = await _c3_displayModeHandler.Handle(cancellationToken);
            return Ok(captchaDisplayModes);
        }

        [Route("~/v1/captcha/read/{id}")]
        [HttpGet, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaQueryDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromCaptchaGetSignleQuery(int id, CancellationToken cancellationToken)
        {
            CaptchaQueryDto captchaDtos = await _c4_captchaGetSingleHandler.Handle(id, cancellationToken);
            return Ok(captchaDtos);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/captcha/lang")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CaptchaLanguageDto>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromCaptchaLanguage(CancellationToken cancellationToken)
        {
            ICollection<CaptchaLanguageDto> languages = await _c5_languageQueryHandler.Handle(cancellationToken);
            return Ok(languages);
        }

        [Route("~/v1/captcha/read")]
        [HttpGet, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CaptchaQueryDto>>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromCaptchaList(CancellationToken cancellationToken)
        {
            ICollection<CaptchaQueryDto> captchaDtos = await _c6_captchaGetListHandler.Handle(cancellationToken);
            return Ok(captchaDtos);
        }

        [Route("~/v1/captcha/params")]
        [HttpGet, HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaParams>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromCaptchaParameter(CancellationToken cancellationToken)
        {
            CaptchaActiveDto captchaDto = await _c7_captchaGetSingleHandler.Handle(cancellationToken);
            var dntCaptchaParams = _c7_captchaApiProvider.CreateDNTCaptcha(new DNTCaptchaTagHelperHtmlAttributes { BackColor = captchaDto.BackColor, FontName = captchaDto.FontName, FontSize = captchaDto.FontSize, ForeColor = captchaDto.ForeColor, Language = (Language)captchaDto.LanguageId, DisplayMode = (DisplayMode)captchaDto.DisplayModeEnumId, Max = captchaDto.Max, Min = captchaDto.Min });
            CaptchaParams captchaParams = new CaptchaParams(dntCaptchaParams.DntCaptchaImgUrl, dntCaptchaParams.DntCaptchaTextValue);
            return Ok(captchaParams);
        }
    }
}
