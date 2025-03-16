using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class CaptchaDisplayModeQueryHandler : ICaptchaDisplayModeQueryHandler
    {
        private readonly ICaptchaDisplayModeQueryService _captchaDisplayModeQueryService;
        private readonly IMapper _mapper;
        public CaptchaDisplayModeQueryHandler(
            ICaptchaDisplayModeQueryService captchaDisplayModeQueryService,
            IMapper mapper)
        {
            _captchaDisplayModeQueryService = captchaDisplayModeQueryService;
            _captchaDisplayModeQueryService.NotNull(nameof(captchaDisplayModeQueryService));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
        }
        public async Task<ICollection<CaptchaDisplayModeDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<CaptchaDisplayMode> captchaDisplayModes = await _captchaDisplayModeQueryService.Get();
            ICollection<CaptchaDisplayModeDto> captchaDisplayModeDtos = _mapper.Map<ICollection<CaptchaDisplayModeDto>>(captchaDisplayModes);
            return captchaDisplayModeDtos;
        }
    }
}
