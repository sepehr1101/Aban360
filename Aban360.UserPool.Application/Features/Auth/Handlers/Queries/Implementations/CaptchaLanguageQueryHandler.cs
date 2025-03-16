using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class CaptchaLanguageQueryHandler : ICaptchaLanguageQueryHandler
    {
        private readonly ICaptchaLanguageQueryService _captchaLanguageQueryService;
        private readonly IMapper _mapper;
        public CaptchaLanguageQueryHandler(
            ICaptchaLanguageQueryService captchaLanguageQueryService,
            IMapper mapper)
        {
            _captchaLanguageQueryService = captchaLanguageQueryService;
            _captchaLanguageQueryService.NotNull(nameof(captchaLanguageQueryService));

            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));
        }
        public async Task<ICollection<CaptchaLanguageDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<CaptchaLanguage> captchaLanguages = await _captchaLanguageQueryService.Get();
            ICollection<CaptchaLanguageDto> captchaLanguageDtos = _mapper.Map<ICollection<CaptchaLanguageDto>>(captchaLanguages);
            return captchaLanguageDtos;
        }
    }
}
