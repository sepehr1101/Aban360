using Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class RepairDeleteHandler : IRepairDeleteHandler
    {
        private readonly IRepairCommandService _commandService;
        private readonly IValidator<RepairDeleteDto> _validator;
        public RepairDeleteHandler(
            IRepairCommandService commandService,
            IValidator<RepairDeleteDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(RepairDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(deleteDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            await _commandService.Delete(deleteDto);
        }
    }
}
