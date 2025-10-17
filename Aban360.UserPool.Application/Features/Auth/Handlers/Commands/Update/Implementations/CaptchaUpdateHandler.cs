using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    public sealed class CaptchaUpdateHandler : ICaptchaUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICaptchaCommandService _commandService;
        private readonly IValidator<CaptchaUpdateDto> _captchaValidator;
        public CaptchaUpdateHandler(
            IMapper mapper,
            ICaptchaCommandService captchaCommandService,
            IValidator<CaptchaUpdateDto> captchaValidator
)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = captchaCommandService;
            _commandService.NotNull(nameof(captchaCommandService));

            _captchaValidator = captchaValidator;
            _captchaValidator.NotNull(nameof(captchaValidator));
        }
        public async Task Handle(CaptchaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _captchaValidator.ValidateAsync(capthcaUpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }//

            Captcha captcha = _mapper.Map<Captcha>(capthcaUpdateDto);
            _commandService.Update(captcha);
            if(captcha.IsSelected )
                await _commandService.SetIsSelected(captcha.Id);
        }
    }
}