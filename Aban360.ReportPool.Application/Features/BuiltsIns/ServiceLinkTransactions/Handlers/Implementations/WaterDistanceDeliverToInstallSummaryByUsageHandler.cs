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
    internal sealed class WaterDistanceDeliverToInstallSummaryByUsageHandler : IWaterDistanceDeliverToInstallSummaryByUsageHandler
    {
        private readonly IWaterDistanceDeliverToInstallSummaryQueryService _waterDistanceDeliverToInstallSummaryByUsageQuery;
        private readonly IValidator<WaterDistanceDeliverToInstallInputDto> _validator;
        public WaterDistanceDeliverToInstallSummaryByUsageHandler(
            IWaterDistanceDeliverToInstallSummaryQueryService waterDistanceDeliverToInstallSummaryByUsageQuery,
            IValidator<WaterDistanceDeliverToInstallInputDto> validator)
        {
            _waterDistanceDeliverToInstallSummaryByUsageQuery = waterDistanceDeliverToInstallSummaryByUsageQuery;
            _waterDistanceDeliverToInstallSummaryByUsageQuery.NotNull(nameof(waterDistanceDeliverToInstallSummaryByUsageQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto>> Handle(WaterDistanceDeliverToInstallInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto> result = await _waterDistanceDeliverToInstallSummaryByUsageQuery.Get(input, ReportLiterals.UsageTitle);

            float sumDistance = 0;
            ICollection<float> distances = new List<float>();
            foreach (var item in result.ReportData)
            {
                item.DistanceAverageText = CalculationDistanceDate.ConvertDayToDate((int)item.DistanceAverage);
                distances.Add(item.DistanceAverage);

                float weight = item.CustomerCount * item.DistanceAverage;
                sumDistance += weight;
            }
            float averageDistance = (float)Math.Round(sumDistance / result.ReportHeader.CustomerCount, 2);
            int allZoneRound = (int)Math.Round(averageDistance);
            result.ReportHeader.AverageDistanceNumber = averageDistance;
            result.ReportHeader.AverageDistance = CalculationDistanceDate.ConvertDayToDate(allZoneRound);
            result.ReportHeader.MaxDistance = CalculationDistanceDate.ConvertDayToDate(distances.Any() ? (int)distances.Max() : 0);
            result.ReportHeader.MinDistance = CalculationDistanceDate.ConvertDayToDate(distances.Any() ? (int)distances.Min() : 0);


            return result;
        }
    }
}
