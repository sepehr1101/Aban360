using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.ClaimPool.GatewayAdhoc.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using org.matheval;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class TariffCalculationByDetailHandler : BaseCalculator, ITariffCalculationByDetailHandler
    {
        private readonly IIntervalBillPrerequisiteInfoAddHoc _intervalBillPrerequisiteInfoAddHocHandler;
        private readonly ITariffQueryService _tariffQueryService;
        private readonly IValidator<IntervalBillSubscriptionInfoImaginary> _intervalBillValidator;
        private readonly IWaterMeterGetByInfoForTariffCalcDetailAddhoc _waterMeterAddhoc;
        public TariffCalculationByDetailHandler(
            IIntervalBillPrerequisiteInfoAddHoc intervalBillPrerequisiteInfoHandler,
            ITariffQueryService tariffQueryService,
            IValidator<IntervalBillSubscriptionInfoImaginary> intervalBillValidator,
            IWaterMeterGetByInfoForTariffCalcDetailAddhoc waterMeterAddhoc)
        {
            _intervalBillPrerequisiteInfoAddHocHandler = intervalBillPrerequisiteInfoHandler;
            _intervalBillPrerequisiteInfoAddHocHandler.NotNull(nameof(_intervalBillPrerequisiteInfoAddHocHandler));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(_tariffQueryService));

            _intervalBillValidator= intervalBillValidator;
            _intervalBillValidator.NotNull(nameof(intervalBillValidator));

            _waterMeterAddhoc= waterMeterAddhoc;
            _waterMeterAddhoc.NotNull(nameof(waterMeterAddhoc));
        }
        public async Task<IntervalCalculationResultWrapper> Handle(TariffByDetailCreateDto createDto, CancellationToken cancellationToken)
        {
            //var validationResult = await _intervalBillValidator.ValidateAsync(tariffTestInput, cancellationToken);
            //if (!validationResult.IsValid)
            //{
            //    var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            //    throw new CustomeValidationException(message);
            //}


            var waterMeters = await _waterMeterAddhoc.Handle(createDto.FromDateJalali, createDto.ToDateJalali
                , createDto.UsageId, createDto.IndividualTypeId, createDto.FromBillId, createDto.ToBillId
                , createDto.ZoneId, cancellationToken);


            IntervalBillSubscriptionInfoImaginary tariffTestInput = new IntervalBillSubscriptionInfoImaginary();
            //fill IntervalBillSubscriptionInfoImaginary 

            string previousReadingDate = tariffTestInput.PreviousWaterMeterDate;
            string currentReadingDate = DateTime.Now.ToShortPersianDateString();
            int consumption = GetConsumption(tariffTestInput.PreviousWaterMeterNumber, tariffTestInput.CurrentWaterMeterNumber);
            int duration = GetDuration(previousReadingDate, currentReadingDate);
            double average = GetDailyConsumptionAverage(consumption, duration);
            ICollection<Tariff> rawTariffs = await GetRawTariffs(previousReadingDate, currentReadingDate);
            ICollection<Tariff> tariffs = GetTariffs(rawTariffs, average, previousReadingDate, currentReadingDate);
            List<IntervalCalculationResult> intervalCalculationResults = CreateCalculationResult(tariffTestInput, tariffs);
            List<IntervalCalculationResult3> result3 = CreateCalculationResult3(intervalCalculationResults);
            IntervalCalculationResultWrapper calculationResult = CreateCalculationResultWrapper(previousReadingDate, currentReadingDate, consumption, duration, average, intervalCalculationResults, result3);
            return calculationResult;
        }

        private IntervalCalculationResultWrapper CreateCalculationResultWrapper(string previousReadingDate, string currentReadingDate, int consumption, int duration, double average, List<IntervalCalculationResult> intervalCalculationResults, List<IntervalCalculationResult3> result3)
        {
            IntervalCalculationResultWrapper calculationResult = new(consumption, duration, average, previousReadingDate, currentReadingDate);
            calculationResult.IntervalCalculationResults = result3;
            calculationResult.IntervalCount = intervalCalculationResults.Count;
            calculationResult.Amount = intervalCalculationResults.Sum(i => i.Amount);
            return calculationResult;
        }
        private List<IntervalCalculationResult> CreateCalculationResult(IntervalBillSubscriptionInfoImaginary info, ICollection<Tariff> tariffs)
        {
            List<IntervalCalculationResult> intervalCalculationResults = new List<IntervalCalculationResult>();
            foreach (var tariff in tariffs)
            {
                Expression expressionCondition = GetExpression(tariff.Condition, info);
                bool conditionSatisfied = expressionCondition.Eval<bool>();
                if (!conditionSatisfied)
                {
                    continue;
                }
                Expression expressionFormula = GetExpression(tariff.Formula, info);
                double result = expressionFormula.Eval<double>();
                intervalCalculationResults.Add(new IntervalCalculationResult(tariff, result));
            }

            return intervalCalculationResults;
        }
        private List<IntervalCalculationResult3> CreateCalculationResult3(List<IntervalCalculationResult> intervalCalculationResults)
        {
            return intervalCalculationResults
                .GroupBy(item => new { item.FromDate, item.ToDate })
                .Select(grp => new IntervalCalculationResult3
                {
                    FromDate = grp.Key.FromDate,
                    ToDate = grp.Key.ToDate,
                    CalculationInfo = grp.Select(g => new IntervalCalculationResult2()
                    {
                        Amount = g.Amount,
                        Consumption = g.Consumption,
                        Duration = g.Duration,
                        Formula = g.Formula,
                        LineItemTypeTitle = g.LineItemTypeTitle,
                        OfferingTitle = g.OfferingTitle
                    }).ToList()
                }).ToList();
        }
        private async Task<ICollection<Tariff>> GetRawTariffs(string @from, string @to)
        {
            ICollection<Tariff> allTariffs = await _tariffQueryService.Get(from, to);
            return allTariffs;
        }
        private ICollection<Tariff> GetTariffs(ICollection<Tariff> rawTariffs, double average, string fromDate, string toDate)
        {
            if (rawTariffs == null)
            {
                return new List<Tariff>();
            }
            rawTariffs.First().FromDateJalali = fromDate;
            rawTariffs.Last().ToDateJalali = toDate;
            foreach (Tariff tariff in rawTariffs)
            {
                int duration = GetDuration(tariff.FromDateJalali, tariff.ToDateJalali);
                double consumption = GetConsumption(average, duration);
                tariff.Duration = duration;
                tariff.Consumption = consumption;
            }
            return rawTariffs;
        }
        private int GetConsumption(int previousNumber, int currentNumber)
        {
            return currentNumber - previousNumber;
        }
        private double GetConsumption(double consumptionDailyAverage, int duration)
        {
            return consumptionDailyAverage * duration;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            return (currentGregorian.Value - previousGregorian.Value).Days;
        }
        private double GetDailyConsumptionAverage(int masraf, int duration)
        {
            return (double)masraf / (double)duration;
        }
    }
}
