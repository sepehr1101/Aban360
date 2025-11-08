using Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class RepairUpdateHandler : IRepairUpdateHandler
    {
        private readonly IRepairCommandService _commandService;
        private readonly IValidator<RepairUpdateDto> _validator;
        public RepairUpdateHandler(
            IRepairCommandService commandService,
            IValidator<RepairUpdateDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(RepairUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            decimal sum = updateDto.AbonFas + updateDto.FasBaha + updateDto.AbBaha + updateDto.Ztadil + updateDto.Shahrdari + updateDto.AbonAb + updateDto.ZaribFasl + updateDto.Ab10 + updateDto.Ab20 + updateDto.Zabresani + updateDto.TabAbnA + updateDto.TabAbnF + updateDto.TabsFa + updateDto.Bodjeh + updateDto.Avarez;
            updateDto.Jam = sum;
            updateDto.Pard = sum;
            updateDto.Baha = sum;
            await _commandService.Update(updateDto);
        }
    }
}
