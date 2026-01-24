using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Caching;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Helpers;
using Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Persistence.Features.Tagging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Validations.TariffValidator;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class OldTariffEngine : BaseExpressionCalculator, IOldTariffEngine
    {
        const int monthDays = 30;

        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly IMeterInfoDetailQueryService _meterInfoDetailQueryService;
        private readonly INerkhCacheService _nerkhGetByConsumptionService;
        private readonly IZaribCCacheService _zaribCQueryService;
        private readonly ITable1CacheService _table1QueryService;
        private readonly IBillIdTagService _tagService;
        private readonly IBedBesQueryService _billQueryService;
        private readonly IConsumptionCalculator _consumptionCalculator;

        private readonly IAbBahaCalculator _abBahaCalculator;
        private readonly IFazelabCalculator _fazelabCalculator;
        private readonly IHotSeasonCalculator _hotSeasonCalculator;
        private readonly IBudgetCalculator _budgetCalculator;
        private readonly IJavaniJamiatCalculator _javaniJamiatCalculator;
        private readonly IAvarezCalculator _avarezCalculator;
        private readonly IAbonmanCalculator _abonmanCalculator;
        private readonly ITaxCalculator _taxCalculator;

        public OldTariffEngine(
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IMeterInfoDetailQueryService meterInfoDetailQueryService,
            INerkhCacheService nerkhGetByConsumptionService,
            IZaribCCacheService zaribCQueryService,
            ITable1CacheService table1GetService,
            IBillIdTagService tagService,
            IConsumptionCalculator consumptionCalculator,
            IAbBahaCalculator abBahaCalculator,
            IBedBesQueryService billQueryService,
            IFazelabCalculator fazelabCalculator,
            IHotSeasonCalculator hotSeasonCalculator,
            IAbonmanCalculator abonmanCalculator,
            IBudgetCalculator budgetCalculator,
            IJavaniJamiatCalculator javaniJamiatCalculator,
            IAvarezCalculator avarezCalculator,
            ITaxCalculator taxCalculator)
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

            _taxCalculator = taxCalculator;
            _taxCalculator.NotNull(nameof(_taxCalculator));
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
            CustomerInfoOutputDto customerInfo = new (input);
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
                throw new BaseException($"{e.Message}. {customerInfo.BillId} {input.MeterPreviousData.PreviousDateJalali} {input.MeterPreviousData.PreviousNumber}");
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
            return new NerkhByConsumptionInputDto(
                _zoneId: customerInfo.ZoneId,
                _usageId: customerInfo.UsageId,
                _previousDate: meterInfo.PreviousDateJalali,
                _currentDate: meterInfo.CurrentDateJalali,
                _average: consumptionInfo.MonthlyAverageConsumption);
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaCalculationDetails(IEnumerable<NerkhGetDto> allNerkh, IEnumerable<AbAzadFormulaDto> abAzad, IEnumerable<ZaribGetDto> zarib, string currentDateJalali, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ConsumptionInfo consumptionInfo, IEnumerable<NerkhGetDto> nerkh1403)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int counter = 0;
            double sumAbBaha = 0, sumFazelab = 0, sumHotSeasonAbBaha = 0, sumHotSeasonFazelab = 0;
            double sumBoodjePart1 = 0, sumBoodjePart2 = 0, sumAvarez = 0;
            double sumAbBahaDiscount = 0, sumFazelabDiscount = 0, sumHotSeasonAbDiscount = 0, sumHotSeasonFazelabDiscount = 0,
                   sumAvarezDiscount = 0, sumJavaniDiscount = 0, sumBoodjeDiscount = 0;
            double sumJavaniAmount = 0; double multiplier = 0;
            IEnumerable<int> tags = await _tagService.GetIdsByBillId(customerInfo.BillId.Trim());
            Table1GetDto table1 = await _table1QueryService.GetByTown(customerInfo.ZoneId);
            foreach (var nerkhItem in allNerkh)
            {
                AbAzadFormulaDto abAzadItem = abAzad.ElementAt(counter);
                ZaribGetDto zaribItem = zarib.ElementAt(counter);
                ZaribCQueryDto zaribC = await GetZaribC(nerkhItem);
                ConsumptionInfo partialConsumptionInfo = new(nerkhItem.Date1, nerkhItem.Date2, consumptionInfo.Consumption, consumptionInfo.Duration, consumptionInfo.DailyAverageConsumption, consumptionInfo.FinalDomesticUnit);
                BaseOldTariffEngineOutputDto resultCalc = CalculateWaterBill(nerkhItem, nerkh1403.ElementAt(counter), abAzadItem, zaribItem, customerInfo, meterInfo, currentDateJalali, partialConsumptionInfo, table1.olgo, zaribC is not null ? zaribC.C : null, tags);
                nerkhItem.CalcVaj = resultCalc.AbBahaValues.Summation.ToString();
                sumAbBaha += resultCalc.AbBahaValues.Summation;
                sumFazelab += resultCalc.FazelabAmount;
                sumBoodjePart1 += resultCalc.BoodjePart1;
                sumBoodjePart2 += resultCalc.BoodjePart2;
                sumHotSeasonAbBaha += resultCalc.HotSeasonAbBahaAmount;
                sumHotSeasonFazelab += resultCalc.HotSeasonFazelabAmount;
                sumAbBahaDiscount += resultCalc.AbBahaDiscount;
                sumFazelabDiscount += resultCalc.FazelabDiscount;
                sumHotSeasonAbDiscount += resultCalc.HotSeasonDiscount;
                sumHotSeasonFazelabDiscount += resultCalc.HotSeasonFazelabDiscount;
                sumAvarez += resultCalc.AvarezAmount;
                sumJavaniAmount += resultCalc.JavaniAmount;
                sumAvarezDiscount += resultCalc.AvarezDiscount;
                sumBoodjeDiscount += resultCalc.BoodjeDiscount;
                sumJavaniDiscount += resultCalc.JavaniDiscount;

                counter++;
            }
            ConsumptionPartialInfo consumptionPartialInfo = _consumptionCalculator.GetPartial(meterInfo, consumptionInfo, meterInfo.PreviousDateJalali, meterInfo.CurrentDateJalali, IsDomestic(customerInfo.UsageId) ?  GetOlgoo(meterInfo.CurrentDateJalali, table1.olgo): customerInfo.ContractualCapacity);

            TariffItemResult sumAbonmanAbBaha = _abonmanCalculator.CalculateAb(customerInfo, meterInfo, currentDateJalali, consumptionPartialInfo, out double before1403_12_02, out double before1404);
            TariffItemResult sumAbonmanFazelab = _fazelabCalculator.Calculate(meterInfo.PreviousDateJalali, currentDateJalali, consumptionInfo.Duration, customerInfo, sumAbonmanAbBaha.Summation, currentDateJalali, true, consumptionPartialInfo, abCalcResult: sumAbonmanAbBaha, out double multiplierFazelab);
            TariffItemResult sumAbonmanAbDiscount = _abonmanCalculator.CalculateDiscount(customerInfo.UsageId, customerInfo.BranchType, sumAbonmanAbBaha.Summation, sumAbBahaDiscount, customerInfo.IsSpecial, consumptionInfo, customerInfo, consumptionPartialInfo, sumAbonmanAbBaha.Allowed, sumAbonmanAbBaha, before1403_12_02, before1404);
            TariffItemResult sumAbonmanFazelabDiscount = _abonmanCalculator.CalculateDiscount(customerInfo.UsageId, customerInfo.BranchType, sumAbonmanFazelab.Summation, sumFazelabDiscount, customerInfo.IsSpecial, consumptionInfo, customerInfo, consumptionPartialInfo, sumAbonmanFazelab.Allowed, sumAbonmanFazelab, before1403_12_02 * multiplierFazelab, before1404);

            double AbBahaResult = sumAbBaha + sumHotSeasonAbBaha + sumAbonmanAbBaha.Summation;
            double sumBoodje = sumBoodjePart1 + sumBoodjePart2;
            TariffItemResult sumMaliatAmount = _taxCalculator.Calculate(sumAbBaha, sumFazelab, sumAbonmanAbBaha.Summation, sumAbonmanFazelab.Summation, sumHotSeasonAbBaha, sumHotSeasonFazelab, sumBoodje);
            TariffItemResult maliatDiscount = _taxCalculator.CalculateDiscount(sumAbBahaDiscount, sumFazelabDiscount, sumAbonmanAbDiscount.Summation, sumAbonmanFazelabDiscount.Summation, sumHotSeasonAbDiscount, sumHotSeasonFazelabDiscount, sumBoodjeDiscount);

            stopWatch.Stop();
            return new AbBahaCalculationDetails(Math.Round(AbBahaResult), Math.Round(sumAbBaha), Math.Round(sumFazelab), Math.Round(sumBoodjePart1), Math.Round(sumBoodjePart2), Math.Round(sumBoodje), Math.Round(sumHotSeasonAbBaha), Math.Round(sumHotSeasonFazelab), Math.Round(sumAbBahaDiscount), Math.Round(sumHotSeasonAbDiscount), Math.Round(sumFazelabDiscount), Math.Round(sumAbonmanAbBaha.Summation), Math.Round(sumAvarez), Math.Round(sumJavaniAmount), Math.Round(sumMaliatAmount.Summation), Math.Round(sumAbonmanFazelab.Summation),
                       Math.Round(sumAbonmanAbDiscount.Summation), Math.Round(sumAbonmanFazelabDiscount.Summation), Math.Round(sumAvarezDiscount), Math.Round(sumJavaniDiscount), Math.Round(maliatDiscount.Summation), Math.Round(sumBoodjeDiscount), allNerkh, abAzad, zarib, consumptionInfo, meterInfo, customerInfo, stopWatch.ElapsedMilliseconds, sumHotSeasonFazelabDiscount);
        }       
        private BaseOldTariffEngineOutputDto CalculateWaterBill(NerkhGetDto nerkh, NerkhGetDto nerkh1403, AbAzadFormulaDto abAzad, ZaribGetDto zarib, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali, ConsumptionInfo consumptionInfo, int _olgoo, [Optional] int? c, [Optional] IEnumerable<int>? tagIds)
        {
            ConsumptionPartialInfo consumptionPartialInfo = _consumptionCalculator.GetPartial(meterInfo, consumptionInfo, nerkh.Date1, nerkh.Date2, IsDomestic(customerInfo.UsageId) ? _olgoo : customerInfo.ContractualCapacity);
          
            int olgoo = GetOlgoo(consumptionPartialInfo.EndDateJalali, _olgoo);
            bool isVillageCalculation = IsVillage(customerInfo.ZoneId);
            double monthlyConsumption = consumptionInfo.DailyAverageConsumption * monthDays; 

            TariffItemResult abBahaResult = _abBahaCalculator.Calculate(nerkh, nerkh1403, customerInfo, meterInfo, zarib, abAzad, consumptionPartialInfo, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, c, tagIds, out double villageMultiplier);
            TariffItemResult boodje = _budgetCalculator.Calculate(consumptionPartialInfo, customerInfo, currentDateJalali, monthlyConsumption, olgoo, consumptionInfo);
            TariffItemResult fazelab = _fazelabCalculator.Calculate(consumptionPartialInfo.StartDateJalali, consumptionPartialInfo.EndDateJalali, consumptionPartialInfo.Duration, customerInfo, abBahaResult.Summation, currentDateJalali, false, consumptionPartialInfo, abBahaResult, out double multiplierFazelab);
            TariffItemResult hotSeasonAbBaha = _hotSeasonCalculator.CalculateAb(abBahaResult.Summation, customerInfo, monthlyConsumption, abBahaResult, consumptionPartialInfo, isVillageCalculation, villageMultiplier);
            TariffItemResult hotSeasonFazelab = _hotSeasonCalculator.CalcFazelab(customerInfo, fazelab.Summation, monthlyConsumption, fazelab, consumptionPartialInfo, isVillageCalculation, villageMultiplier);
            TariffItemResult avarez = _avarezCalculator.Calculate(consumptionPartialInfo, customerInfo, monthlyConsumption);
            TariffItemResult javani = _javaniJamiatCalculator.Calculate(consumptionPartialInfo, customerInfo, abBahaResult.Summation, monthlyConsumption, olgoo);

            //Discounts
            TariffItemResult abBahaDiscount = _abBahaCalculator.CalculateDiscount(consumptionPartialInfo ,zarib, isVillageCalculation, monthlyConsumption, customerInfo, nerkh, olgoo, abBahaResult, consumptionInfo.FinalDomesticUnit);
            TariffItemResult fazelabDiscount = _fazelabCalculator.CalculateDiscount(fazelab, abBahaDiscount.Summation, fazelab.Summation, customerInfo, consumptionPartialInfo);
            TariffItemResult hotSeasonAbDiscount = _hotSeasonCalculator.CalculateDiscount(abBahaDiscount.Summation, hotSeasonAbBaha, customerInfo, abBahaResult, consumptionPartialInfo);
            TariffItemResult hotSeasonFazelabDiscount = _hotSeasonCalculator.CalculateDiscount(fazelabDiscount.Summation, hotSeasonFazelab, customerInfo, abBahaResult, consumptionPartialInfo);
            TariffItemResult boodjeDiscount = _budgetCalculator.CalculateDiscount(consumptionPartialInfo,customerInfo, abBahaDiscount.Summation, boodje);
            TariffItemResult avarezDiscount = _avarezCalculator.CalculateDiscount();
            TariffItemResult javaniDiscount= _javaniJamiatCalculator.CalculateDiscount(customerInfo,consumptionPartialInfo, javani);

            //Mullah again


            return new BaseOldTariffEngineOutputDto(
                abBahaValues: abBahaResult,
                fazelabAmount: fazelab.Summation,
                hotSeasonAbBahaAmount: hotSeasonAbBaha.Summation,
                hotSeasonFazelabAmount: hotSeasonFazelab.Summation,
                boodjePart1: boodje.Allowed,
                boodjePart2: boodje.Disallowed,
                abBahaDiscount: abBahaDiscount.Summation,
                hotSeasonDiscount: hotSeasonAbDiscount.Summation,
                fazelabDiscount: fazelabDiscount.Summation,
                abonmanAbAmount: 0,
                avarezAmount: avarez.Summation,
                javaniAmount: javani.Summation,
                abonmanAbDiscount: 0,
                abonamenFazelabDiscount: 0,
                avarezDiscount: avarezDiscount.Summation,
                javaniDiscount: javaniDiscount.Summation,
                boodjeDiscount: boodjeDiscount.Summation,
                hotSeasonFazelabDiscount: hotSeasonFazelabDiscount.Summation,
                multiplier: 0,/*TODO*/
                fazelab:fazelab,
                hotSeasonAb: hotSeasonAbBaha,
                hotSeasonFazelab: hotSeasonFazelab,
                boodje: boodje);
        }

        private int GetOlgoo(string nerkhDate2, int olgo)
        {
            string baseDate = "1403/12/30";
            return nerkhDate2.CompareTo(baseDate) <= 0 ? 14 : olgo;
        }
        private async Task<ZaribCQueryDto> GetZaribC(NerkhGetDto nerkh)
        {
            string date1402_12_28 = "1402/12/28";
            string date1402_12_29 = "1402/12/29";
            string date1403_06_25 = "1403/12/25";
            string @from = "1402/12/28".MoreOrEq(nerkh.Date1) ? date1402_12_29 : nerkh.Date1;
            string @to = "1402/12/28".MoreOrEq(nerkh.Date2) ? date1403_06_25 : nerkh.Date2;
            ZaribCQueryDto zaribCQueryDto = await _zaribCQueryService.GetZaribC(nerkh.Date1, nerkh.Date2);
            return zaribCQueryDto;
        }
    }
}