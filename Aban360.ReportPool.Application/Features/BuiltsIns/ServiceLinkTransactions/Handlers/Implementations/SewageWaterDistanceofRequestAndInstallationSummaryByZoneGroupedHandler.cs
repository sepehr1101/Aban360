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
    internal sealed class SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler : ISewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler
    {
        private readonly ISewageWaterDistanceofRequestAndInstallationSummaryByZoneQueryService _sewageWaterDistanceofRequestAndInstallationSummaryByZoneQuery;
        private readonly IValidator<SewageWaterDistanceofRequestAndInstallationByZoneInputDto> _validator;
        public SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler(
            ISewageWaterDistanceofRequestAndInstallationSummaryByZoneQueryService sewageWaterDistanceofRequestAndInstallationSummaryByZoneQuery,
            IValidator<SewageWaterDistanceofRequestAndInstallationByZoneInputDto> validator)
        {
            _sewageWaterDistanceofRequestAndInstallationSummaryByZoneQuery = sewageWaterDistanceofRequestAndInstallationSummaryByZoneQuery;
            _sewageWaterDistanceofRequestAndInstallationSummaryByZoneQuery.NotNull(nameof(sewageWaterDistanceofRequestAndInstallationSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, ReportOutput<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>>> Handle(SewageWaterDistanceofRequestAndInstallationByZoneInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _sewageWaterDistanceofRequestAndInstallationSummaryByZoneQuery.Get(input);

            ICollection<float> distances = new List<float>();
            foreach (var item in result.ReportData)
            {
                item.DistanceAverageText = CalculationDistanceDate.ConvertDayToDate((int)item.DistanceAverage);
                distances.Add(item.DistanceAverage);
            }
            int averageDistance = (int)distances.Sum() / (result.ReportHeader.RecordCount <= 0 ? 1 : result.ReportHeader.RecordCount);
            result.ReportHeader.AverageDistance = CalculationDistanceDate.ConvertDayToDate(averageDistance);
            result.ReportHeader.MaxDistance = CalculationDistanceDate.ConvertDayToDate(distances.Any() ? (int)distances.Max() : 0);
            result.ReportHeader.MinDistance = CalculationDistanceDate.ConvertDayToDate(distances.Any() ? (int)distances.Min() : 0);


            IEnumerable<ReportOutput<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>> dataGroup = result
                    .ReportData
                    .GroupBy(m => m.RegionTitle) // فقط بر اساس RegionId گروه‌بندی
                    .Select(g => new ReportOutput<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>
                    (
                        result.Title,
                        new SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto
                        {
                            ItemTitle = g.First().RegionTitle,
                            DistanceAverage = g.Sum(x => x.DistanceAverage),
                            DistanceMedian = g.Sum(x => x.DistanceMedian),
                        },
                        g.Select(v => new SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto
                        {
                            ItemTitle = v.ItemTitle,
                            DistanceAverage = v.DistanceAverage,
                            DistanceMedian = v.DistanceMedian,
                            DistanceAverageText = v.DistanceAverageText,
                        })
                    ))
                    .ToList();

            dataGroup.ForEach(x =>
            {
                x.ReportHeader.DistanceAverageText = CalculationDistanceDate.ConvertDayToDate((int)x.ReportHeader.DistanceAverage);
            });
            ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, ReportOutput<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
       
        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>> HandleFlat(SewageWaterDistanceofRequestAndInstallationByZoneInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, ReportOutput<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }
    }
}
