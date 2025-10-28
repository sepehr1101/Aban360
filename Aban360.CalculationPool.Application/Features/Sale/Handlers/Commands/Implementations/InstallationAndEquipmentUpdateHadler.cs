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

        public async Task Handle(InstallationAndEquipmentUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            InstallationAndEquipmentInputDto installationAndEquipmentCreate = GetInstallationAndEquipment(inputDto, appUser.UserId);
            DeleteDto installationAndEquipmentDelete = new(inputDto.Id, DateTime.Now, appUser.UserId);
            await _commandService.Update(installationAndEquipmentCreate, installationAndEquipmentDelete);
        }
        private InstallationAndEquipmentInputDto GetInstallationAndEquipment(InstallationAndEquipmentUpdateDto input, Guid userId)
        {
            return new InstallationAndEquipmentInputDto()
            {
                IsWater = input.IsWater,
                MeterDiameterId = input.MeterDiameterId,
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
