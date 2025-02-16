using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    public sealed class CaptchaCreateHandler : ICaptchaCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICaptchaQueryService _captchaQueryService;
        private readonly ICaptchaCommandService _captchaCommandService;
        private readonly ICaptchaLanguageQueryService _captchaLanguageQueryService;
        private readonly ICaptchaDisplayModeQueryService _captchaDisplayModeQueryService;
        public CaptchaCreateHandler(
            IMapper mapper,
            ICaptchaQueryService captchaQueryService,
            ICaptchaCommandService captchaCommandService,
            ICaptchaLanguageQueryService captchaLanguageQueryService,
            ICaptchaDisplayModeQueryService captchaDisplayModeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _captchaQueryService = captchaQueryService;
            _captchaQueryService.NotNull(nameof(captchaCommandService));

            _captchaCommandService = captchaCommandService;
            _captchaCommandService.NotNull(nameof(captchaCommandService));

            _captchaLanguageQueryService = captchaLanguageQueryService;
            _captchaLanguageQueryService.NotNull(nameof(captchaLanguageQueryService));

            _captchaDisplayModeQueryService = captchaDisplayModeQueryService;
            _captchaDisplayModeQueryService.NotNull(nameof(captchaDisplayModeQueryService));
        }
        public async Task Handle(CaptchaCreateDto captchaCreateDto, CancellationToken cancellationToken)
        {
            Validation(captchaCreateDto.CaptchaLanguageId, captchaCreateDto.CaptchaDisplayModeId);
            var captcha = _mapper.Map<Captcha>(captchaCreateDto);
            await _captchaCommandService.Create(captcha);
            if (captcha.IsSelected)
                await _captchaCommandService.SetIsSelected();
        }

        private async void Validation(short languegaId, short displayModeId)
        {
            var language = await _captchaLanguageQueryService.Get(languegaId);
            if (language == null)
                throw new InvalidIdException();

            var displayMode = await _captchaDisplayModeQueryService.Get(displayModeId);
            if (displayMode == null)
                throw new InvalidIdException();
        }
    }
}
