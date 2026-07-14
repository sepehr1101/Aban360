using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ConsumptionAverageManagementDetailHandler : IConsumptionAverageManagementDetailHandler
    {
        private readonly IConsumptionAverageManagementSummaryByUsageQueryService _consumptionAverageManagerQueryService;
        private readonly IValidator<ConsumptionAverageManagementSummrayInputDto> _summaryValidator;
        public ConsumptionAverageManagementDetailHandler(
            IConsumptionAverageManagementSummaryByUsageQueryService consumptionAverageManagerQueryService,
            IValidator<ConsumptionAverageManagementSummrayInputDto> summaryValidator)
        {
            _consumptionAverageManagerQueryService = consumptionAverageManagerQueryService;
            _consumptionAverageManagerQueryService.NotNull(nameof(consumptionAverageManagerQueryService));

            _summaryValidator = summaryValidator;
            _summaryValidator.NotNull(nameof(summaryValidator));
        }

        public async Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryOutputDto>> Handle(ConsumptionAverageManagementSummrayInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _summaryValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            IEnumerable<ConsumptionAverageManagementSummaryOutputDto> data = await _consumptionAverageManagerQueryService.Get(input);

            ConsumptionAverageManagementHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data?.Count() ?? 0,
                CustomerCount = data?.Count() ?? 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.ConsumptionManagerDetail,
                Consumption = data is null || !data.Any() ? 0 : data?.Average(r => r.Consumption) ?? 0,
                ConsumptionAverage = data is null || !data.Any() ? 0 : data?.Average(r => r.ConsumptionAverage) ?? 0,
            };

            ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryOutputDto> result = new(ReportLiterals.ConsumptionManagerDetail + ReportLiterals.ByCustomer, header, data);
            return result;
        }
    }
}