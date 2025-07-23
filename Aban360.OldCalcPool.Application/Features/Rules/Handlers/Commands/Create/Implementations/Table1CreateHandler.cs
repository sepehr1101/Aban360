using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Implementations
{
    internal sealed class Table1CreateHandler : ITable1CreateHandler
    {
        private readonly ITable1CreateService _table1CreateService;
        private readonly IValidator<Table1CreateDto> _table1CreateValidator;

        public Table1CreateHandler(
            ITable1CreateService table1CreateService,
            IValidator<Table1CreateDto> table1CreateValidator)
        {
            _table1CreateService = table1CreateService;
            _table1CreateService.NotNull(nameof(table1CreateService));

            _table1CreateValidator = table1CreateValidator;
            _table1CreateValidator.NotNull(nameof(_table1CreateValidator));
        }
        public async Task Handle(Table1CreateDto CreateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _table1CreateValidator.ValidateAsync(CreateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            await _table1CreateService.Create(CreateDto);
        }
    }
}
