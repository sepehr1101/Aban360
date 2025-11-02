using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class InstallationAndEquipmentCreateHadler : IInstallationAndEquipmentCreateHadler
    {
        private readonly IInstallationAndEquipmentCommandService _commandService;
        private readonly IValidator<InstallationAndEquipmentCreateDto> _validator;
        public InstallationAndEquipmentCreateHadler(
            IInstallationAndEquipmentCommandService commandService,
            IValidator<InstallationAndEquipmentCreateDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(InstallationAndEquipmentCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            InstallationAndEquipmentInputDto installationAndEquipment = GetInstallationAndEquipment(inputDto, appUser.UserId);
            await _commandService.Create(installationAndEquipment);
        }
        private InstallationAndEquipmentInputDto GetInstallationAndEquipment(InstallationAndEquipmentCreateDto input, Guid userId)
        {
            return new InstallationAndEquipmentInputDto()
            {
                IsWater = input.IsWater,
                DiameterId = input.DiameterId,
                InstallationAmount = input.InstallationAmount,
                EquipmentAmount = input.EquipmentAmount,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RegisterDateTime = DateTime.Now,
                RegisterByUserId = userId
            };
        }
    }
}