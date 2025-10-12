using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class ReadingIssueDistanceBillSummaryByUsageHandler : IReadingIssueDistanceBillSummaryByUsageHandler
    {
        private readonly IReadingIssueDistanceBillSummaryByUsageQueryService _readingIssueDistanceBillSummaryByUsageQuery;
        private readonly IValidator<ReadingIssueDistanceBillInputDto> _validator;
        public ReadingIssueDistanceBillSummaryByUsageHandler(
            IReadingIssueDistanceBillSummaryByUsageQueryService readingIssueDistanceBillSummaryByUsageQuery,
            IValidator<ReadingIssueDistanceBillInputDto> validator)
        {
            _readingIssueDistanceBillSummaryByUsageQuery = readingIssueDistanceBillSummaryByUsageQuery;
            _readingIssueDistanceBillSummaryByUsageQuery.NotNull(nameof(readingIssueDistanceBillSummaryByUsageQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>> Handle(ReadingIssueDistanceBillInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryDataOutputDto> result = await _readingIssueDistanceBillSummaryByUsageQuery.GetInfo(input);
            result.ReportData.ForEach(r =>
            {
                r.UnSpecifiedText =  CalculationDistanceDate.ConvertDayToDate((int)r.UnSpecified);
                r.Field0_5Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field0_5);
                r.Field0_75Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field0_75);
                r.Field1Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field1);
                r.Field1_2Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field1_2);
                r.Field1_5Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field1_5);
                r.Field2Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field2);
                r.Field3Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field3);
                r.Field4Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field4);
                r.Field5Text =  CalculationDistanceDate.ConvertDayToDate((int)r.Field5);
                r.MoreThan6Text =  CalculationDistanceDate.ConvertDayToDate((int)r.MoreThan6);
            });
            return result;
        }
    }
}
