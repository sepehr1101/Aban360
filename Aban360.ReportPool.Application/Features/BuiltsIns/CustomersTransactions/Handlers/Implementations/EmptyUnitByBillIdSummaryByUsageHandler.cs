using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class EmptyUnitByBillIdSummaryByUsageHandler : IEmptyUnitByBillIdSummaryByUsageHandler
    {
        private readonly IEmptyUnitByBillIdUsageGroupingQueryService _emptyUnitByBillIdUsageGroupingQueryService;
        private readonly IValidator<EmptyUnitByBillInputDto> _validator;
        public EmptyUnitByBillIdSummaryByUsageHandler(
            IEmptyUnitByBillIdUsageGroupingQueryService emptyUnitByBillIdUsageGroupingQueryService,
            IValidator<EmptyUnitByBillInputDto> validator)
        {
            _emptyUnitByBillIdUsageGroupingQueryService = emptyUnitByBillIdUsageGroupingQueryService;
            _emptyUnitByBillIdUsageGroupingQueryService.NotNull(nameof(emptyUnitByBillIdUsageGroupingQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdSummaryDataOutputDto>> Handle(EmptyUnitByBillInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdSummaryDataOutputDto> emptyUnit = await _emptyUnitByBillIdUsageGroupingQueryService.Get(input);
            return emptyUnit;
        }
    }
}
