using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Helpers;
using Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Aban360.ReportPool.Persistence.Features.Tagging;
using DNTPersianUtils.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Validations.TariffValidator;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class OldTariffEngine : BaseExpressionCalculator, IOldTariffEngine
    {
        const int monthDays = 30;
        const int thresholdDay = 4;
        const float vatRate = 0.1f;

        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly IMeterInfoDetailQueryService _meterInfoDetailQueryService;
        private readonly INerkhGetByConsumptionService _nerkhGetByConsumptionService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly ITable1GetService _table1QueryService;
        private readonly IBillIdTagService _tagService;
        private readonly IBillQueryService _billQueryService;
        private readonly IConsumptionCalculator _consumptionCalculator;

        private readonly IAbBahaCalculator _abBahaCalculator;
        private readonly IFazelabCalculator _fazelabCalculator;
        private readonly IHotSeasonCalculator _hotSeasonCalculator;
        private readonly IBudgetCalculator _budgetCalculator;
        private readonly IJavaniJamiatCalculator _javaniJamiatCalculator;
        private readonly IAvarezCalculator _avarezCalculator;
        private readonly IAbonmanCalculator _abonmanCalculator;

        public OldTariffEngine(
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IMeterInfoDetailQueryService meterInfoDetailQueryService,
            INerkhGetByConsumptionService nerkhGetByConsumptionService,
            IZaribCQueryService zaribCQueryService,
            ITable1GetService table1GetService,
            IBillIdTagService tagService,
            IConsumptionCalculator consumptionCalculator,
            IAbBahaCalculator abBahaCalculator,
            IBillQueryService billQueryService,
            IFazelabCalculator fazelabCalculator,
            IHotSeasonCalculator hotSeasonCalculator,
            IAbonmanCalculator abonmanCalculator,
            IBudgetCalculator budgetCalculator,
            IJavaniJamiatCalculator javaniJamiatCalculator,
            IAvarezCalculator avarezCalculator)
        {
            _customerInfoDetailQueryService = customerInfoDetailQueryService;
            _customerInfoDetailQueryService.NotNull(nameof(customerInfoDetailQueryService));

            _meterInfoDetailQueryService = meterInfoDetailQueryService;
            _meterInfoDetailQueryService.NotNull(nameof(meterInfoDetailQueryService));

            _nerkhGetByConsumptionService = nerkhGetByConsumptionService;
            _nerkhGetByConsumptionService.NotNull(nameof(nerkhGetByConsumptionService));

            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(_zaribCQueryService));

            _table1QueryService = table1GetService;
            _table1QueryService.NotNull(nameof(_table1QueryService));

            _tagService = tagService;
            _tagService.NotNull(nameof(_tagService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(_billQueryService));

            _consumptionCalculator = consumptionCalculator;
            _consumptionCalculator.NotNull(nameof(_consumptionCalculator));

            _abBahaCalculator = abBahaCalculator;
            _abBahaCalculator.NotNull(nameof(_fazelabCalculator));

            _fazelabCalculator = fazelabCalculator;
            _fazelabCalculator.NotNull(nameof(_fazelabCalculator));

            _hotSeasonCalculator = hotSeasonCalculator;
            _hotSeasonCalculator.NotNull(nameof(_hotSeasonCalculator));

            _budgetCalculator = budgetCalculator;
            _budgetCalculator.NotNull(nameof(_budgetCalculator));

            _javaniJamiatCalculator = javaniJamiatCalculator;
            _javaniJamiatCalculator.NotNull(nameof(_javaniJamiatCalculator));

            _avarezCalculator = avarezCalculator;
            _avarezCalculator.NotNull(nameof(_avarezCalculator));

            _abonmanCalculator = abonmanCalculator;
            _abonmanCalculator.NotNull(nameof(_abonmanCalculator));
        }

        public async Task<AbBahaCalculationDetails> Handle(MeterInfoInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            MeterInfoOutputDto meterInfo = await _meterInfoDetailQueryService.GetInfo(new CustomerInfoInputDto(customerInfo.ZoneId, customerInfo.Radif));
            AbBahaCalculationDetails calculationDetails = await GetCalculationDetails(meterInfo, customerInfo);
            return calculationDetails;
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken)
        {
            ValidateCounterStateCode(input.CounterStateCode, input.CurrentMeterNumber, input.PreviousNumber);

            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            MeterInfoOutputDto meterInfo = new MeterInfoOutputDto()
            {
                PreviousDateJalali = input.PreviousDateJalali,
                PreviousNumber = input.PreviousNumber,
                CurrentNumber = input.CurrentMeterNumber,
                CurrentDateJalali = input.CurrentDateJalali,
                CounterStateCode = input.CounterStateCode,
            };
            AbBahaCalculationDetails calculationDetails = await GetCalculationDetails(meterInfo, customerInfo);
            return calculationDetails;
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterImaginaryInputDto input, CancellationToken cancellationToken)
        {
            ValidateCounterStateCode(input.CustomerInfo.CounterStateCode, input.MeterPreviousData.CurrentMeterNumber, input.MeterPreviousData.PreviousNumber);

            //TODO: direct create object from MeterComparisonBatchWithAggregatedNerkhGetHandler.cs
            CustomerInfoOutputDto customerInfo = CreateCustomerInfoDto(input);
            try
            {
                MeterInfoOutputDto meterInfo = new()
                {
                    PreviousDateJalali = input.MeterPreviousData.PreviousDateJalali,
                    PreviousNumber = input.MeterPreviousData.PreviousNumber,
                    CurrentDateJalali = input.MeterPreviousData.CurrentDateJalali,
                    CurrentNumber = input.MeterPreviousData.CurrentMeterNumber,
                    CounterStateCode = input.CustomerInfo.CounterStateCode
                };
                AbBahaCalculationDetails calculationDetails = await GetCalculationDetails(meterInfo, customerInfo);
                return calculationDetails;
            }
            catch (Exception e)
            {
                throw new BaseException($"{customerInfo.BillId} {input.MeterPreviousData.PreviousDateJalali} {input.MeterPreviousData.PreviousNumber}");
            }
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterDateInfoWithMonthlyConsumptionOutputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            AbBahaCalculationDetails calculationDetails = await GetCalculationDetailsWithConsumption(input, customerInfo);
            return calculationDetails;
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterDateInfoByLastMonthlyConsumptionOutputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            BedBesConsumptionOutputDto latestConsumptionAverage = await _billQueryService.Get(input.BillId);
            MeterDateInfoWithMonthlyConsumptionOutputDto meterInfoWithConsumption = new MeterDateInfoWithMonthlyConsumptionOutputDto(input.BillId, input.PreviousDateJalali, input.CurrentDateJalali, latestConsumptionAverage.ConsumptionAverage);
            AbBahaCalculationDetails calculationDetails = await GetCalculationDetailsWithConsumption(meterInfoWithConsumption, customerInfo);
            return calculationDetails;
        }
        private async Task<AbBahaCalculationDetails> GetCalculationDetails(MeterInfoOutputDto meterInfo, CustomerInfoOutputDto customerInfo)
        {
            ConsumptionInfo consumptionInfo = _consumptionCalculator.GetConsumptionInfo(meterInfo, customerInfo);
            NerkhByConsumptionInputDto nerkhInput = CreateNerkhInput(meterInfo, customerInfo, consumptionInfo);
            ValidatePreviousDate(meterInfo.PreviousDateJalali);
            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int, IEnumerable<NerkhGetDto>) allNerkhAbAbAzad 
                = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(nerkhInput);
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, meterInfo.CurrentDateJalali, customerInfo, meterInfo, consumptionInfo, allNerkhAbAbAzad.Item5);
            return result;
        }

        private async Task<AbBahaCalculationDetails> GetCalculationDetailsWithConsumption(MeterDateInfoWithMonthlyConsumptionOutputDto meterInfoWithConsumption, CustomerInfoOutputDto customerInfo)
        {
            ConsumptionInfo consumptionInfo = _consumptionCalculator.GetConsumptionInfoWithMonthlyConsumption(meterInfoWithConsumption, customerInfo);

            MeterInfoOutputDto meterInfo = new MeterInfoOutputDto(meterInfoWithConsumption.PreviousDateJalali, meterInfoWithConsumption.CurrentDateJalali, 0, 0, null);
            NerkhByConsumptionInputDto nerkhInput = CreateNerkhInput(meterInfo, customerInfo, consumptionInfo);
            ValidatePreviousDate(meterInfo.PreviousDateJalali);
            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int, IEnumerable<NerkhGetDto>) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(nerkhInput);
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, meterInfo.CurrentDateJalali, customerInfo, meterInfo, consumptionInfo, allNerkhAbAbAzad.Item5);
            return result;
        }

        private NerkhByConsumptionInputDto CreateNerkhInput(MeterInfoOutputDto meterInfo, CustomerInfoOutputDto customerInfo, ConsumptionInfo consumptionInfo)
        {
            int constructionBranchType = 4;
            int azadUsageId = 39;
            return new NerkhByConsumptionInputDto(
                customerInfo.ZoneId,
                customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                meterInfo.PreviousDateJalali,
                meterInfo.CurrentDateJalali,
                consumptionInfo.MonthlyAverageConsumption);
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaCalculationDetails(IEnumerable<NerkhGetDto> allNerkh, IEnumerable<AbAzadFormulaDto> abAzad, IEnumerable<ZaribGetDto> zarib, string currentDateJalali, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ConsumptionInfo consumptionInfo, IEnumerable<NerkhGetDto> nerkh1403)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int counter = 0;
            double sumAbBaha = 0, sumFazelab = 0, sumHotSeasonAbBaha = 0, sumHotSeasonFazelab = 0, sumAbonmanAbBaha = 0, sumAbonmanFazelab = 0;
            double sumBoodjePart1 = 0, sumBoodjePart2 = 0, sumAvarez = 0;
            double sumAbBahaDiscount = 0, sumFazelabDiscount = 0, sumHotSeasonAbDiscount = 0, sumHotSeasonFazelabDiscount = 0,
                   sumAbonmanAbDiscount = 0, sumAbonmanFazelabDiscount = 0, sumAvarezDiscount = 0, sumJavaniDiscount = 0, sumBoodjeDiscount = 0;
            double sumJavaniAmount = 0; double multiplier = 0;
            IEnumerable<int> tags = await _tagService.GetIdsByBillId(customerInfo.BillId.Trim());
            Table1GetDto table1 = await _table1QueryService.GetByTown(customerInfo.ZoneId);
            foreach (var nerkhItem in allNerkh)
            {
                AbAzadFormulaDto abAzadItem = abAzad.ElementAt(counter);
                ZaribGetDto zaribItem = zarib.ElementAt(counter);
                ZaribCQueryDto zaribC = await _zaribCQueryService.GetZaribC(nerkhItem.Date1, nerkhItem.Date2);
                nerkhItem.C = zaribC.C;                
                nerkhItem.Olgo = GetOlgoo(nerkhItem.Date1, table1.olgo);
                ConsumptionInfo partialConsumptionInfo = new(nerkhItem.Date1, nerkhItem.Date2, consumptionInfo.Consumption, consumptionInfo.Duration, consumptionInfo.DailyAverageConsumption, consumptionInfo.FinalDomesticUnit);
                BaseOldTariffEngineOutputDto resultCalc = CalculateWaterBill(nerkhItem, nerkh1403.ElementAt(counter), abAzadItem, zaribItem, customerInfo, meterInfo, currentDateJalali, partialConsumptionInfo, table1.olgo, zaribC is not null ? zaribC.C : null, tags);
                nerkhItem.CalcVaj = resultCalc.AbBahaValues.AbBahaAmount.ToString();
                sumAbBaha += resultCalc.AbBahaValues.AbBahaAmount;
                sumFazelab += resultCalc.FazelabAmount;
                sumBoodjePart1 += resultCalc.BoodjePart1;
                sumBoodjePart2 += resultCalc.BoodjePart2;
                sumHotSeasonAbBaha += resultCalc.HotSeasonAbBahaAmount;
                sumHotSeasonFazelab += resultCalc.HotSeasonFazelabAmount;
                sumAbBahaDiscount += resultCalc.AbBahaDiscount;
                sumFazelabDiscount += resultCalc.FazelabDiscount;
                sumHotSeasonAbDiscount += resultCalc.HotSeasonDiscount;
                sumHotSeasonFazelabDiscount += resultCalc.HotSeasonFazelabDiscount;
                sumAbonmanAbBaha += resultCalc.AbonmanAbAmount;
                sumAvarez += resultCalc.AvarezAmount;
                sumJavaniAmount += resultCalc.JavaniAmount;
                sumAbonmanAbDiscount += resultCalc.AbonmanAbDiscount;
                sumAbonmanFazelabDiscount = resultCalc.AbonmanFazelabDiscount;
                sumAvarezDiscount += resultCalc.AvarezDiscount;
                sumBoodjeDiscount += resultCalc.BoodjeDiscount;
                sumJavaniDiscount += resultCalc.JavaniDiscount;

                counter++;
            }
            sumAbonmanAbBaha = _abonmanCalculator.CalculateAb(customerInfo, meterInfo, currentDateJalali);
            sumAbonmanFazelab = _fazelabCalculator.Calculate(meterInfo.PreviousDateJalali, currentDateJalali, consumptionInfo.Duration, customerInfo, sumAbonmanAbBaha, currentDateJalali, true);
            sumAbonmanAbDiscount = _abonmanCalculator.CalculateDiscount(customerInfo.UsageId, customerInfo.BranchType, sumAbonmanAbBaha, sumAbBahaDiscount, customerInfo.IsSpecial, consumptionInfo, customerInfo);
            sumAbonmanFazelabDiscount = _abonmanCalculator.CalculateDiscount(customerInfo.UsageId, customerInfo.BranchType, sumAbonmanFazelab, sumFazelabDiscount, customerInfo.IsSpecial, consumptionInfo, customerInfo);
            double AbBahaResult = sumAbBaha + sumHotSeasonAbBaha + sumAbonmanAbBaha;
            double sumBoodje = sumBoodjePart1 + sumBoodjePart2;
            double sumMaliatAmount = CalculateTax(sumAbBaha, sumFazelab, sumAbonmanAbBaha, sumAbonmanFazelab, sumHotSeasonAbBaha, sumHotSeasonFazelab, sumBoodje);
            double maliatDiscount = CalculateTaxDiscount(sumAbBahaDiscount, sumFazelabDiscount, sumAbonmanAbDiscount, sumAbonmanFazelabDiscount, sumHotSeasonAbDiscount, sumHotSeasonFazelabDiscount, sumBoodjeDiscount);

            stopWatch.Stop();
            return new AbBahaCalculationDetails(Math.Round(AbBahaResult), Math.Round(sumAbBaha), Math.Round(sumFazelab), Math.Round(sumBoodjePart1), Math.Round(sumBoodjePart2), Math.Round(sumBoodje), Math.Round(sumHotSeasonAbBaha), Math.Round(sumHotSeasonFazelab), Math.Round(sumAbBahaDiscount), Math.Round(sumHotSeasonAbDiscount), Math.Round(sumFazelabDiscount), Math.Round(sumAbonmanAbBaha), Math.Round(sumAvarez), Math.Round(sumJavaniAmount), Math.Round(sumMaliatAmount), Math.Round(sumAbonmanFazelab),
                       Math.Round(sumAbonmanAbDiscount), Math.Round(sumAbonmanFazelabDiscount), Math.Round(sumAvarezDiscount), Math.Round(sumJavaniDiscount), Math.Round(maliatDiscount), Math.Round(sumBoodjeDiscount), allNerkh, abAzad, zarib, consumptionInfo, meterInfo, customerInfo, stopWatch.ElapsedMilliseconds, sumHotSeasonFazelabDiscount);
        }
        private CustomerInfoOutputDto CreateCustomerInfoDto(MeterImaginaryInputDto input)
        {
            return new CustomerInfoOutputDto()
            {
                ZoneId = input.CustomerInfo.ZoneId,
                Radif = input.CustomerInfo.Radif ?? 0,
                BranchType = input.CustomerInfo.BranchType,
                UsageId = input.CustomerInfo.UsageId,
                DomesticUnit = input.CustomerInfo.DomesticUnit,
                CommertialUnit = input.CustomerInfo.CommertialUnit,
                OtherUnit = input.CustomerInfo.OtherUnit,
                EmptyUnit = input.CustomerInfo.EmptyUnit ?? 0,
                WaterInstallationDateJalali = input.CustomerInfo.WaterInstallationDateJalali,
                SewageInstallationDateJalali = input.CustomerInfo.SewageInstallationDateJalali,
                //WaterCount = input.CustomerInfo.WaterCount,
                SewageCalcState = input.CustomerInfo.SewageCalcState ?? 0,
                ContractualCapacity = input.CustomerInfo.ContractualCapacity ?? 0,
                HouseholdNumber = input.CustomerInfo.HouseholdNumber ?? 0,
                HouseholdDate = input.CustomerInfo.HouseholdDate,
                ReadingNumber = input.CustomerInfo.ReadingNumber ?? string.Empty,
                VillageId = input.CustomerInfo.VillageId,
                IsSpecial = input.CustomerInfo.IsSpecial,
                BillId = input.MeterPreviousData.BillId ?? string.Empty,
                VirtualCategoryId = input.CustomerInfo.VirtualCategoryId ?? 0,
                WaterRegisterDate = input.CustomerInfo.WaterRegisterDate,
                SewageRegisterDate = input.CustomerInfo.SewageRegisterDate,

            };
        }
        private BaseOldTariffEngineOutputDto CalculateWaterBill(NerkhGetDto nerkh, NerkhGetDto nerkh1403, AbAzadFormulaDto abAzad, ZaribGetDto zarib, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali, ConsumptionInfo consumptionInfo, int _olgoo, [Optional] int? c, [Optional] IEnumerable<int>? tagIds)
        {
            DateOnly previousDate = ConvertJalaliToGregorian(meterInfo.PreviousDateJalali);
            DateOnly currentDate = ConvertJalaliToGregorian(currentDateJalali);
            nerkh.DailyAverageConsumption = consumptionInfo.DailyAverageConsumption;
            (nerkh, nerkh.Duration, nerkh.PartialConsumption) = CalcPartial(nerkh, previousDate, currentDate, consumptionInfo);

            int olgoo = GetOlgoo(nerkh.Date2, _olgoo);
            bool isVillageCalculation = IsVillage(customerInfo.ZoneId);
            double monthlyConsumption = nerkh.DailyAverageConsumption * monthDays;

            CalculateAbBahaOutputDto abBahaResult = _abBahaCalculator.Calculate(nerkh, nerkh1403, customerInfo, meterInfo, zarib, abAzad, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, c, tagIds); //CalculateAbBaha(nerkh, customerInfo, meterInfo, zarib, abAzad, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, multiplierAbBaha, c, tagIds);
            (double, double) boodje = _budgetCalculator.Calculate(nerkh, customerInfo, currentDateJalali, monthlyConsumption, olgoo, consumptionInfo);
            double fazelab = _fazelabCalculator.Calculate(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, abBahaResult.AbBahaAmount, currentDateJalali, false);
            (int, double, double) hotSeasonAbBaha = _hotSeasonCalculator.CalculateAb(nerkh, abBahaResult.AbBahaAmount, customerInfo, monthlyConsumption, abBahaResult);
            (int, double, double) hotSeasonFazelab = _hotSeasonCalculator.CalcFazelab(nerkh, customerInfo, fazelab, monthlyConsumption, abBahaResult);
            double avarez = _avarezCalculator.Calculate(nerkh, customerInfo, monthlyConsumption);
            double javani = _javaniJamiatCalculator.Calculate(nerkh, customerInfo, abBahaResult.AbBahaAmount, monthlyConsumption, olgoo);

            //Discounts
            double abBahaDiscount = _abBahaCalculator.CalculateDiscount(zarib, isVillageCalculation, monthlyConsumption, customerInfo, nerkh, olgoo, abBahaResult, false, consumptionInfo.FinalDomesticUnit);
            double fazelabDiscount = _fazelabCalculator.CalculateDiscount(abBahaDiscount, fazelab, customerInfo, nerkh);
            double hotSeasonAbDiscount = _hotSeasonCalculator.CalculateDiscount(nerkh, abBahaDiscount, hotSeasonAbBaha, customerInfo, abBahaResult);
            double hotSeasonFazelabDiscount = _hotSeasonCalculator.CalculateDiscount(nerkh, fazelabDiscount, hotSeasonFazelab, customerInfo, abBahaResult);
            double boodjeDiscount = _budgetCalculator.CalculateDiscount(customerInfo, abBahaDiscount, boodje, nerkh);
            return new BaseOldTariffEngineOutputDto(
                abBahaValues: abBahaResult,
                fazelabAmount: fazelab,
                hotSeasonAbBahaAmount: hotSeasonAbBaha.Item2 + hotSeasonAbBaha.Item3,
                hotSeasonFazelabAmount: hotSeasonFazelab.Item2 + hotSeasonFazelab.Item3,
                boodjePart1: boodje.Item1,
                boodjePart2: boodje.Item2,
                abBahaDiscount: abBahaDiscount,
                hotSeasonDiscount: hotSeasonAbDiscount,
                fazelabDiscount: fazelabDiscount,
                abonmanAbAmount: 0,
                avarezAmount: avarez,
                javaniAmount: javani,
                abonmanAbDiscount: 0,
                abonamenFazelabDiscount: 0,
                avarezDiscount: 0,
                javaniDiscount: 0,
                boodjeDiscount,
                hotSeasonFazelabDiscount: hotSeasonFazelabDiscount,
                multiplier: abBahaResult.Multiplier);
        }
        private int GetOlgoo(string nerkhDate2, int olgo)
        {
            string baseDate = "1403/12/30";
            return nerkhDate2.CompareTo(baseDate) <= 0 ? 14 : olgo;
        }
        private double CalculateTax(params double[] amounts)
        {
            return amounts.Sum() * vatRate;
        }
        private double CalculateTaxDiscount(params double[] amounts)
        {
            return amounts.Sum() * vatRate;
        }
    }
}