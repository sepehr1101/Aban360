using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    public sealed class CaptchaUpdateHandler : ICaptchaUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICaptchaCommandService _commandService;

        public CaptchaUpdateHandler(
            IMapper mapper,
            ICaptchaCommandService captchaCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = captchaCommandService;
            _commandService.NotNull(nameof(captchaCommandService));
        }
        public async Task Handle(CaptchaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken)
        {
            Captcha captcha = _mapper.Map<Captcha>(capthcaUpdateDto);
            _commandService.Update(captcha);
            if(captcha.IsSelected )
                await _commandService.SetIsSelected(captcha.Id);
        }
    }
}