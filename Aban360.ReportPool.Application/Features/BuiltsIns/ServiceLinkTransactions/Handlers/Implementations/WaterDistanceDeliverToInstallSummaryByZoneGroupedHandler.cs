using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class WaterDistanceDeliverToInstallSummaryByZoneGroupedHandler : IWaterDistanceDeliverToInstallSummaryByZoneGroupedHandler
    {
        private readonly IWaterDistanceDeliverToInstallSummaryQueryService _waterDistanceDeliverToInstallSummaryByZoneQuery;
        private readonly IValidator<WaterDistanceDeliverToInstallInputDto> _validator;
        public WaterDistanceDeliverToInstallSummaryByZoneGroupedHandler(
            IWaterDistanceDeliverToInstallSummaryQueryService waterDistanceDeliverToInstallSummaryByZoneQuery,
            IValidator<WaterDistanceDeliverToInstallInputDto> validator)
        {
            _waterDistanceDeliverToInstallSummaryByZoneQuery = waterDistanceDeliverToInstallSummaryByZoneQuery;
            _waterDistanceDeliverToInstallSummaryByZoneQuery.NotNull(nameof(waterDistanceDeliverToInstallSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, ReportOutput<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>>> Handle(WaterDistanceDeliverToInstallInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto> result = await _waterDistanceDeliverToInstallSummaryByZoneQuery.Get(input,ReportLiterals.ZoneTitle);

            ICollection<float> distances = new List<float>();
            foreach (var item in result.ReportData)
            {
                item.DistanceAverageText = CalculationDistanceDate.ConvertDayToDate((int)item.DistanceAverage);
                distances.Add(item.DistanceAverage);
            }
            result.ReportHeader.MaxDistance = CalculationDistanceDate.ConvertDayToDate(distances.Any() ? (int)distances.Max() : 0);
            result.ReportHeader.MinDistance = CalculationDistanceDate.ConvertDayToDate(distances.Any() ? (int)distances.Min() : 0);

            IEnumerable<ReportOutput<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>> dataGroup = result
                    .ReportData
                    .GroupBy(m => m.RegionTitle) // فقط بر اساس RegionId گروه‌بندی
                    .Select(g => new ReportOutput<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>
                    (
                        result.Title,
                        new SewageWaterDistanceSummaryByZoneGroupedDataOutputDto
                        {
                            ItemTitle = g.First().RegionTitle,
                            //DistanceAverage = g.Sum(x => x.DistanceAverage),
                            DistanceMedian = g.Sum(x => x.DistanceMedian),
                            CustomerCount = g.Sum(x => x.CustomerCount),
                        },
                        g.Select(v => new SewageWaterDistanceSummaryByZoneGroupedDataOutputDto
                        {
                            ItemTitle = v.ItemTitle,
                            DistanceAverage = v.DistanceAverage,
                            DistanceMedian = v.DistanceMedian,
                            DistanceAverageText = v.DistanceAverageText,
                            CustomerCount = v.CustomerCount,
                        })
                    ))
                    .ToList();

            float sumAllRegionDistance = 0;
            foreach (var group in dataGroup)
            {
                float sumDistance = 0;
                int sumBillCounts = 0;
                foreach (var zones in group.ReportData)
                {
                    float weight = zones.CustomerCount * zones.DistanceAverage;
                    sumDistance += weight;
                    sumBillCounts += zones.CustomerCount;
                }
                float weightAverege = (float)Math.Round(sumDistance / sumBillCounts, 2);
                int round = (int)Math.Round(weightAverege);
                group.ReportHeader.DistanceAverage = weightAverege;
                group.ReportHeader.DistanceAverageText = CalculationDistanceDate.ConvertDayToDate(round);

                sumAllRegionDistance += sumDistance;

            }
            float averageDistance = (float)Math.Round(sumAllRegionDistance / result.ReportHeader.CustomerCount, 2);
            int allZoneRound = (int)Math.Round(averageDistance);
            result.ReportHeader.AverageDistanceNumber = averageDistance;
            result.ReportHeader.AverageDistance = CalculationDistanceDate.ConvertDayToDate(allZoneRound);

            ReportOutput<SewageWaterDistanceHeaderOutputDto, ReportOutput<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }

        public async Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>> HandleFlat(WaterDistanceDeliverToInstallInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterDistanceHeaderOutputDto, ReportOutput<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }
    }
}