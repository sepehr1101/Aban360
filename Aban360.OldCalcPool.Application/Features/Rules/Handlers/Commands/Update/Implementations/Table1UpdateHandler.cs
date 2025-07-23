using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Implementations
{
    internal sealed class Table1UpdateHandler : ITable1UpdateHandler
    {
        private readonly ITable1UpdateService _table1UpdateService;
        private readonly IValidator<Table1UpdateDto> _table1UpdateValidator;

        public Table1UpdateHandler(
            ITable1UpdateService table1UpdateService,
            IValidator<Table1UpdateDto> table1UpdateValidator)
        {
            _table1UpdateService = table1UpdateService;
            _table1UpdateService.NotNull(nameof(table1UpdateService));

            _table1UpdateValidator = table1UpdateValidator;
            _table1UpdateValidator.NotNull(nameof(_table1UpdateValidator));
        }
        public async Task Handle(Table1UpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _table1UpdateValidator.ValidateAsync(UpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            await _table1UpdateService.Update(UpdateDto);
        }
    }
}
