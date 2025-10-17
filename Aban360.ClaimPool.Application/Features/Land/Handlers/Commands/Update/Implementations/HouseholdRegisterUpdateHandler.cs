using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class HouseholdRegisterUpdateHandler : IHouseholdRegisterUpdateHandler
    {
        private readonly IHouseholdRegisterCommandService _householdRegisterCommandService;
        private readonly IValidator<HouseholdRegisterUpdateDto> _validator;
        public HouseholdRegisterUpdateHandler(
            IHouseholdRegisterCommandService householdRegisterCommandService,
            IValidator<HouseholdRegisterUpdateDto> validator)
        {
            _householdRegisterCommandService = householdRegisterCommandService;
            _householdRegisterCommandService.NotNull(nameof(householdRegisterCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(HouseholdRegisterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            await _householdRegisterCommandService.Update(updateDto, DateTime.Now.ToShortPersianDateString());
        }
    }
}
