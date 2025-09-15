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
    internal sealed class ReadingIssueDistanceBillSummaryByZoneHandler : IReadingIssueDistanceBillSummaryByZoneHandler
    {
        private readonly IReadingIssueDistanceBillSummaryByZoneQueryService _readingIssueDistanceBillSummaryByZoneQuery;
        private readonly IValidator<ReadingIssueDistanceBillInputDto> _validator;
        public ReadingIssueDistanceBillSummaryByZoneHandler(
            IReadingIssueDistanceBillSummaryByZoneQueryService readingIssueDistanceBillSummaryByZoneQuery,
            IValidator<ReadingIssueDistanceBillInputDto> validator)
        {
            _readingIssueDistanceBillSummaryByZoneQuery = readingIssueDistanceBillSummaryByZoneQuery;
            _readingIssueDistanceBillSummaryByZoneQuery.NotNull(nameof(readingIssueDistanceBillSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryByZoneDataOutputDto>> Handle(ReadingIssueDistanceBillInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryByZoneDataOutputDto> result = await _readingIssueDistanceBillSummaryByZoneQuery.GetInfo(input);
            result.ReportData.ForEach(r =>
            {
                r.UnSpecifiedText =  CalculationDistanceDate.ConvertDaysToDate(r.UnSpecified);
                r.Field0_5Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field0_5);
                r.Field0_75Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field0_75);
                r.Field1Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field1);
                r.Field1_2Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field1_2);
                r.Field1_5Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field1_5);
                r.Field2Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field2);
                r.Field3Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field3);
                r.Field4Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field4);
                r.Field5Text =  CalculationDistanceDate.ConvertDaysToDate(r.Field5);
                r.MoreThan6Text =  CalculationDistanceDate.ConvertDaysToDate(r.MoreThan6);
            });
            return result;
        }
    }
}
