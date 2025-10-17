using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class DeletionStateChangeHistoryHandler : IDeletionStateChangeHistoryHandler
    {
        private readonly IDeletionStateChangeHistoryQueryService _DeletionStateChangeHistoryQueryService;
        private readonly IValidator<DeletionStateChangeHistoryInputDto> _validator;
        public DeletionStateChangeHistoryHandler(
            IDeletionStateChangeHistoryQueryService DeletionStateChangeHistoryQueryService,
            IValidator<DeletionStateChangeHistoryInputDto> validator)
        {
            _DeletionStateChangeHistoryQueryService = DeletionStateChangeHistoryQueryService;
            _DeletionStateChangeHistoryQueryService.NotNull(nameof(DeletionStateChangeHistoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>> Handle(DeletionStateChangeHistoryInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto> deletionStateChangeHistory = await _DeletionStateChangeHistoryQueryService.GetInfo(input);
            deletionStateChangeHistory.ReportData.ForEach(d =>
            { 
                d.DistanceText=CalculationDistanceDate.ConvertDayToDate(d.Distance); 
            });
            return deletionStateChangeHistory;
        }
    }
}
