using Aban360.Common.Excel;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class EmptyUnitHandler : IEmptyUnitHandler
    {
        private readonly IEmptyUnitQueryService _emptyUnitQueryService;
        private readonly IValidator<EmptyUnitInputDto> _validator;
        public EmptyUnitHandler(
            IEmptyUnitQueryService emptyUnitQueryService,
            IValidator<EmptyUnitInputDto> validator)
        {
            _emptyUnitQueryService = emptyUnitQueryService;
            _emptyUnitQueryService.NotNull(nameof(emptyUnitQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>> Handle(EmptyUnitInputDto input, [Optional]CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnitQueryService.GetInfo(input);
            return emptyUnit;
        }
    }

}
