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
    internal sealed class UsageChangeHistoryHandler : IUsageChangeHistoryHandler
    {
        private readonly IUsageChangeHistoryQueryService _usageChangeHistoryQueryService;
        private readonly IValidator<UsageChangeHistoryInputDto> _validator;
        public UsageChangeHistoryHandler(
            IUsageChangeHistoryQueryService usageChangeHistoryQueryService,
            IValidator<UsageChangeHistoryInputDto> validator)
        {
            _usageChangeHistoryQueryService = usageChangeHistoryQueryService;
            _usageChangeHistoryQueryService.NotNull(nameof(usageChangeHistoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UsageChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>> Handle(UsageChangeHistoryInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<UsageChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto> usageChangeHistory = await _usageChangeHistoryQueryService.GetInfo(input);
            usageChangeHistory.ReportData.ForEach(d =>
            {
                d.DistanceText = CalculationDistanceDate.ConvertDaysToDate(d.Distance);
            });
            return usageChangeHistory;
        }
    }
}
