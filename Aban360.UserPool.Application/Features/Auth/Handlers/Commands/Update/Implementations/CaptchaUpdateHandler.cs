using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    public sealed class CaptchaUpdateHandler
    {
        private readonly ICaptchaCommandService _commandService;
        public CaptchaUpdateHandler(ICaptchaCommandService captchaCommandService)
        {
            _commandService = captchaCommandService;
            _commandService.NotNull(nameof(captchaCommandService));
        }
        public async Task Handle(CapthcaUpdateDto capthcaUpdateDto)
        {
            await _commandService.Update(capthcaUpdateDto);
        }
    }
}