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
    internal sealed class EmptyUnitByBillIdSummaryByZoneHandler : IEmptyUnitByBillIdSummaryByZoneHandler
    {
        private readonly IEmptyUnitByBillIdZoneGroupingQueryService _emptyUnitByBillIdZoneGroupingQueryService;
        private readonly IValidator<EmptyUnitInputDto> _validator;
        public EmptyUnitByBillIdSummaryByZoneHandler(
            IEmptyUnitByBillIdZoneGroupingQueryService emptyUnitByBillIdZoneGroupingQueryService,
            IValidator<EmptyUnitInputDto> validator)
        {
            _emptyUnitByBillIdZoneGroupingQueryService = emptyUnitByBillIdZoneGroupingQueryService;
            _emptyUnitByBillIdZoneGroupingQueryService.NotNull(nameof(emptyUnitByBillIdZoneGroupingQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdByZoneDataOutputDto>> Handle(EmptyUnitInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdByZoneDataOutputDto> emptyUnit = await _emptyUnitByBillIdZoneGroupingQueryService.Get(input);
            return emptyUnit;
        }
    }
}
