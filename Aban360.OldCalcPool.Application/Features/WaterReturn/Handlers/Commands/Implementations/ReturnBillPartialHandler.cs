using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillPartialHandler : IReturnBillPartialHandler
    {
        private readonly ISQueryService _sQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IReturnBillBaseHandler _returnBillBaseHandler;

        public ReturnBillPartialHandler(
            ISQueryService sQueryService,
            IBedBesQueryService besQueryService,
            IZaribCQueryService zaribCQueryService,
            IOldTariffEngine oldTariffEngine,
            IReturnBillBaseHandler returnBillBaseHandler)
        {
            _sQueryService = sQueryService;
            _sQueryService.NotNull(nameof(sQueryService));

            _bedBesQueryService = besQueryService;
            _bedBesQueryService.NotNull(nameof(besQueryService));

            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(zaribCQueryService));

            _oldTariffEngine = oldTariffEngine;
            _oldTariffEngine.NotNull(nameof(oldTariffEngine));

            _returnBillBaseHandler = returnBillBaseHandler;
            _returnBillBaseHandler.NotNull(nameof(returnBillBaseHandler));
        }

        public async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> Handle(ReturnBillPartialInputDto inputDto, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await Validation(inputDto, cancellationToken);
            int jalaseNumber = await _returnBillBaseHandler.GetJalaliNumber(inputDto.MinutesNumber, customerInfo.ZoneId, customerInfo.Radif);
            float consumptionAverage = await _returnBillBaseHandler.GetConsumptionAverage(inputDto.FromDateJalali, inputDto.CalculationType, inputDto.UserInput, customerInfo, inputDto.ReturnCauseId);
            var (bedBesInfo, bedBesResult) = await GetBedBesCreateDto(inputDto, customerInfo);

            int[] burstPipe = { 1 };
            int[] misreaded = { 5, 7, 9, 14, 15 };
            if (burstPipe.Contains(inputDto.ReturnCauseId))
            {
                var (finalAmount, hadarConsumption, _consumptionAverage) = await GetAbHadarMasHadar(bedBesResult, customerInfo, consumptionAverage, bedBesResult.PriDate, bedBesResult.TodayDate);
                AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(inputDto, bedBesInfo, _consumptionAverage, cancellationToken);
                return await CreateAutoBacksAndReturn(abBahaResult, inputDto, bedBesInfo, bedBesResult, customerInfo, hadarConsumption, (long)finalAmount, _consumptionAverage, jalaseNumber, cancellationToken);
            }
            if (misreaded.Contains(inputDto.ReturnCauseId))
            {
                AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(inputDto, bedBesInfo, cancellationToken);
                return await CreateAutoBacksAndReturn(abBahaResult, inputDto, bedBesInfo, bedBesResult, customerInfo, null, null, (float)abBahaResult.MonthlyConsumption, jalaseNumber, cancellationToken);
            }
            else
            {
                AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(inputDto, bedBesInfo, consumptionAverage, cancellationToken);
                return await CreateAutoBacksAndReturn(abBahaResult, inputDto, bedBesInfo, bedBesResult, customerInfo, null, null, consumptionAverage, jalaseNumber, cancellationToken);
            }
        }
        private async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> CreateAutoBacksAndReturn(AbBahaCalculationDetails abBahaResult, ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBesInfo, BedBesCreateDto bedBesResult, CustomerInfoOutputDto customerInfo, float? hadarConsumption, long? finalAmount, float consumptionAverage, int jalaseNumber, CancellationToken cancellationToken)
        {
            AutoBackCreateDto bedBes = _returnBillBaseHandler.GetBedBes(bedBesResult, bedBesInfo.Count(), jalaseNumber, input.ReturnCauseId);
            AutoBackCreateDto newCalculation = _returnBillBaseHandler.GetNewCalculation(abBahaResult, bedBesResult, input.ReturnCauseId, bedBesInfo.Count(), hadarConsumption ?? 0, (long)(finalAmount ?? 0), jalaseNumber);
            AutoBackCreateDto different = _returnBillBaseHandler.GetDifferent(bedBesResult, newCalculation, jalaseNumber);

            _returnBillBaseHandler.ValidationAmount(newCalculation.Baha, bedBesInfo.Sum(s => s.Baha));
            return await _returnBillBaseHandler.GetReturn(bedBes, newCalculation, different, customerInfo, bedBesInfo.Count(), input.IsConfirm);
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaTariff(ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBes, double consumptionAverage, CancellationToken cancellationToken)
        {
            string firstDateJalali = bedBes.Min(x => x.PriDate);
            string latestDateJalali = bedBes.Max(x => x.TodayDate);

            MeterDateInfoWithMonthlyConsumptionOutputDto meterData = new(input.BillId, firstDateJalali, latestDateJalali, consumptionAverage);
            AbBahaCalculationDetails abBahaResult = await _oldTariffEngine.Handle(meterData, cancellationToken);
            return abBahaResult;
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaTariff(ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBes, CancellationToken cancellationToken)
        {
            string previousDateJalali = bedBes.Min(x => x.PriDate);
            string currentDateJalali = bedBes.Max(x => x.TodayDate);

            int previousNumber = (int)bedBes.Min(x => x.PriNo);
            int currentNumber = (int)bedBes.Max(x => x.TodayNo);


            MeterInfoByPreviousDataInputDto meterData = new()
            {
                BillId = input.BillId,
                PreviousDateJalali = previousDateJalali,
                CurrentDateJalali = currentDateJalali,
                PreviousNumber = previousNumber,
                CurrentMeterNumber = currentNumber
            };
            AbBahaCalculationDetails abBahaResult = await _oldTariffEngine.Handle(meterData, cancellationToken);
            return abBahaResult;
        }
        private async Task<(IEnumerable<BedBesCreateDto>, BedBesCreateDto)> GetBedBesCreateDto(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo)
        {
            IEnumerable<BedBesCreateDto> bedBesInfo = await _returnBillBaseHandler.GetBedBesList(customerInfo, input.FromDateJalali, input.ToDateJalali);
            BedBesCreateDto bedBesResult = _returnBillBaseHandler.GetBedbes(bedBesInfo, customerInfo);

            return (bedBesInfo, bedBesResult);
        }
        private async Task<(float, float, float)> GetAbHadarMasHadar(BedBesCreateDto bedBes, CustomerInfoOutputDto customerInfo, float consumptionAverage, string priDateLatestBill, string todayDateLatestBill)
        {
            var (olgo, c) = await GetOlgoAndC(priDateLatestBill, todayDateLatestBill, customerInfo.ZoneId);

            float _consumptionAverage = _returnBillBaseHandler.IsDomestic(customerInfo.UsageId) ?
                   olgo switch
                   {
                       11 or 12 => 28,
                       13 or 14 => 29,
                       _ => consumptionAverage,
                   } :
                  consumptionAverage;

            int customerConsumption = Math.Abs((int)(bedBes.TodayNo - bedBes.PriNo));
            int amount = _returnBillBaseHandler.IsDomestic(customerInfo.UsageId) ? 68022 : c;
            int duration = Duration(bedBes.TodayDate, bedBes.PriDate, (int)bedBes.CodVas);
            float consumptionTakhmin = _consumptionAverage / 30 * duration;
            float masHadr = customerConsumption - consumptionTakhmin;
            float finalAmount = amount * masHadr;

            return (finalAmount, masHadr, _consumptionAverage);
        }
        private async Task<(int, int)> GetOlgoAndC(string fromDate, string toDate, int zoneId)
        {
            SGetDto s = await _sQueryService.Get(fromDate, toDate, zoneId);
            int olgo = s is null || s.Olgo <= 0 ? 14 : s.Olgo;

            ZaribCQueryDto zaribC = await _zaribCQueryService.GetLatestZaribC(fromDate, toDate);
            int c = zaribC is null || zaribC.C <= 0 ? throw new ReturnedBillException(ExceptionLiterals.CantReturn) : zaribC.C;

            return (olgo, c);
        }
        private int Duration(string currentDateJalali, string previousDateJalali, int counterStateCode)
        {
            var previousGregorian = previousDateJalali.ToGregorianDateTime();
            var currentGregorian = currentDateJalali.ToGregorianDateTime();
            int duration = 0;

            duration = IsReverse(counterStateCode) ?
                       duration = (previousGregorian.Value - currentGregorian.Value).Days :
                       duration = (currentGregorian.Value - previousGregorian.Value).Days;

            return duration > 0 ? duration : throw new ReturnedBillException(ExceptionLiterals.CurrentDateNotMoreThanPreviousDate);
        }
        private async Task<CustomerInfoOutputDto> Validation(ReturnBillPartialInputDto input, CancellationToken cancellationToken)
        {
            await _returnBillBaseHandler.PartialValidation(input, cancellationToken);
            CustomerInfoOutputDto customerInfo = await _returnBillBaseHandler.Validation(input.BillId, input.FromDateJalali, input.ToDateJalali);

            return customerInfo;
        }
        private bool IsReverse(int counterStateCode)
        {
            int[] reverse = [3];
            return reverse.Contains(counterStateCode);
        }
    }
}
