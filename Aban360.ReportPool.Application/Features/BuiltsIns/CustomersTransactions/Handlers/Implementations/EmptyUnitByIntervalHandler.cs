using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class EmptyUnitByIntervalHandler : IEmptyUnitByIntervalHandler
    {
        private readonly IEmptyUnitByIntervalQueryService _emptyUnitByIntervalQueryService;
        private readonly IValidator<EmptyUnitByIntervalInputDto> _validator;
        public EmptyUnitByIntervalHandler(
            IEmptyUnitByIntervalQueryService emptyUnitByIntervalQueryService,
            IValidator<EmptyUnitByIntervalInputDto> validator)
        {
            _emptyUnitByIntervalQueryService = emptyUnitByIntervalQueryService;
            _emptyUnitByIntervalQueryService.NotNull(nameof(emptyUnitByIntervalQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>> Handle(EmptyUnitByIntervalInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnitByIntervalQueryService.GetInfo(input);
            return emptyUnit;
        }
    }
}
