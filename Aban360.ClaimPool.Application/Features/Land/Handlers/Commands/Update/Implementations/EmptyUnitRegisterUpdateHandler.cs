using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class EmptyUnitRegisterUpdateHandler : IEmptyUnitRegisterUpdateHandler
    {
        private readonly IEmptyUnitRegisterCommandService _emptyUnitRegisterCommandService;
        private readonly IValidator<EmptyUnitRegisterUpdateDto> _validator;
        public EmptyUnitRegisterUpdateHandler(
            IEmptyUnitRegisterCommandService emptyUnitRegisterCommandService,
            IValidator<EmptyUnitRegisterUpdateDto> validator)
        {
            _emptyUnitRegisterCommandService = emptyUnitRegisterCommandService;
            _emptyUnitRegisterCommandService.NotNull(nameof(emptyUnitRegisterCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(EmptyUnitRegisterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            await _emptyUnitRegisterCommandService.Update(updateDto, DateTime.Now.ToShortPersianDateString());
        }
    }
}
