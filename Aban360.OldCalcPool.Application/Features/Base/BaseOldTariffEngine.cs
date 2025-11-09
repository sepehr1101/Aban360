using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using System.Runtime.InteropServices;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal abstract class BaseOldTariffEngine : BaseExpressionCalculator
    {
        const int monthDays = 30;
        private readonly IAbBahaCalculator _abBahaCalculator;
        private readonly IFazelabCalculator _fazelabCalculator;
        private readonly IHotSeasonCalculator _hotSeasonCalculator;
        private readonly IBudgetCalculator _budgetCalculator;
        private readonly IJavaniJamiatCalculator _javaniJamiatCalculator;
        private readonly IAvarezCalculator _avarezCalculator;

        internal BaseOldTariffEngine(
            IAbBahaCalculator abBahaCalculator,
            IFazelabCalculator fazelabCalculator,
            IHotSeasonCalculator hotSeasonCalculator,
            IAbonmanCalculator abonmanCalculator,
            IBudgetCalculator budgetCalculator,
            IJavaniJamiatCalculator javaniJamiatCalculator,
            IAvarezCalculator avarezCalculator)
        {
            _abBahaCalculator = abBahaCalculator;
            _abBahaCalculator.NotNull(nameof(_fazelabCalculator));

            _fazelabCalculator = fazelabCalculator;
            _fazelabCalculator.NotNull(nameof(_fazelabCalculator));

            _hotSeasonCalculator = hotSeasonCalculator;
            _hotSeasonCalculator.NotNull(nameof(_hotSeasonCalculator));

            _budgetCalculator = budgetCalculator;
            _budgetCalculator.NotNull(nameof(_budgetCalculator));

            _javaniJamiatCalculator = javaniJamiatCalculator;
            _javaniJamiatCalculator.NotNull( nameof(_javaniJamiatCalculator));

            _avarezCalculator = avarezCalculator;
            _avarezCalculator.NotNull(nameof(_avarezCalculator));
        }

        /// <summary>
        ///  تابع اصلی با دسترسی اینترنال بابت محاسبه تک رکورد جدول نرخ
        /// </summary>
        /// <returns>مقدار خروجی بعد از اتمام نوشتن کد، اصلاح شود</returns>
        internal BaseOldTariffEngineOutputDto CalculateWaterBill(NerkhGetDto nerkh, AbAzadFormulaDto abAzad, ZaribGetDto zarib, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali, ConsumptionInfo consumptionInfo, int _olgoo, [Optional] int? c, [Optional] IEnumerable<int>? tagIds)
        {
            DateOnly previousDate = ConvertJalaliToGregorian(meterInfo.PreviousDateJalali);
            DateOnly currentDate = ConvertJalaliToGregorian(currentDateJalali);
            nerkh.DailyAverageConsumption = consumptionInfo.DailyAverageConsumption;
            (nerkh, nerkh.Duration, nerkh.PartialConsumption) = CalcPartial(nerkh, previousDate, currentDate, consumptionInfo);

            int olgoo = GetOlgoo(nerkh.Date2, _olgoo);
            bool isVillageCalculation = IsVillage(customerInfo.ZoneId);
            double monthlyConsumption = nerkh.DailyAverageConsumption * monthDays;            

            CalculateAbBahaOutputDto abBahaResult = _abBahaCalculator.Calculate(nerkh, customerInfo, meterInfo, zarib, abAzad, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, c, tagIds); //CalculateAbBaha(nerkh, customerInfo, meterInfo, zarib, abAzad, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, multiplierAbBaha, c, tagIds);
            (double, double) boodje = _budgetCalculator.Calculate(nerkh, customerInfo, currentDateJalali, monthlyConsumption, olgoo,consumptionInfo);
            double fazelab = _fazelabCalculator.Calculate(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, abBahaResult.AbBahaAmount, currentDateJalali, false);
            (int, double) hotSeasonAbBaha = _hotSeasonCalculator.CalculateAb(nerkh, abBahaResult.AbBahaAmount, customerInfo, monthlyConsumption);//CalcHotSeasonAbBaha(nerkh, abBahaResult.AbBahaAmount, customerInfo, monthlyConsumption);
            (int, double) hotSeasonFazelab = _hotSeasonCalculator.CalcFazelab(nerkh, customerInfo, fazelab, monthlyConsumption);//CalcHotSeasonFaselab(nerkh, customerInfo, fazelab, monthlyConsumption);
            double avarez = _avarezCalculator.Calculate(nerkh, customerInfo, monthlyConsumption);
            double javani = _javaniJamiatCalculator.Calculate(nerkh, customerInfo, abBahaResult.AbBahaAmount, monthlyConsumption, olgoo);
                        
            //Discounts
            double abBahaDiscount = _abBahaCalculator.CalculateDiscount(zarib, isVillageCalculation, monthlyConsumption, customerInfo, nerkh, olgoo, (long)abBahaResult.AbBahaAmount, false, consumptionInfo.FinalDomesticUnit);
            double fazelabDiscount = _fazelabCalculator.CalculateDiscount(abBahaDiscount, fazelab, customerInfo, nerkh);
            double hotSeasonAbDiscount = _hotSeasonCalculator.CalculateDiscount(nerkh, abBahaDiscount, hotSeasonAbBaha, customerInfo);
            double hotSeasonFazelabDiscount = _hotSeasonCalculator.CalculateDiscount(nerkh, fazelabDiscount, hotSeasonFazelab, customerInfo);         
            double boodjeDiscount = _budgetCalculator.CalculateDiscount(customerInfo,abBahaDiscount,boodje, nerkh);
            return new BaseOldTariffEngineOutputDto(abBahaResult, fazelab, hotSeasonAbBaha.Item2, hotSeasonFazelab.Item2, boodje.Item1, boodje.Item2, abBahaDiscount, hotSeasonAbDiscount, fazelabDiscount+hotSeasonFazelabDiscount, 0, avarez, javani,
                0, 0, 0, 0, boodjeDiscount);
        }
        private int GetOlgoo(string nerkhDate2, int olgo)
        {
            string baseDate = "1403/12/30";
            return nerkhDate2.CompareTo(baseDate) <= 0 ? 14 : olgo;
        }        
    }
}