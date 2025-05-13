using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using org.matheval;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal class BaseIntervalTariffEngine : BaseExpressionCalculator, IBaseIntervalTariffEngine
    {
        private readonly IIntervalBillPrerequisiteInfoAddHoc _intervalBillPrerequisiteInfoAddHocHandler;
        private readonly ITariffQueryService _tariffQueryService;
        private readonly ITariffConstantQueryService _tariffConstantQueryService;
        private readonly IValidator<TariffTestInput> _tariffValidator;
        private readonly IValidator<TariffTestImaginaryInput> _intervalBillValidator;

        protected BaseIntervalTariffEngine(IIntervalBillPrerequisiteInfoAddHoc intervalBillPrerequisiteInfoHandler,
            ITariffQueryService tariffQueryService,
            ITariffConstantQueryService tariffConstantQueryService,
            IValidator<TariffTestInput> tariffValidator,
            IValidator<TariffTestImaginaryInput> intervalBillValidator)
        {
            _intervalBillPrerequisiteInfoAddHocHandler = intervalBillPrerequisiteInfoHandler;
            _intervalBillPrerequisiteInfoAddHocHandler.NotNull(nameof(_intervalBillPrerequisiteInfoAddHocHandler));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(_tariffQueryService));

            _tariffConstantQueryService = tariffConstantQueryService;
            _tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));

            _tariffValidator = tariffValidator;
            _tariffValidator.NotNull(nameof(tariffValidator));

            _intervalBillValidator = intervalBillValidator;
            _intervalBillValidator.NotNull(nameof(intervalBillValidator));
        }

        public async Task<Tuple<ConsumptionInfo, List<IntervalCalculationResult>>> Calculate(TariffTestInput tariffTestInput, CancellationToken cancellationToken)
        {
            await Validate(tariffTestInput, cancellationToken);
            ConsumptionInfo consumptionInfo = GetConsumptionInfo(tariffTestInput);

            IntervalBillSubscriptionInfo info = await _intervalBillPrerequisiteInfoAddHocHandler.Handle(tariffTestInput.BillId, cancellationToken);
            ICollection<Tariff> rawTariffs = await GetRawTariffs(tariffTestInput.PreviousReadingDate, tariffTestInput.CurrentReadingDate);
            ICollection<Tariff> tariffs = GetTariffs(rawTariffs, consumptionInfo.AverageConsumption, tariffTestInput.PreviousReadingDate, tariffTestInput.CurrentReadingDate);
            List<IntervalCalculationResult> intervalCalculationResults = await CreateCalculationResult(info, tariffs, tariffTestInput.PreviousReadingDate, tariffTestInput.CurrentReadingDate, consumptionInfo.AverageConsumption);
            return Tuple.Create(consumptionInfo, intervalCalculationResults);
        }

        public async Task<Tuple<ConsumptionInfo, List<IntervalCalculationResult>>> Calculate(TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken)
        {
            await Validate(tariffTestInput, cancellationToken);
            ConsumptionInfo consumptionInfo = GetConsumptionInfo(tariffTestInput);

            ICollection<Tariff> rawTariffs = await GetRawTariffs(tariffTestInput.PreviousWaterMeterDate, tariffTestInput.CurrentWaterMeterDate);
            ICollection<Tariff> tariffs = GetTariffs(rawTariffs, consumptionInfo.AverageConsumption, tariffTestInput.PreviousWaterMeterDate, tariffTestInput.CurrentWaterMeterDate);
            List<IntervalCalculationResult> intervalCalculationResults = await CreateCalculationResult(tariffTestInput, tariffs, consumptionInfo.PreviousReadingDate, consumptionInfo.CurrentReadingDate, consumptionInfo.AverageConsumption);
            return Tuple.Create(consumptionInfo, intervalCalculationResults);
        }

        private async Task Validate(TariffTestInput tariffTestInput, CancellationToken cancellationToken)
        {
            var validationResult = await _tariffValidator.ValidateAsync(tariffTestInput, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
        }
        private async Task Validate(TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken)
        {
            var validationResult = await _intervalBillValidator.ValidateAsync(tariffTestInput, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
        }
        private ConsumptionInfo GetConsumptionInfo(TariffTestInput tariffTestInput)
        {
            int consumption = GetConsumption(tariffTestInput.PreviousReadingNumber, tariffTestInput.CurrentReadingNumber);
            int duration = GetDuration(tariffTestInput.PreviousReadingDate, tariffTestInput.PreviousReadingDate);
            double average = GetDailyConsumptionAverage(consumption, duration);
            ConsumptionInfo consumptionInfo = new(tariffTestInput.PreviousReadingDate, tariffTestInput.CurrentReadingDate, consumption, duration, average);
            return consumptionInfo;
        }
        private ConsumptionInfo GetConsumptionInfo(TariffTestImaginaryInput tariffTestInput)
        {
            int consumption = GetConsumption(tariffTestInput.PreviousWaterMeterNumber, tariffTestInput.CurrentWaterMeterNumber);
            int duration = GetDuration(tariffTestInput.PreviousWaterMeterDate, tariffTestInput.CurrentWaterMeterDate);
            double average = GetDailyConsumptionAverage(consumption, duration);
            ConsumptionInfo consumptionInfo = new(tariffTestInput.PreviousWaterMeterDate, tariffTestInput.CurrentWaterMeterDate, consumption, duration, average);
            return consumptionInfo;
        }
        private async Task<List<IntervalCalculationResult>> CreateCalculationResult(object objectOfFields, ICollection<Tariff> tariffs, string @from, string @to, double average)
        {
            Dictionary<string, object> constantDictionary = await GetTariffConstantsDictionary(from, to, average, objectOfFields);
            List<IntervalCalculationResult> intervalCalculationResults = new List<IntervalCalculationResult>();
            foreach (var tariff in tariffs)
            {
                Expression expressionCondition = GetExpression(tariff.Condition, objectOfFields, constantDictionary);
                bool conditionSatisfied = expressionCondition.Eval<bool>();
                if (!conditionSatisfied)
                {
                    continue;
                }
                Expression expressionFormula = GetExpression(tariff.Formula, objectOfFields, constantDictionary);
                double result = expressionFormula.Eval<double>();
                intervalCalculationResults.Add(new IntervalCalculationResult(tariff, result));
            }

            return intervalCalculationResults;
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
        private async Task<Dictionary<string, object>> GetTariffConstantsDictionary(string @from, string @to, double average, object objectOfFields)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            ICollection<TariffConstant> tariffConstatns = await _tariffConstantQueryService.Get(from, to);
            if (tariffConstatns is null)
            {
                return keyValuePairs;
            }
            List<IntervalCalculationResult> intervalCalculationResults = new List<IntervalCalculationResult>();
            foreach (var tariffConstant in tariffConstatns)
            {
                Expression expressionCondition = GetExpression(tariffConstant.Condition, objectOfFields);
                bool conditionSatisfied = expressionCondition.Eval<bool>();
                if (!conditionSatisfied)
                {
                    continue;
                }
                keyValuePairs.Add(tariffConstant.Key, Convert.ToDouble(tariffConstant.Value));
            }
            return keyValuePairs;
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
