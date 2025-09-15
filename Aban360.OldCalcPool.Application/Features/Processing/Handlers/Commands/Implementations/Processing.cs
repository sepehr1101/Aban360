using Aban360.CalculationPool.Application.Features.Base;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
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
    internal sealed class Processing : BaseOldTariffEngine, IProcessing
    {
        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly IMeterInfoDetailQueryService _meterInfoDetailQueryService;
        private readonly INerkhGetByConsumptionService _nerkhGetByConsumptionService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IBillIdTagService _tagService;

        int thresholdDay = 4;
        int constructionBranchType = 4;
        int azadUsageId = 39;
        int monthDays = 30;

        public Processing(
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IMeterInfoDetailQueryService meterInfoDetailQueryService,
            INerkhGetByConsumptionService nerkhGetByConsumptionService,
            IZaribCQueryService zaribCQueryService,
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

            _tagService = tagService;
            _tagService.NotNull(nameof(_tagService));
        }

        private int GetConsumption(int previousNumber, int currentNumber)
        {
            return currentNumber - previousNumber;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            int duration= (currentGregorian.Value - previousGregorian.Value).Days;
            if (duration <= thresholdDay)
            {
                throw new InvalidBillIdException(Literals.InvalidDuration);
            }
            return duration;
        }
        private double GetDailyConsumptionAverage(int masraf, int duration, int domesticUnit, int emptyUnit)
        {
            int domesticUnitTemp = (domesticUnit - emptyUnit) < 1 ? 1 : (domesticUnit - emptyUnit);
            return masraf / (double)duration / domesticUnitTemp;
        }
        private void Validation(string previousDateJalali)
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

        public async Task<AbBahaCalculationDetails> Handle(MeterInfoInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            MeterInfoOutputDto meterInfo = await _meterInfoDetailQueryService.GetInfo(new CustomerInfoInputDto(customerInfo.ZoneId, customerInfo.Radif));
            Validation(meterInfo.PreviousDateJalali);

            int consumption = GetConsumption(meterInfo.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(meterInfo.PreviousDateJalali, input.CurrentDateJalali);         
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, customerInfo.DomesticUnit, customerInfo.EmptyUnit);
            double monthlyAverageConsumption = dailyAverage * monthDays;

            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.Get(new NerkhByConsumptionInputDto(customerInfo.ZoneId,
                                                                                                                       customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                                                                                                                       meterInfo.PreviousDateJalali,
                                                                                                                       input.CurrentDateJalali,
                                                                                                                       monthlyAverageConsumption));
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, input.CurrentDateJalali, customerInfo, meterInfo, duration, consumption);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            result.MonthlyConsumption = monthlyAverageConsumption;
            result.DailyConsumption = dailyAverage;
            result.Duration = duration;
            return result;
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            Validation(input.PreviousDateJalali);

            int consumption = GetConsumption(input.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(input.PreviousDateJalali, input.CurrentDateJalali);           
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, customerInfo.DomesticUnit, customerInfo.EmptyUnit);
            double monthlyAverageConsumption = dailyAverage * monthDays;

            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>,int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.Get(new NerkhByConsumptionInputDto(
                                                                                                                      customerInfo.ZoneId,
                                                                                                                      customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                                                                                                                      input.PreviousDateJalali,
                                                                                                                      input.CurrentDateJalali,
                                                                                                                      monthlyAverageConsumption));
            MeterInfoOutputDto meterInfo = new MeterInfoOutputDto()
            {
                PreviousDateJalali = input.PreviousDateJalali,
                PreviousNumber = input.PreviousNumber,
            };
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, input.CurrentDateJalali, customerInfo, meterInfo, duration, consumption);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            result.MonthlyConsumption = monthlyAverageConsumption;
            result.DailyConsumption = dailyAverage;
            result.Duration = duration;
            result.Consumption = consumption;
            return result;
        }
        public async Task<AbBahaCalculationDetails> Handle(BaseOldTariffEngineImaginaryInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = GetCustomerInfo(input);
            Validation(input.MeterPreviousData.PreviousDateJalali);

            int consumption = GetConsumption(input.MeterPreviousData.PreviousNumber, input.MeterPreviousData.CurrentMeterNumber);
            int duration = GetDuration(input.MeterPreviousData.PreviousDateJalali, input.MeterPreviousData.CurrentDateJalali);           
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, customerInfo.DomesticUnit, customerInfo.EmptyUnit);
            double monthlyAverageConsumption = dailyAverage * monthDays;

            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>,int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.Get(new NerkhByConsumptionInputDto(customerInfo.ZoneId,
                                                                                                                      customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                                                                                                                       input.MeterPreviousData.PreviousDateJalali,
                                                                                                                       input.MeterPreviousData.CurrentDateJalali,
                                                                                                                       monthlyAverageConsumption));
            MeterInfoOutputDto meterInfo = new MeterInfoOutputDto()
            {
                PreviousDateJalali = input.MeterPreviousData.PreviousDateJalali,
                PreviousNumber = input.MeterPreviousData.PreviousNumber,
            };

            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, input.MeterPreviousData.CurrentDateJalali, customerInfo, meterInfo, duration, consumption);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            result.MonthlyConsumption = monthlyAverageConsumption;
            result.DailyConsumption = dailyAverage;
            result.Duration = duration;
            result.Consumption = consumption;
            return result;
        }
       
        public async Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(MeterInfoInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            MeterInfoOutputDto meterInfo = await _meterInfoDetailQueryService.GetInfo(new CustomerInfoInputDto(customerInfo.ZoneId, customerInfo.Radif));
            Validation(meterInfo.PreviousDateJalali);

            int consumption = GetConsumption(meterInfo.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(meterInfo.PreviousDateJalali, input.CurrentDateJalali);         
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, customerInfo.DomesticUnit, customerInfo.EmptyUnit);
            double monthlyAverageConsumption = dailyAverage * monthDays;

            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(new NerkhByConsumptionInputDto(customerInfo.ZoneId,
                                                                                                                      customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                                                                                                                       meterInfo.PreviousDateJalali,
                                                                                                                       input.CurrentDateJalali,
                                                                                                                       monthlyAverageConsumption));
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, input.CurrentDateJalali, customerInfo, meterInfo, duration, consumption);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            result.MonthlyConsumption = monthlyAverageConsumption;
            result.DailyConsumption = dailyAverage;
            result.Duration = duration;
            return result;
        }
        public async Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            Validation(input.PreviousDateJalali);

            int consumption = GetConsumption(input.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(input.PreviousDateJalali, input.CurrentDateJalali);           
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, customerInfo.DomesticUnit, customerInfo.EmptyUnit);
            double monthlyAverageConsumption = dailyAverage * monthDays;

            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(new NerkhByConsumptionInputDto(
                                                                                                                      customerInfo.ZoneId,
                                                                                                                      customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                                                                                                                      input.PreviousDateJalali,
                                                                                                                      input.CurrentDateJalali,
                                                                                                                      monthlyAverageConsumption));
            MeterInfoOutputDto meterInfo = new MeterInfoOutputDto()
            {
                PreviousDateJalali = input.PreviousDateJalali,
                PreviousNumber = input.PreviousNumber,
            };
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, input.CurrentDateJalali, customerInfo, meterInfo, duration, consumption);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            result.MonthlyConsumption = monthlyAverageConsumption;
            result.DailyConsumption = dailyAverage;
            result.Duration = duration;
            result.Consumption = consumption;
            return result;
        }
        public async Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(BaseOldTariffEngineImaginaryInputDto input, CancellationToken cancellationToken)
        {
            try
            {
                CustomerInfoOutputDto customerInfo = GetCustomerInfo(input);
                Validation(input.MeterPreviousData.PreviousDateJalali);

                int consumption = GetConsumption(input.MeterPreviousData.PreviousNumber, input.MeterPreviousData.CurrentMeterNumber);
                int duration = GetDuration(input.MeterPreviousData.PreviousDateJalali, input.MeterPreviousData.CurrentDateJalali);
                double dailyAverage = GetDailyConsumptionAverage(consumption, duration, customerInfo.DomesticUnit, customerInfo.EmptyUnit);
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

                AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, input.MeterPreviousData.CurrentDateJalali, customerInfo, meterInfo, duration, consumption);
                result.Customer = customerInfo;
                result.MeterInfo = meterInfo;
                result.MonthlyConsumption = monthlyAverageConsumption;
                result.DailyConsumption = dailyAverage;
                result.Duration = duration;
                result.Consumption = consumption;
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private async Task<AbBahaCalculationDetails> GetAbBahaCalculationDetails(IEnumerable<NerkhGetDto> allNerkh, IEnumerable<AbAzadFormulaDto> abAzad, IEnumerable<ZaribGetDto> zarib, double dailyAverage, string currentDateJalali, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, int duration, int consumption)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int counter = 0;
            double sumAbBaha = 0, sumFazelab = 0, sumHotSeasonAbBaha = 0, sumHotSeasonFazelab = 0, sumAbonmanAbBaha = 0;
            double sumBoodjePart1 = 0, sumBoodjePart2 = 0, sumAvarez = 0;
            double sumAbBahaDiscount = 0, sumFazelabDiscount = 0, sumHotSeasonDiscount = 0,
                   sumAbonmanAbDiscount = 0, sumAbonmanFazelabDiscount=0, sumAvarezDiscount=0, sumJavaniDiscount=0, sumBoodjeDiscount=0;
            double sumJavaniAmount = 0;
            IEnumerable<int> tags = await _tagService.GetIdsByBillId(customerInfo.BillId.Trim());

            foreach (var nerkhItem in allNerkh)
            {
                AbAzadFormulaDto abAzadItem = abAzad.ElementAt(counter);
                ZaribGetDto zaribItem = zarib.ElementAt(counter);
                ZaribCQueryDto zaribC = await _zaribCQueryService.GetZaribC(nerkhItem.Date1, nerkhItem.Date2);
                BaseOldTariffEngineOutputDto resultCalc = CalculateWaterBill(nerkhItem, abAzadItem, zaribItem, customerInfo, meterInfo, dailyAverage, currentDateJalali, consumption, duration, zaribC is not null? zaribC.C: null, tags);
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
            //double abonmanFazelbAmount = CalculateAbonmanFazelab(duration, customerInfo, currentDateJalali, sumAbBaha);//todo:bonmanab
            sumAbonmanAbBaha = CalculateAbonmanAb(customerInfo, meterInfo, currentDateJalali);
            double abonmanFazelbAmount = CalculateAbonmanFazelab(duration, customerInfo, currentDateJalali, sumAbonmanAbBaha);
            double AbBahaResult = sumAbBaha + sumHotSeasonAbBaha + sumAbonmanAbBaha;
            double sumBoodje = sumBoodjePart1 + sumBoodjePart2;
            double sumMaliatAmount = CalcMaliat(sumAbBaha, sumAbonmanAbBaha, sumHotSeasonAbBaha, sumFazelab, abonmanFazelbAmount, sumHotSeasonFazelab, sumBoodje);

            stopWatch.Stop();
            return new AbBahaCalculationDetails(AbBahaResult, sumAbBaha, sumFazelab, sumBoodjePart1, sumBoodjePart2, sumBoodje, sumHotSeasonAbBaha, sumHotSeasonFazelab, sumAbBahaDiscount, sumHotSeasonDiscount, sumFazelabDiscount, sumAbonmanAbBaha, sumAvarez, sumJavaniAmount, sumMaliatAmount, abonmanFazelbAmount,
                        sumAbonmanAbDiscount, sumAbonmanFazelabDiscount,sumAvarezDiscount,sumJavaniDiscount,allNerkh, abAzad, zarib, stopWatch.ElapsedMilliseconds);
        }
        private CustomerInfoOutputDto GetCustomerInfo(BaseOldTariffEngineImaginaryInputDto input)
        {
            return new CustomerInfoOutputDto()
            {
                ZoneId = input.customerInfo.ZoneId,
                Radif = input.customerInfo.Radif,
                BranchType = input.customerInfo.BranchType,
                UsageId = input.customerInfo.UsageId,
                DomesticUnit = input.customerInfo.DomesticUnit,
                CommertialUnit = input.customerInfo.CommertialUnit,
                OtherUnit = input.customerInfo.OtherUnit,
                WaterInstallationDateJalali = input.customerInfo.WaterInstallationDateJalali,
                SewageInstallationDateJalali = input.customerInfo.SewageInstallationDateJalali,
                WaterCount = input.customerInfo.WaterCount,
                SewageCalcState = input.customerInfo.SewageCalcState,
                ContractualCapacity = input.customerInfo.ContractualCapacity,
                HouseholdNumber = input.customerInfo.HouseholdNumber,
                ReadingNumber = input.customerInfo.ReadingNumber,
                VillageId = input.customerInfo.VillageId,
                IsSpecial = input.customerInfo.IsSpecial,
                BillId= input.MeterPreviousData.BillId
            };
        }
    }
}