using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;
using static Aban360.Common.Timing.CalculationDistanceDate;
using System.Diagnostics.Metrics;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class MalfunctionToChangeSummaryHandler : IMalfunctionToChangeSummaryHandler
    {
        private readonly IMalfunctionToChangeSummaryQueryService _malfunctionToChangeQueryService;
        private readonly IValidator<MalfunctionToChangeInputDto> _validator;
        public MalfunctionToChangeSummaryHandler(
            IMalfunctionToChangeSummaryQueryService malfunctionToChangeQueryService,
            IValidator<MalfunctionToChangeInputDto> validator)
        {
            _malfunctionToChangeQueryService = malfunctionToChangeQueryService;
            _malfunctionToChangeQueryService.NotNull(nameof(malfunctionToChangeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeSummaryDataOutputDto>> Handle(MalfunctionToChangeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeSummaryDataOutputFromDataBaseDto> malfunctionToChange = await _malfunctionToChangeQueryService.Get(input);
            int sumDurations = 0;
            ICollection<MalfunctionToChangeSummaryDataOutputDto> malfunctionMeter = malfunctionToChange
                  .ReportData
                  .GroupBy(meter => meter.ZoneId)
                  .Select(meter =>
                  {
                      var (average, max, min, sum) = GetDuration(meter);
                      sumDurations += sum;
                      return new MalfunctionToChangeSummaryDataOutputDto()
                      {
                          ZoneId = meter.Key,
                          ZoneTitle = meter.Max(m => m.ZoneTitle),
                          Duration = average,
                          MaxDuration = max,
                          MinDuration = min
                      };
                  }).ToList();
            int averageDuration = sumDurations / malfunctionToChange.ReportData.Count();
            malfunctionToChange.ReportHeader.AverageDuration= CalculationDistanceDate.ConvertDayToDate(averageDuration);
            ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeSummaryDataOutputDto> result = new(malfunctionToChange.Title, malfunctionToChange.ReportHeader, malfunctionMeter);
            return result;
        }
        private (string, string, string, int) GetDuration(IEnumerable<MalfunctionToChangeSummaryDataOutputFromDataBaseDto> meters)
        {
            ICollection<int> durations = new List<int>();
            meters.ForEach(m =>
            {
                CalcDistanceResultDto calcDistance = CalculationDistanceDate.CalcDistance(m.LatestMalfunctionDateJalali, m.ChangeDateJalali);
               int duration = calcDistance.HasError ? 0: calcDistance.Distance;
                //todo:min date equal 0

                durations.Add(duration);
            });

            string average = CalculationDistanceDate.ConvertDayToDate(durations.Sum()/durations.Count());
            string max = CalculationDistanceDate.ConvertDayToDate(durations.Max());
            string min = CalculationDistanceDate.ConvertDayToDate(durations.Min());

            return (average, max, min, durations.Sum());
        }
    }
}
