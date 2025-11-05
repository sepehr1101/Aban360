using Aban360.CalculationPool.Application.Features.Base;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Aban360.ReportPool.Persistence.Features.Tagging;
using DNTPersianUtils.Core;
using System.Diagnostics;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class OldTariffEngine : BaseOldTariffEngine, IOldTariffEngine
    {
        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly IMeterInfoDetailQueryService _meterInfoDetailQueryService;
        private readonly INerkhGetByConsumptionService _nerkhGetByConsumptionService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly ITable1GetService _table1QueryService;
        private readonly IBillIdTagService _tagService;

        int thresholdDay = 4;
        int constructionBranchType = 4;
        int azadUsageId = 39;
        int monthDays = 30;
        float vatRate = 0.1f;

        public OldTariffEngine(
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IMeterInfoDetailQueryService meterInfoDetailQueryService,
            INerkhGetByConsumptionService nerkhGetByConsumptionService,
            IZaribCQueryService zaribCQueryService,
            ITable1GetService table1GetService,
            IBillIdTagService tagService)
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
        }

        public async Task<AbBahaCalculationDetails> Handle(MeterInfoInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            MeterInfoOutputDto meterInfo = await _meterInfoDetailQueryService.GetInfo(new CustomerInfoInputDto(customerInfo.ZoneId, customerInfo.Radif));
            Validate(meterInfo.PreviousDateJalali);
            ConsumptionInfo consumptionInfo = GetConsumptionInfo(input, customerInfo, meterInfo);
            NerkhByConsumptionInputDto nerkgInput = CreateNerkhInput(input, customerInfo, meterInfo, consumptionInfo);
            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(nerkgInput);
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, consumptionInfo.DailyAverageConsumption, input.CurrentDateJalali, customerInfo, meterInfo, consumptionInfo.Duration, consumptionInfo.Consumption, consumptionInfo.FinalDomesticUnit);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            result.MonthlyConsumption = consumptionInfo.MonthlyAverageConsumption;
            result.DailyConsumption = consumptionInfo.DailyAverageConsumption;
            result.Duration = consumptionInfo.Duration;
            return result;
        }       
        public async Task<AbBahaCalculationDetails> Handle(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            Validate(input.PreviousDateJalali);
            ConsumptionInfo consumptionInfo = GetConsumptionInfo(input, customerInfo);
            NerkhByConsumptionInputDto nerkhInfo = CreateNerkhInput(input, customerInfo, consumptionInfo);
            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(nerkhInfo);
            MeterInfoOutputDto meterInfo = new MeterInfoOutputDto()
            {
                PreviousDateJalali = input.PreviousDateJalali,
                PreviousNumber = input.PreviousNumber,
            };
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, consumptionInfo.DailyAverageConsumption, input.CurrentDateJalali, customerInfo, meterInfo, consumptionInfo.Duration, consumptionInfo.Consumption, consumptionInfo.Duration);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            result.MonthlyConsumption = consumptionInfo.MonthlyAverageConsumption;
            result.DailyConsumption = consumptionInfo.DailyAverageConsumption;
            result.Duration = consumptionInfo.Duration;
            result.Consumption = consumptionInfo.Consumption;
            return result;
        }
        public async Task<AbBahaCalculationDetails> Handle(BaseOldTariffEngineImaginaryInputDto input, CancellationToken cancellationToken)
        {
            //TODO: direct create object from MeterComparisonBatchWithAggregatedNerkhGetHandler.cs
            CustomerInfoOutputDto customerInfo = CreateCustomerInfoDto(input);
            try
            {
                Validate(input.MeterPreviousData.PreviousDateJalali);

                int consumption = GetConsumption(input.MeterPreviousData.PreviousNumber, input.MeterPreviousData.CurrentMeterNumber);
                int duration = GetDuration(input.MeterPreviousData.PreviousDateJalali, input.MeterPreviousData.CurrentDateJalali);
                int finalDomesticUnit = GetFinalDomesticUnit(customerInfo, input.MeterPreviousData.CurrentDateJalali);
                double dailyAverage = GetDailyConsumptionAverage(consumption, duration, finalDomesticUnit);
                double monthlyAverageConsumption = dailyAverage * monthDays;

                (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(new NerkhByConsumptionInputDto(customerInfo.ZoneId,
                                                                                                                           customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                                                                                                                           input.MeterPreviousData.PreviousDateJalali,
                                                                                                                           input.MeterPreviousData.CurrentDateJalali,
                                                                                                                           monthlyAverageConsumption));
                MeterInfoOutputDto meterInfo = new MeterInfoOutputDto()
                {
                    PreviousDateJalali = input.MeterPreviousData.PreviousDateJalali,
                    PreviousNumber = input.MeterPreviousData.PreviousNumber,
                };

                AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, input.MeterPreviousData.CurrentDateJalali, customerInfo, meterInfo, duration, consumption, finalDomesticUnit);
                result.Customer = customerInfo;
                result.MeterInfo = meterInfo;
                result.MonthlyConsumption = monthlyAverageConsumption;
                result.DailyConsumption = dailyAverage;
                result.Duration = duration;
                result.Consumption = consumption;
                return result;
            }
            catch (Exception e)
            {
                throw new BaseException($"{customerInfo.BillId} {input.MeterPreviousData.PreviousDateJalali} {input.MeterPreviousData.PreviousNumber}");
            }
        }

        private async Task<AbBahaCalculationDetails> GetAbBahaCalculationDetails(IEnumerable<NerkhGetDto> allNerkh, IEnumerable<AbAzadFormulaDto> abAzad, IEnumerable<ZaribGetDto> zarib, double dailyAverage, string currentDateJalali, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, int duration, int consumption, int finalDomesticCount)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int counter = 0;
            double sumAbBaha = 0, sumFazelab = 0, sumHotSeasonAbBaha = 0, sumHotSeasonFazelab = 0, sumAbonmanAbBaha = 0, sumAbonmanFazelab = 0;
            double sumBoodjePart1 = 0, sumBoodjePart2 = 0, sumAvarez = 0;
            double sumAbBahaDiscount = 0, sumFazelabDiscount = 0, sumHotSeasonDiscount = 0,
                   sumAbonmanAbDiscount = 0, sumAbonmanFazelabDiscount = 0, sumAvarezDiscount = 0, sumJavaniDiscount = 0, sumBoodjeDiscount = 0;
            double sumJavaniAmount = 0;
            IEnumerable<int> tags = await _tagService.GetIdsByBillId(customerInfo.BillId.Trim());
            Table1GetDto table1 = await _table1QueryService.GetByTown(customerInfo.ZoneId);
            foreach (var nerkhItem in allNerkh)
            {
                AbAzadFormulaDto abAzadItem = abAzad.ElementAt(counter);
                ZaribGetDto zaribItem = zarib.ElementAt(counter);
                ZaribCQueryDto zaribC = await _zaribCQueryService.GetZaribC(nerkhItem.Date1, nerkhItem.Date2);
                nerkhItem.C = zaribC.C;
                ConsumptionInfo partialConsumptionInfo = new(nerkhItem.Date1, nerkhItem.Date2, consumption, duration, dailyAverage, finalDomesticCount);
                BaseOldTariffEngineOutputDto resultCalc = CalculateWaterBill(nerkhItem, abAzadItem, zaribItem, customerInfo, meterInfo, currentDateJalali, partialConsumptionInfo, table1.olgo, zaribC is not null ? zaribC.C : null, tags);
                nerkhItem.CalcVaj = resultCalc.AbBahaValues.AbBahaAmount.ToString();
                sumAbBaha += resultCalc.AbBahaValues.AbBahaAmount;
                sumFazelab += resultCalc.FazelabAmount;
                sumBoodjePart1 += resultCalc.BoodjePart1;
                sumBoodjePart2 += resultCalc.BoodjePart2;
                sumHotSeasonAbBaha = resultCalc.HotSeasonAbBahaAmount;
                sumHotSeasonFazelab += resultCalc.HotSeasonFazelabAmount;
                sumAbBahaDiscount += resultCalc.AbBahaDiscount;
                sumFazelabDiscount += resultCalc.FazelabDiscount;
                sumHotSeasonDiscount += resultCalc.HotSeasonDiscount;
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
            sumAbonmanAbBaha = CalculateAbonmanAb(customerInfo, meterInfo, currentDateJalali);
            sumAbonmanFazelab = CalculateFazelab(meterInfo.PreviousDateJalali, currentDateJalali, duration, customerInfo, sumAbonmanAbBaha, currentDateJalali, true);
            sumAbonmanAbDiscount = CalculateAbonmanDiscount(customerInfo.UsageId, sumAbonmanAbBaha, sumAbBahaDiscount, customerInfo.IsSpecial);
            sumAbonmanFazelabDiscount = CalculateAbonmanDiscount(customerInfo.UsageId, sumAbonmanFazelab, sumFazelabDiscount, customerInfo.IsSpecial);
            double maliatDiscount = CalculateTaxDiscount(sumAbBahaDiscount, sumFazelabDiscount, sumAbonmanAbDiscount,
                sumAbonmanFazelabDiscount, sumBoodjeDiscount, sumHotSeasonDiscount);
            double AbBahaResult = sumAbBaha + sumHotSeasonAbBaha + sumAbonmanAbBaha;
            double sumBoodje = sumBoodjePart1 + sumBoodjePart2;
            double sumMaliatAmount = CalculateTax(sumAbBaha, sumAbonmanAbBaha, sumHotSeasonAbBaha, sumFazelab, sumAbonmanFazelab, sumHotSeasonFazelab, sumBoodje);

            stopWatch.Stop();
            return new AbBahaCalculationDetails(Math.Round(AbBahaResult), Math.Round(sumAbBaha), Math.Round(sumFazelab), Math.Round(sumBoodjePart1), Math.Round(sumBoodjePart2), Math.Round(sumBoodje), Math.Round(sumHotSeasonAbBaha), Math.Round(sumHotSeasonFazelab), Math.Round(sumAbBahaDiscount), Math.Round(sumHotSeasonDiscount), Math.Round(sumFazelabDiscount), Math.Round(sumAbonmanAbBaha), Math.Round(sumAvarez), Math.Round(sumJavaniAmount), Math.Round(sumMaliatAmount), Math.Round(sumAbonmanFazelab),
                       Math.Round(sumAbonmanAbDiscount), Math.Round(sumAbonmanFazelabDiscount), Math.Round(sumAvarezDiscount), Math.Round(sumJavaniDiscount), Math.Round(maliatDiscount), Math.Round(sumBoodjeDiscount), allNerkh, abAzad, zarib, stopWatch.ElapsedMilliseconds);
        }
        private CustomerInfoOutputDto CreateCustomerInfoDto(BaseOldTariffEngineImaginaryInputDto input)
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
                VirtualCategoryId = input.CustomerInfo.VirtualCategoryId ?? 0
            };
        }

     
        private int GetConsumption(int previousNumber, int currentNumber)
        {
            return currentNumber - previousNumber;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            int duration = (currentGregorian.Value - previousGregorian.Value).Days;
            if (duration <= thresholdDay)
            {
                throw new InvalidBillIdException(Literals.InvalidDuration);
            }
            return duration;
        }
        private double GetDailyConsumptionAverage(int masraf, int duration, int domesticUnit)
        {
            return masraf / (double)duration / domesticUnit;
        }

        private void Validate(string previousDateJalali)
        {
            DateOnly? previousDate = previousDateJalali.ToGregorianDateOnly();
            if (!previousDate.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }
            if (previousDate.Value > DateOnly.FromDateTime(DateTime.Now.AddDays(-thresholdDay)))
            {
                throw new BaseException(ExceptionLiterals.InvalidPreviousDateInvoice(thresholdDay));
            }
        }
        private int GetFinalDomesticUnit(CustomerInfoOutputDto customerInfo, string readingDateJalali)
        {
            int finalHousehold = GetHouseholdUnit(customerInfo.HouseholdNumber, customerInfo.HouseholdDate, readingDateJalali);
            if (finalHousehold > 0)
            {
                return finalHousehold;
            }
            return (customerInfo.DomesticUnit - customerInfo.EmptyUnit) < 1 ? 1 : (customerInfo.DomesticUnit - customerInfo.EmptyUnit);

            int GetHouseholdUnit(int householdUnit, string? householdDate, string readingDateJalali)
            {
                if (householdUnit <= 0)
                {
                    return 0;
                }
                if (string.IsNullOrWhiteSpace(householdDate))
                {
                    return 0;
                }
                DateTime? expireHouseHoldGregorian = householdDate.ToGregorianDateTime();
                if (!expireHouseHoldGregorian.HasValue)
                {
                    return 0;
                }
                DateTime? readingDateGregorian = readingDateJalali.ToGregorianDateTime();
                if (!readingDateGregorian.HasValue)
                {
                    throw new InvalidDateException(readingDateJalali);
                }
                if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value)
                {
                    return 0;
                }
                return householdUnit;
            }
        }
        private double CalculateTax(double abBahaAmount, double abonmanAbBahaAmount, double hotseasonAbBahaAmount, double fazelabAmount, double abonmanFazelabAmount, double hotseasonFazelabAmount, double boodjeAmount)
        {
            double sumAmount = abBahaAmount + abonmanAbBahaAmount + hotseasonAbBahaAmount + fazelabAmount + abonmanFazelabAmount + hotseasonFazelabAmount + boodjeAmount;
            return sumAmount * vatRate;
        }
        private double CalculateTaxDiscount(double abBahaDiscount, double fazelabDiscount, double abonAbDiscount,
            double abonFazelabDiscount, double boodjeDiscount, double hotSeasonDiscount)
        {
            return 0.1 * (abBahaDiscount + fazelabDiscount + abonAbDiscount + abonFazelabDiscount + boodjeDiscount + hotSeasonDiscount);
        }
        private ConsumptionInfo GetConsumptionInfo(MeterInfoInputDto input, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo)
        {
            int consumption = GetConsumption(meterInfo.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(meterInfo.PreviousDateJalali, input.CurrentDateJalali);
            int finalDomesticUnit = GetFinalDomesticUnit(customerInfo, input.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, finalDomesticUnit);
            ConsumptionInfo consumptionInfo = new(meterInfo.PreviousDateJalali, input.CurrentDateJalali, consumption, duration, dailyAverage, finalDomesticUnit);
            return consumptionInfo;
        }
        private ConsumptionInfo GetConsumptionInfo(MeterInfoByPreviousDataInputDto input, CustomerInfoOutputDto customerInfo)
        {
            int consumption = GetConsumption(input.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(input.PreviousDateJalali, input.CurrentDateJalali);
            int finalDomesticUnit = GetFinalDomesticUnit(customerInfo, input.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, finalDomesticUnit);
            ConsumptionInfo consumptionInfo = new(input.PreviousDateJalali, input.CurrentDateJalali, consumption, duration, dailyAverage, finalDomesticUnit);
            return consumptionInfo;
        }
        private NerkhByConsumptionInputDto CreateNerkhInput(MeterInfoInputDto input, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ConsumptionInfo consumptionInfo)
        {
            return new NerkhByConsumptionInputDto(
                customerInfo.ZoneId,
                customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                meterInfo.PreviousDateJalali,
                input.CurrentDateJalali,
                consumptionInfo.MonthlyAverageConsumption);
        }
        private NerkhByConsumptionInputDto CreateNerkhInput(MeterInfoByPreviousDataInputDto input, CustomerInfoOutputDto customerInfo, ConsumptionInfo consumptionInfo)
        {
            return new NerkhByConsumptionInputDto(
                customerInfo.ZoneId,
                customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                input.PreviousDateJalali,
                input.CurrentDateJalali,
                consumptionInfo.MonthlyAverageConsumption);
        }
    }
}