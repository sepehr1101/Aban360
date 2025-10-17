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
    internal sealed class BranchTypeChangeHistoryHandler : IBranchTypeChangeHistoryHandler
    {
        private readonly IBranchTypeChangeHistoryQueryService _branchTypeChangeHistoryQueryService;
        private readonly IValidator<BranchTypeChangeHistoryInputDto> _validator;
        public BranchTypeChangeHistoryHandler(
            IBranchTypeChangeHistoryQueryService branchTypeChangeHistoryQueryService,
            IValidator<BranchTypeChangeHistoryInputDto> validator)
        {
            _branchTypeChangeHistoryQueryService = branchTypeChangeHistoryQueryService;
            _branchTypeChangeHistoryQueryService.NotNull(nameof(branchTypeChangeHistoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>> Handle(BranchTypeChangeHistoryInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var messeState = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(messeState);
            }

            ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto> branchTypeChangeHistory = await _branchTypeChangeHistoryQueryService.GetInfo(input);
            branchTypeChangeHistory.ReportData.ForEach(d =>
            {
                d.DistanceText = CalculationDistanceDate.ConvertDayToDate(d.Distance);
            });
            return branchTypeChangeHistory;
        }
    }
}
