using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
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
        private readonly IRepairQueryService _repairQueryService;
        private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;
        private const int _11Olgoo = 11;
        private const int _12Olgoo = 12;
        private const int _13Olgoo = 13;
        private const int _14Olgoo = 14;
        private int _11And12OlgooConsumptionAverage = 28;
        private int _13And14OlgooConsumptionAverage = 29;
        private int _domesticCAmount = 68022;
        private int _dayOfMonth = 30;
        private int _from4YearsAgo = -4;
        public ReturnBillPartialHandler(
            ISQueryService sQueryService,
            IBedBesQueryService besQueryService,
            IZaribCQueryService zaribCQueryService,
            IOldTariffEngine oldTariffEngine,
            IReturnBillBaseHandler returnBillBaseHandler,
            IRepairQueryService repairQueryService,
            IBillReturnCauseQueryService billReturnCauseQueryService)
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

            _repairQueryService = repairQueryService;
            _repairQueryService.NotNull(nameof(repairQueryService));

            _billReturnCauseQueryService = billReturnCauseQueryService;
            _billReturnCauseQueryService.NotNull(nameof(billReturnCauseQueryService));
        }

        public async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> Handle(ReturnBillPartialInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await Validate(inputDto, appUser, cancellationToken);
            int jalaseNumber = await _returnBillBaseHandler.GetJalaliNumber(inputDto.MinutesNumber, customerInfo.ZoneId, customerInfo.Radif);
            var (bedBesInfo, bedBesResult) = await GetBedBesCreateDto(inputDto, customerInfo);

            float consumptionAverage = 0;
            if (BurstPipe.Contains(inputDto.ReturnCauseId))
            {
                await BurstPipeValidate(bedBesResult, inputDto.ReturnCauseId);

                if (inputDto.UserInput.HasValue && inputDto.UserInput.Value > 0)
                {
                    consumptionAverage = inputDto.UserInput.Value;
                }
                consumptionAverage = await GetConsumptionAverage(customerInfo, bedBesResult.PriDate, bedBesResult.TodayDate, consumptionAverage);
                AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(inputDto, bedBesInfo, consumptionAverage, cancellationToken);
                var (finalAmount, hadarConsumption) = await GetAbHadarMasHadar(bedBesResult, customerInfo, (float)abBahaResult.Consumption, bedBesResult.PriDate, bedBesResult.TodayDate);
                return await CreateAutoBacksAndReturn(abBahaResult, inputDto, bedBesInfo, bedBesResult, customerInfo, hadarConsumption, (long)finalAmount, consumptionAverage, jalaseNumber, appUser, inputDto.FromDateJalali, inputDto.ToDateJalali, cancellationToken);
            }

            else
            {
                if (!MisreadedCalcWithMeterNumber.Contains(inputDto.ReturnCauseId))
                {
                    consumptionAverage = await _returnBillBaseHandler.GetConsumptionAverage(inputDto.FromDateJalali, inputDto.ToDateJalali, inputDto.CalculationType, inputDto.UserInput, customerInfo, inputDto.ReturnCauseId);
                }
            }

            if (Misreaded.Contains(inputDto.ReturnCauseId))
            {
                AbBahaCalculationDetails abBahaResult = new();
                if (MisreadedCalcWithMeterNumber.Contains(inputDto.ReturnCauseId))
                {
                    abBahaResult = await GetAbBahaTariffWithMeterNumber(inputDto, bedBesInfo, bedBesResult, customerInfo, cancellationToken);
                }
                else
                {
                    if (consumptionAverage == 0)
                    {
                        throw new BaseException(ExceptionLiterals.InvalidToCalculateConsumptioAverage);
                    }
                    abBahaResult = await GetAbBahaTariffWithConsumptionAverage(inputDto, bedBesInfo, consumptionAverage, cancellationToken);
                }
                return await CreateAutoBacksAndReturn(abBahaResult, inputDto, bedBesInfo, bedBesResult, customerInfo, null, null, (float)abBahaResult.MonthlyConsumption, jalaseNumber, appUser, inputDto.FromDateJalali, inputDto.ToDateJalali, cancellationToken);
            }
            else
            {
                AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(inputDto, bedBesInfo, consumptionAverage, cancellationToken);
                return await CreateAutoBacksAndReturn(abBahaResult, inputDto, bedBesInfo, bedBesResult, customerInfo, null, null, consumptionAverage, jalaseNumber, appUser, inputDto.FromDateJalali, inputDto.ToDateJalali, cancellationToken);
            }
        }
        private async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> CreateAutoBacksAndReturn(AbBahaCalculationDetails abBahaResult, ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBesInfo, BedBesCreateDto bedBesResult, CustomerInfoOutputDto customerInfo, float? hadarConsumption, long? finalAmount, float consumptionAverage, int jalaseNumber, IAppUser appUser, string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
        {
            AutoBackCreateDto bedBes = _returnBillBaseHandler.GetBedBes(bedBesResult, bedBesInfo.Count(), jalaseNumber, input.ReturnCauseId);
            AutoBackCreateDto newCalculation = _returnBillBaseHandler.GetNewCalculation(abBahaResult, bedBesResult, input.ReturnCauseId, bedBesInfo.Count(), hadarConsumption ?? 0, (long)(finalAmount ?? 0), jalaseNumber);
            AutoBackCreateDto different = _returnBillBaseHandler.GetDifferent(bedBesResult, newCalculation, jalaseNumber);

            return await _returnBillBaseHandler.GetReturn(bedBes, newCalculation, different, customerInfo, bedBesInfo.Count(), input.IsConfirm, true, appUser, fromDateJalali, toDateJalali);
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaTariff(ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBes, double consumptionAverage, CancellationToken cancellationToken)
        {
            string firstDateJalali = bedBes.Min(x => x.PriDate);
            string latestDateJalali = bedBes.Max(x => x.TodayDate);

            MeterDateInfoWithMonthlyConsumptionOutputDto meterData = new(input.BillId, firstDateJalali, latestDateJalali, consumptionAverage);
            AbBahaCalculationDetails abBahaResult = await _oldTariffEngine.Handle(meterData, cancellationToken);
            return abBahaResult;
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaTariffWithConsumptionAverage(ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBes, float consumptionAverage, CancellationToken cancellationToken)
        {
            string previousDateJalali = bedBes.Min(x => x.PriDate);
            string currentDateJalali = bedBes.Max(x => x.TodayDate);

            int previousNumber = (int)bedBes.Min(x => x.PriNo);
            int currentNumber = (int)bedBes.Max(x => x.TodayNo);

            MeterDateInfoWithMonthlyConsumptionOutputDto meterData = new(input.BillId, previousDateJalali, currentDateJalali, consumptionAverage);
            AbBahaCalculationDetails abBahaResult = await _oldTariffEngine.Handle(meterData, cancellationToken);

            return abBahaResult;
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaTariffWithMeterNumber(ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBesList, BedBesCreateDto bedBesResult, CustomerInfoOutputDto customerInfoOutputDto, CancellationToken cancellationToken)
        {
            string previousDateJalali = bedBesList.Min(x => x.PriDate);
            string currentDateJalali = bedBesList.Max(x => x.TodayDate);

            int previousNumber = (int)bedBesList.Min(x => x.PriNo);
            int currentNumber = (int)bedBesList.Max(x => x.TodayNo);

            if (bedBesList.Any(bedBes => bedBes.CodVas == (int)CounterStateCodeEnum.Malfunction))
            {
                decimal masraf = bedBesResult.Masraf;
                float consumptionAverage = (float)masraf * _dayOfMonth / (float)(customerInfoOutputDto.PureDomesticUnit * bedBesResult.Modat);
                MeterDateInfoWithMonthlyConsumptionOutputDto meterData = new(input.BillId, previousDateJalali, currentDateJalali, consumptionAverage);
                AbBahaCalculationDetails abBahaResult = await _oldTariffEngine.Handle(meterData, cancellationToken);
                return abBahaResult;
            }
            else
            {
                MeterInfoByPreviousDataInputDto meterData = new()
                {
                    BillId = input.BillId,
                    PreviousDateJalali = previousDateJalali,
                    CurrentDateJalali = currentDateJalali,
                    PreviousNumber = previousNumber,
                    CurrentMeterNumber = currentNumber,
                };

                AbBahaCalculationDetails abBahaResult = await _oldTariffEngine.Handle(meterData, cancellationToken);
                return abBahaResult;
            }
        }
        private async Task<(IEnumerable<BedBesCreateDto>, BedBesCreateDto)> GetBedBesCreateDto(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo)
        {
            IEnumerable<BedBesCreateDto> bedBesInfo = await _returnBillBaseHandler.GetBedBesList(customerInfo, input.FromDateJalali, input.ToDateJalali);
            BedBesCreateDto bedBesResult = _returnBillBaseHandler.GetBedbes(bedBesInfo, customerInfo);

            return (bedBesInfo, bedBesResult);
        }
        private async Task<(float, float)> GetAbHadarMasHadar(BedBesCreateDto bedBes, CustomerInfoOutputDto customerInfo, float consumption, string priDateLatestBill, string todayDateLatestBill)
        {
            var (olgo, c) = await GetOlgoAndC(priDateLatestBill, todayDateLatestBill, customerInfo.ZoneId);

            int customerConsumption = (int)bedBes.Masraf;
            int amount = _returnBillBaseHandler.IsDomestic(customerInfo.UsageId) ? _domesticCAmount : c;
            float masHadr = customerConsumption - consumption;
            float finalAmount = amount * masHadr;

            return (finalAmount, masHadr);
        }
        private async Task<float> GetConsumptionAverage(CustomerInfoOutputDto customerInfo, string priDateLatestBill, string todayDateLatestBill, float consumptionAverage)
        {
            var (olgo, c) = await GetOlgoAndC(priDateLatestBill, todayDateLatestBill, customerInfo.ZoneId);

            return _returnBillBaseHandler.IsDomestic(customerInfo.UsageId) ?
                  olgo switch
                  {
                      _11Olgoo or _12Olgoo => _11And12OlgooConsumptionAverage,
                      _13Olgoo or _14Olgoo => _13And14OlgooConsumptionAverage,
                      _ => consumptionAverage,
                  } :
                 consumptionAverage;
        }
        private async Task<(int, int)> GetOlgoAndC(string fromDate, string toDate, int zoneId)
        {
            SGetDto s = await _sQueryService.Get(fromDate, toDate, zoneId);
            int olgo = s is null || s.Olgo <= 0 ? _14Olgoo : s.Olgo;

            ZaribCQueryDto zaribC = await _zaribCQueryService.GetLatest(fromDate, toDate);
            int c = zaribC is null || zaribC.C <= 0 ? throw new ReturnedBillException(ExceptionLiterals.CantReturn) : zaribC.C;

            return (olgo, c);
        }
        private async Task<CustomerInfoOutputDto> Validate(ReturnBillPartialInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _returnBillBaseHandler.PartialValidate(input, cancellationToken);
            CustomerInfoOutputDto customerInfo = await _returnBillBaseHandler.Validate(appUser, input.BillId, input.FromDateJalali, input.ToDateJalali);
            if (ValidateDomesticBurstPieConsumptionAverage(input, customerInfo.UsageId))
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidReturnDomesticConsumptionAverage);
            }

            return customerInfo;
        }
        private bool ValidateDomesticBurstPieConsumptionAverage(ReturnBillPartialInputDto input, int usageId)
        {
            return input.ReturnCauseId == (int)ReturnCauseEnum.LooleTarakidegi && !_returnBillBaseHandler.IsDomestic(usageId) && (input.UserInput is null || input.UserInput <= 0);
        }
        private async Task BurstPipeValidate(BedBesCreateDto bedBesResult, int returnCauseId)
        {
            RepairDateValidateDto repairDateValidateDto = new()
            {
                ZoneId = (int)bedBesResult.Town,
                CustomerNumber = (int)bedBesResult.Radif,
                ReturnCauseId = returnCauseId,
                FromDateJalali = DateTime.Now.AddYears(_from4YearsAgo).ToShortPersianDateString(),
                ToDateJalali = bedBesResult.TodayDate,
            };
            BillReturnCauseGetDto returnCauseInfo = await _billReturnCauseQueryService.Get((short)returnCauseId);

            RepairedOutputDto? repairOutput = await _repairQueryService.GetRepairDateValidate(repairDateValidateDto);
            if (repairOutput != null)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidReturn(returnCauseInfo.Title));
            }
        }
        private int[] BurstPipe { get { return [(int)ReturnCauseEnum.LooleTarakidegi]; } }
        private int[] Misreaded
        {
            get
            {
                return [(int)ReturnCauseEnum.EshtebahGheraat,
                        (int)ReturnCauseEnum.EshtebahDarGhotrEnshab,
                        (int)ReturnCauseEnum.EshtebahDarShomareGhabli,
                        (int)ReturnCauseEnum.EshtebahDarMablaghAbBaha,
                        (int)ReturnCauseEnum.EshtebahDarMablaghFazelab,
                        (int)ReturnCauseEnum.EshtebahDarTedadVahed,
                        (int)ReturnCauseEnum.EshtebahDarNoeKarbari];
            }
        }
        private int[] MisreadedCalcWithMeterNumber
        {
            get
            {
                return [(int)ReturnCauseEnum.EshtebahGheraat,
                        (int)ReturnCauseEnum.EshtebahDarShomareGhabli,
                        (int)ReturnCauseEnum.EshtebahDarMasraf,
                        (int)ReturnCauseEnum.EshtebahDarMablaghAbBaha,
                        (int)ReturnCauseEnum.EshtebahDarMablaghFazelab,
                        (int)ReturnCauseEnum.EshtebahDarTedadVahed  ,
                        (int)ReturnCauseEnum.EshtebahDarNoeKarbari];
            }
        }
    }
}