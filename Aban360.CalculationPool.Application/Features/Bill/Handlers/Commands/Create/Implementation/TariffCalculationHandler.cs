using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Exceptions;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using DNTPersianUtils.Core;
using org.matheval;
using System.Reflection;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class TariffCalculationHandler : ITariffCalculationHandler
    {
        private readonly IIntervalBillPrerequisiteInfoHandler _intervalBillPrerequisiteInfoHandler;
        private readonly ITariffQueryService _tariffQueryService;
        public TariffCalculationHandler(
            IIntervalBillPrerequisiteInfoHandler intervalBillPrerequisiteInfoHandler,
            ITariffQueryService tariffQueryService)
        {
            _intervalBillPrerequisiteInfoHandler = intervalBillPrerequisiteInfoHandler;
            _intervalBillPrerequisiteInfoHandler.NotNull(nameof(_intervalBillPrerequisiteInfoHandler));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(_tariffQueryService));
        }
        public async Task<IntervalCalculationResultWrapper> Test(TariffTestInput tariffTestInput, CancellationToken cancellationToken)
        {
            int previousReadingNumber = tariffTestInput.PreviousReadingNumber;
            int currentReadingNumber = tariffTestInput.CurrentReadingNumber;
            string previousReadingDate = tariffTestInput.PreviousReadingDate;
            string currentReadingDate = DateTime.Now.ToShortPersianDateString();
            int consumption = GetConsumption(previousReadingNumber, currentReadingNumber);
            int duration = GetDuration(previousReadingDate, currentReadingDate);
            double average= GetDailyConsumptionAverage(consumption, duration);
            IntervalCalculationResultWrapper calculationResult = new()
            {
                Consumption = consumption,
                Duration= duration,
                ConsumptionAverage=average,
                FromDate=previousReadingDate,
                ToDate=currentReadingDate,
            };
            
            IntervalBillSubscriptionInfo info = await _intervalBillPrerequisiteInfoHandler.Handle(tariffTestInput.BillId, cancellationToken);
            var rawTariffs = await GetRawTariffs(previousReadingDate, currentReadingDate);
            var tariffs= GetTariffs(rawTariffs,average,previousReadingDate, currentReadingDate);
            List<IntervalCalculationResult> intervalCalculationResults = new List<IntervalCalculationResult>();
            foreach (var tariff in tariffs)
            {
                Expression expressionCondition = GetExpression(tariff.Condition, info);
                bool conditionSatisfied= expressionCondition.Eval<bool>();
                if (!conditionSatisfied)
                {
                    continue;   
                }
                Expression expressionFormula = GetExpression(tariff.Formula, info);
                double result = expressionFormula.Eval<double>();
                intervalCalculationResults.Add(new IntervalCalculationResult() { Amount=result,Formula=tariff.Formula,Consumption=tariff.Consumption,Duration=tariff.Duration,FromDate=tariff.FromDateJalali,ToDate=tariff.ToDateJalali,LineItemTypeTitle=tariff.LineItemType.Title,OfferingTitle=tariff.Offering.Title});
            }
            calculationResult.IntervalCalculationResults = intervalCalculationResults;
            calculationResult.IntervalCount = intervalCalculationResults.Count;
            calculationResult.Amount = intervalCalculationResults.Sum(i => i.Amount);
            return calculationResult;
        }
        private Expression GetExpression(string formula, IntervalBillSubscriptionInfo info)
        {
            Dictionary<string, object> propertyDictionary = GetDictionaryOfProperties(info);
            Expression expression = new Expression(formula);
            List<string> errors = expression.GetError();
            if (errors != null && errors.Any())
            {
                throw new ExpressionValidationException(errors.First());
            }
            List<string> formulaVariables = expression.getVariables();
            foreach (string variable in formulaVariables)
            {
                Bind(expression, variable, propertyDictionary);
            }
            return expression;
        }
        private void Bind(Expression expression, string formulaVariable, Dictionary<string, object> propertyDictionary)
        {
            foreach (var prop in propertyDictionary)
            {
                if (formulaVariable != null && formulaVariable.Equals(prop.Key))
                {
                    expression.Bind(prop.Key, prop.Value);
                    break;
                }
            }
        }
        private Dictionary<string, object> GetDictionaryOfProperties(object obj)
        {
            if (obj == null)
            {
                return new Dictionary<string, object>();
            }
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(obj, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }
        private async Task<ICollection<Tariff>> GetRawTariffs(string @from, string @to)
        {
            ICollection<Tariff> allTariffs = await _tariffQueryService.Get(from, to);
            return allTariffs;
        }
        private ICollection<Tariff> GetTariffs(ICollection<Tariff> rawTariffs, double average, string fromDate, string toDate)
        {
            if(rawTariffs == null)
            {
                return new List<Tariff>();
            }
            rawTariffs.First().FromDateJalali=fromDate;
            rawTariffs.Last().ToDateJalali=toDate;
            foreach (Tariff tariff in rawTariffs)
            {
                int duration=GetDuration(tariff.FromDateJalali,tariff.ToDateJalali);
                double consumption= GetConsumption(average, duration);
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
            return consumptionDailyAverage*duration;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            return (currentGregorian.Value-previousGregorian.Value).Days;
        }
        private double GetDailyConsumptionAverage(int masraf, int duration)
        {
            return (double)masraf / (double)duration;
        }                
    }
    public record IntervalCalculationResultWrapper
    {
        public ICollection<IntervalCalculationResult>? IntervalCalculationResults { get; set; }
        public int Consumption { get; set; }
        public double ConsumptionAverage { get; set; }
        public int IntervalCount { get; set; }
        public double Amount { get; set; }
        public int Duration { get; set; }
        public string FromDate { get; set; } = default!;
        public string ToDate { get; set; } = default!;
    }
    public record IntervalCalculationResult()
    {
        public string FromDate { get; set; } = default!;
        public string ToDate { get; set; } = default!;
        public string Formula { get; set; } = default!;
        public double Consumption { get; set; }
        public int Duration { get; set; }
        public double Amount { get; set; }
        public string OfferingTitle { get; set; }= default!;
        public string LineItemTypeTitle { get; set; }= default!;
    }
    public record TariffTestInput
    {
        public int PreviousReadingNumber { get; set; }
        public int CurrentReadingNumber { get; set; }
        public string PreviousReadingDate { get; set; }= default!;
        public string BillId { get; set; } = default!;
    }
}
