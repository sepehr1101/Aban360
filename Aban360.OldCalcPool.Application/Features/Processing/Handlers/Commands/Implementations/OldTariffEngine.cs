using Aban360.CalculationPool.Application.Features.Base;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Helpers;
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
        private readonly IConsumptionCalculator _consumptionCalculator;
        private readonly IBillQueryService _billQueryService;

        int thresholdDay = 4;
        float vatRate = 0.1f;

        public OldTariffEngine(
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IMeterInfoDetailQueryService meterInfoDetailQueryService,
            INerkhGetByConsumptionService nerkhGetByConsumptionService,
            IZaribCQueryService zaribCQueryService,
            ITable1GetService table1GetService,
            IBillIdTagService tagService,
            IConsumptionCalculator consumptionCalculator,
            IAbBahaCalculator abBahaCalculator,
            IBillQueryService billQueryService)
            :base(abBahaCalculator)
          )
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

            _consumptionCalculator = consumptionCalculator;
            _consumptionCalculator.NotNull(nameof(_consumptionCalculator));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(_billQueryService));
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
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            MeterInfoOutputDto meterInfo = new MeterInfoOutputDto()
            {
                PreviousDateJalali = input.PreviousDateJalali,
                PreviousNumber = input.PreviousNumber,
                CurrentNumber = input.CurrentMeterNumber,
                CurrentDateJalali = input.CurrentDateJalali
            };
            AbBahaCalculationDetails calculationDetails = await GetCalculationDetails(meterInfo, customerInfo);
            return calculationDetails;
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterImaginaryInputDto input, CancellationToken cancellationToken)
        {
            //TODO: direct create object from MeterComparisonBatchWithAggregatedNerkhGetHandler.cs
            CustomerInfoOutputDto customerInfo = CreateCustomerInfoDto(input);
            try
            {
                MeterInfoOutputDto meterInfo = new()
                {
                    PreviousDateJalali = input.MeterPreviousData.PreviousDateJalali,
                    PreviousNumber = input.MeterPreviousData.PreviousNumber,
                    CurrentDateJalali = input.MeterPreviousData.CurrentDateJalali,
                    CurrentNumber = input.MeterPreviousData.CurrentMeterNumber
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
            Validate(meterInfo.PreviousDateJalali);
            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(nerkhInput);
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, meterInfo.CurrentDateJalali, customerInfo, meterInfo, consumptionInfo);
            return result;
        }

        private async Task<AbBahaCalculationDetails> GetCalculationDetailsWithConsumption(MeterDateInfoWithMonthlyConsumptionOutputDto meterInfoWithConsumption, CustomerInfoOutputDto customerInfo)
        {
            ConsumptionInfo consumptionInfo = _consumptionCalculator.GetConsumptionInfoWithMonthlyConsumption(meterInfoWithConsumption, customerInfo);

            MeterInfoOutputDto meterInfo = new MeterInfoOutputDto(meterInfoWithConsumption.PreviousDateJalali, meterInfoWithConsumption.CurrentDateJalali, 0, 0);
            NerkhByConsumptionInputDto nerkhInput = CreateNerkhInput(meterInfo, customerInfo, consumptionInfo);
            Validate(meterInfo.PreviousDateJalali);
            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>, IEnumerable<ZaribGetDto>, int) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.GetWithAggregatedNerkh(nerkhInput);
            AbBahaCalculationDetails result = await GetAbBahaCalculationDetails(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, meterInfo.CurrentDateJalali, customerInfo, meterInfo, consumptionInfo);
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
        private async Task<AbBahaCalculationDetails> GetAbBahaCalculationDetails(IEnumerable<NerkhGetDto> allNerkh, IEnumerable<AbAzadFormulaDto> abAzad, IEnumerable<ZaribGetDto> zarib, string currentDateJalali, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ConsumptionInfo consumptionInfo)
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
                ConsumptionInfo partialConsumptionInfo = new(nerkhItem.Date1, nerkhItem.Date2, consumptionInfo.Consumption, consumptionInfo.Duration, consumptionInfo.DailyAverageConsumption, consumptionInfo.FinalDomesticUnit);
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
            sumAbonmanFazelab = CalculateFazelab(meterInfo.PreviousDateJalali, currentDateJalali, consumptionInfo.Duration, customerInfo, sumAbonmanAbBaha, currentDateJalali, true);
            sumAbonmanAbDiscount = CalculateAbonmanDiscount(customerInfo.UsageId, sumAbonmanAbBaha, sumAbBahaDiscount, customerInfo.IsSpecial);
            sumAbonmanFazelabDiscount = CalculateAbonmanDiscount(customerInfo.UsageId, sumAbonmanFazelab, sumFazelabDiscount, customerInfo.IsSpecial);
            double maliatDiscount = CalculateTaxDiscount(sumAbBahaDiscount, sumFazelabDiscount, sumAbonmanAbDiscount,
                sumAbonmanFazelabDiscount, sumBoodjeDiscount, sumHotSeasonDiscount);
            double AbBahaResult = sumAbBaha + sumHotSeasonAbBaha + sumAbonmanAbBaha;
            double sumBoodje = sumBoodjePart1 + sumBoodjePart2;
            double sumMaliatAmount = CalculateTax(sumAbBaha, sumAbonmanAbBaha, sumHotSeasonAbBaha, sumFazelab, sumAbonmanFazelab, sumHotSeasonFazelab, sumBoodje);

            stopWatch.Stop();
            return new AbBahaCalculationDetails(Math.Round(AbBahaResult), Math.Round(sumAbBaha), Math.Round(sumFazelab), Math.Round(sumBoodjePart1), Math.Round(sumBoodjePart2), Math.Round(sumBoodje), Math.Round(sumHotSeasonAbBaha), Math.Round(sumHotSeasonFazelab), Math.Round(sumAbBahaDiscount), Math.Round(sumHotSeasonDiscount), Math.Round(sumFazelabDiscount), Math.Round(sumAbonmanAbBaha), Math.Round(sumAvarez), Math.Round(sumJavaniAmount), Math.Round(sumMaliatAmount), Math.Round(sumAbonmanFazelab),
                       Math.Round(sumAbonmanAbDiscount), Math.Round(sumAbonmanFazelabDiscount), Math.Round(sumAvarezDiscount), Math.Round(sumJavaniDiscount), Math.Round(maliatDiscount), Math.Round(sumBoodjeDiscount), allNerkh, abAzad, zarib, consumptionInfo, meterInfo, customerInfo, stopWatch.ElapsedMilliseconds);
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
                VirtualCategoryId = input.CustomerInfo.VirtualCategoryId ?? 0
            };
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