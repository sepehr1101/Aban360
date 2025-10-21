using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;
using static Aban360.Common.Timing.CalculationDistanceDate;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class ReadingIssueDistanceBillDetailHandler : IReadingIssueDistanceBillDetailHandler
    {
        private readonly IReadingIssueDistanceBillQueryService _readingIssueDistanceBillDetailQuery;
        private readonly IValidator<ReadingIssueDistanceBillInputDto> _validator;
        public ReadingIssueDistanceBillDetailHandler(
            IReadingIssueDistanceBillQueryService readingIssueDistanceBillDetailQuery,
            IValidator<ReadingIssueDistanceBillInputDto> validator)
        {
            _readingIssueDistanceBillDetailQuery = readingIssueDistanceBillDetailQuery;
            _readingIssueDistanceBillDetailQuery.NotNull(nameof(readingIssueDistanceBillDetailQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillDataOutputDto>> Handle(ReadingIssueDistanceBillInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            int sumDistance = 0;
            ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillDataOutputDto> result = await _readingIssueDistanceBillDetailQuery.GetInfo(input);
            if (result is not null && result.ReportData is not null && result.ReportData.Any())
            {
                result.ReportData.ForEach(data =>
                {
                    CalcDistanceResultDto calcDistance = CalculationDistanceDate.CalcDistance(data.CurrentDateJalali, data.RegisterDateJalali);
                    data.DistanceText = calcDistance.DistanceText;
                    sumDistance += calcDistance.Distance;
                });

                float sumAverage = (sumDistance / result.ReportData.Count());
                int sumRoundAverage = (int)Math.Round(sumAverage);
                result.ReportHeader.AverageAll = sumAverage;
                result.ReportHeader.AverageAllText = CalculationDistanceDate.ConvertDayToDate(sumRoundAverage);
            }

            return result;
        }
    }
}
