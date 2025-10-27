using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class InstallationAndEquipmentUpdateHadler : IInstallationAndEquipmentUpdateHadler
    {
        private readonly IInstallationAndEquipmentCommandService _commandService;
        private readonly IValidator<InstallationAndEquipmentUpdateDto> _validator;
        public InstallationAndEquipmentUpdateHadler(
            IInstallationAndEquipmentCommandService commandService,
            IValidator<InstallationAndEquipmentUpdateDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(InstallationAndEquipmentUpdateDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            inputDto.RegisterDateJalali = DateTime.Now.ToShortPersianDateString();
            await _commandService.Update(inputDto);
        }
    }
}
