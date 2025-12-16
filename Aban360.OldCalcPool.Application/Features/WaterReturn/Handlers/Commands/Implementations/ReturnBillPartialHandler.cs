using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using System.Threading;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillPartialHandler : IReturnBillPartialHandler
    {
        private readonly IRepairCommandService _repairCommandService;
        private readonly IAutoBackCommandService _autoBackCommandService;
        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly ISQueryService _sQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IBedBesCommandService _bedBesCommandService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IValidator<ReturnBillPartialInputDto> _validator;
        public ReturnBillPartialHandler(
            IRepairCommandService repairCommandService,
            IAutoBackCommandService autoBackCommandService,
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            ISQueryService sQueryService,
            IBedBesQueryService besQueryService,
            IBedBesCommandService bedBesCommandService,
            IZaribCQueryService zaribCQueryService,
            IOldTariffEngine oldTariffEngine,
            IValidator<ReturnBillPartialInputDto> validator)
        {
            _repairCommandService = repairCommandService;
            _repairCommandService.NotNull(nameof(repairCommandService));

            _autoBackCommandService = autoBackCommandService;
            _autoBackCommandService.NotNull(nameof(autoBackCommandService));

            _customerInfoDetailQueryService = customerInfoDetailQueryService;
            _customerInfoDetailQueryService.NotNull(nameof(customerInfoDetailQueryService));

            _sQueryService = sQueryService;
            _sQueryService.NotNull(nameof(sQueryService));

            _bedBesQueryService = besQueryService;
            _bedBesQueryService.NotNull(nameof(besQueryService));

            _bedBesCommandService = bedBesCommandService;
            _bedBesCommandService.NotNull(nameof(bedBesCommandService));

            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(zaribCQueryService));

            _oldTariffEngine = oldTariffEngine;
            _oldTariffEngine.NotNull(nameof(oldTariffEngine));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReturnBillOutputDto> Handle(ReturnBillPartialInputDto inputDto, CancellationToken cancellationToken)
        {
            await ValidationPartialInputDto(inputDto, cancellationToken);
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(inputDto.BillId);
            await FromToDateValidation(inputDto, customerInfo);
            float consumptionAverage = await GetConsumptionAverage(inputDto, customerInfo);

            int[] burstPipe = [1, 4];
            if (burstPipe.Contains(inputDto.ReturnCauseId))
            {
                return await BurstPipe(inputDto, customerInfo, consumptionAverage, cancellationToken);
            }
            else
            {
                return await OtherCause(inputDto, customerInfo, consumptionAverage, cancellationToken);
            }
        }
        public async Task<ReturnBillOutputDto> OtherCause(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo, float consumptionAverage, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(input, consumptionAverage, cancellationToken);
            IEnumerable<BedBesCreateDto> bedBesInfo = await GetBedBesList(customerInfo, input);

            await UpdateBedBesDel(bedBesInfo);
            RepairCreateDto repairCreate = GetRepairCreateDto(abBahaResult, input, bedBesInfo);
            ValidationAmount(repairCreate.Baha, bedBesInfo.Sum(s => s.Baha));
            AutoBackCreateDto autoBackCreate = GetAutoBackCreateDto(bedBesInfo, repairCreate);
            if (!input.IsConfirm)
            {
                return new ReturnBillOutputDto(bedBesInfo, repairCreate, autoBackCreate);

            }

            await _repairCommandService.Create(repairCreate);//todo : remove comment
            await _autoBackCommandService.Create(autoBackCreate);

            return new ReturnBillOutputDto(bedBesInfo, repairCreate, autoBackCreate);
        }
        private async Task<ReturnBillOutputDto> BurstPipe(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo, float consumptionAverage, CancellationToken cancellationToken)
        {
            var (finalAmount, hadarConsumption, _consumptionAverage) = await GetAbHadarMasHadar(input, customerInfo, consumptionAverage);

            AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(input, _consumptionAverage, cancellationToken);
            IEnumerable<BedBesCreateDto> bedBesInfo = await GetBedBesList(customerInfo, input);

            await UpdateBedBesDel(bedBesInfo);
            RepairCreateDto repairCreate = GetRepairCreateDto(abBahaResult, input, customerInfo, hadarConsumption, (long)finalAmount, bedBesInfo);
            ValidationAmount(repairCreate.Baha, bedBesInfo.Sum(s => s.Baha));
            AutoBackCreateDto autoBackCreate = GetAutoBackCreateDto(bedBesInfo, repairCreate);
            if (!input.IsConfirm)
            {
                return new ReturnBillOutputDto(bedBesInfo, repairCreate, autoBackCreate);

            }

            await _repairCommandService.Create(repairCreate);
            await _autoBackCommandService.Create(autoBackCreate);

            return new ReturnBillOutputDto(bedBesInfo, repairCreate, autoBackCreate);
        }


        private RepairCreateDto GetRepairCreateDto(AbBahaCalculationDetails tariffInfo, ReturnBillPartialInputDto input, CustomerInfoOutputDto customeInfo, float consumptionHadar, long abHadarAmount, IEnumerable<BedBesCreateDto> bedBes)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string currentDateJalali10Char = currentDateJalali.Substring(2);
            decimal previousNumber = bedBes.Min(x => x.PriNo);
            decimal currentNumber = bedBes.Max(x => x.TodayNo);

            return new RepairCreateDto()
            {
                Town = tariffInfo.Customer.ZoneId,
                Radif = tariffInfo.Customer.Radif,
                Eshtrak = tariffInfo.Customer.ReadingNumber,
                Barge = 0,
                PriNo = previousNumber,
                TodayNo = currentNumber,
                PriDate = tariffInfo.MeterInfo.PreviousDateJalali,
                TodayDate = tariffInfo.MeterInfo.CurrentDateJalali,
                AbonFas = (decimal)tariffInfo.AbonmanFazelabAmount,
                FasBaha = (decimal)tariffInfo.FazelabAmount + (decimal)tariffInfo.HotSeasonFazelabAmount,
                AbBaha = (decimal)tariffInfo.AbBahaAmount,
                Ztadil = 0,
                Masraf = (decimal)tariffInfo.Consumption,
                Shahrdari = (decimal)tariffInfo.MaliatAmount,
                Modat = (decimal)tariffInfo.Duration,
                DateBed = currentDateJalali,
                JalaseNo = (decimal)input.Minutes,
                Mohlat = string.Empty,
                Baha = (decimal)tariffInfo.SumItems,
                AbonAb = (decimal)tariffInfo.AbonmanAbAmount,
                Pard = ((decimal)tariffInfo.SumItems / 1000) * 1000,
                Jam = (decimal)tariffInfo.SumItems,
                CodVas = (decimal)(tariffInfo.MeterInfo.CounterStateCode ?? 0),
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = tariffInfo.Customer.UsageId,
                Enshab = tariffInfo.Customer.MeterDiameterId,
                Elat = input.ReturnCauseId,
                Serial = bedBes.FirstOrDefault().Serial,
                Ser = 0,
                ZaribFasl = (decimal)tariffInfo.HotSeasonAbBahaAmount,
                Ab10 = 0,
                Ab20 = 0,
                TedadMas = (decimal)tariffInfo.Customer.DomesticUnit,
                TedadTej = (decimal)tariffInfo.Customer.CommertialUnit,
                TedadVahd = (decimal)tariffInfo.Customer.OtherUnit,
                NoeVa = (decimal)tariffInfo.Customer.BranchType,
                Jarime = 0,//
                Masjar = 0,
                Sabt = 0,
                Rate = (decimal)tariffInfo.MonthlyConsumption,
                Operator = 0,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,//
                Tafavot = 0,//
                MasHadar = (decimal)consumptionHadar,//
                AbHadar = abHadarAmount,
                RangeMas = 0,//
                TafBack = 0,
                TedGhabs = bedBes.Count(),
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = (decimal)tariffInfo.SumBoodje,
                Group1 = (decimal)tariffInfo.Customer.UsageId,
                Faz = tariffInfo.Customer.SewageCalcState > 0 ? true : false,
                ChkKarbari = 0,//
                C200 = 0,//
                TmpPriDate = string.Empty,
                TmpTodayDate = string.Empty,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = string.Empty,
                EdarehK = tariffInfo.Customer.IsSpecial,
                Lavazem = 0,//
                DateSbt = currentDateJalali,
                Avarez = (decimal)tariffInfo.AvarezAmount,
                TedKhane = tariffInfo.Customer.HouseholdNumber
            };
        }
        private RepairCreateDto GetRepairCreateDto(AbBahaCalculationDetails tariffInfo, ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBes)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            decimal previousNumber = bedBes.Min(x => x.PriNo);
            decimal currentNumber = bedBes.Max(x => x.TodayNo);

            return new RepairCreateDto()
            {
                Town = tariffInfo.Customer.ZoneId,
                Radif = tariffInfo.Customer.Radif,
                Eshtrak = tariffInfo.Customer.ReadingNumber,
                Barge = 0,
                PriNo = previousNumber,
                TodayNo = currentNumber,
                PriDate = tariffInfo.MeterInfo.PreviousDateJalali,
                TodayDate = tariffInfo.MeterInfo.CurrentDateJalali,
                AbonFas = (decimal)tariffInfo.AbonmanFazelabAmount,
                FasBaha = (decimal)tariffInfo.FazelabAmount + (decimal)tariffInfo.HotSeasonFazelabAmount,
                AbBaha = (decimal)tariffInfo.AbBahaAmount,
                Ztadil = 0,
                Masraf = (decimal)tariffInfo.Consumption,
                Shahrdari = (decimal)tariffInfo.MaliatAmount,
                Modat = (decimal)tariffInfo.Duration,
                DateBed = currentDateJalali,
                JalaseNo = (decimal)input.Minutes,
                Mohlat = string.Empty,
                Baha = (decimal)tariffInfo.SumItems,
                AbonAb = (decimal)tariffInfo.AbonmanAbAmount,
                Pard = ((decimal)tariffInfo.SumItems/1000)*1000,
                Jam = (decimal)tariffInfo.SumItems,
                CodVas = (decimal)(tariffInfo.MeterInfo.CounterStateCode ?? 0),
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = tariffInfo.Customer.UsageId,
                Enshab = tariffInfo.Customer.MeterDiameterId,
                Elat = input.ReturnCauseId,
                Serial = bedBes.FirstOrDefault().Serial,
                Ser = 0,
                ZaribFasl = (decimal)tariffInfo.HotSeasonAbBahaAmount,
                Ab10 = 0,
                Ab20 = 0,
                TedadMas = (decimal)tariffInfo.Customer.DomesticUnit,
                TedadTej = (decimal)tariffInfo.Customer.CommertialUnit,
                TedadVahd = (decimal)tariffInfo.Customer.OtherUnit,
                NoeVa = (decimal)tariffInfo.Customer.BranchType,
                Jarime = 0,//
                Masjar = 0,
                Sabt = 0,
                Rate = (decimal)tariffInfo.MonthlyConsumption,
                Operator = 0,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,//
                Tafavot = 0,//
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,//
                TafBack = 0,
                TedGhabs = bedBes.Count(),
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = (decimal)tariffInfo.SumBoodje,
                Group1 = (decimal)tariffInfo.Customer.UsageId,
                Faz = tariffInfo.Customer.SewageCalcState > 0 ? true : false,
                ChkKarbari = 0,//
                C200 = 0,//
                TmpPriDate = string.Empty,
                TmpTodayDate = string.Empty,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = string.Empty,
                EdarehK = tariffInfo.Customer.IsSpecial,
                Lavazem = 0,//
                DateSbt = currentDateJalali,
                Avarez = (decimal)tariffInfo.AvarezAmount,
                TedKhane = tariffInfo.Customer.HouseholdNumber
            };
        }
        private AutoBackCreateDto GetAutoBackCreateDto(IEnumerable<BedBesCreateDto> bedBes, RepairCreateDto repair)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string currentDateJalali10Char = currentDateJalali.Substring(2);


            BedBesCreateDto finalBedBes = new BedBesCreateDto
            {
                AbonFas = bedBes.Sum(x => x.AbonFas),
                FasBaha = bedBes.Sum(x => x.FasBaha),
                Ztadil = bedBes.Sum(x => x.Ztadil),
                Masraf = bedBes.Sum(x => x.Masraf),
                Shahrdari = bedBes.Sum(x => x.Shahrdari),
                Baha = bedBes.Sum(x => x.Baha),
                AbonAb = bedBes.Sum(x => x.AbonAb),
                Pard = bedBes.Sum(x => x.Pard),
                Jam = bedBes.Sum(x => x.Jam),
                ZaribFasl = bedBes.Sum(x => x.ZaribFasl),
                Jarime = bedBes.Sum(x => x.Jarime),
                AbBaha = bedBes.Sum(x => x.AbBaha),
                Ab10 = bedBes.Sum(x => x.Ab10),
                Ab20 = bedBes.Sum(x => x.Ab20),
                Tafavot = bedBes.Sum(x => x.Tafavot),
                Bodjeh = bedBes.Sum(x => x.Bodjeh),
                Rate = bedBes.Sum(x => x.Rate),
                ZaribD = bedBes.Sum(x => x.ZaribD),
            };

            return new AutoBackCreateDto()
            {
                Town = repair.Town,
                Radif = repair.Radif,
                Eshtrak = repair.Eshtrak,
                Barge = 0,
                PriNo = 0,
                TodayNo = 0,
                PriDate = repair.PriDate,
                TodayDate = repair.TodayDate,
                AbonFas = finalBedBes.AbonFas - repair.AbonFas,
                FasBaha = finalBedBes.FasBaha - repair.FasBaha,
                AbBaha = finalBedBes.AbBaha - repair.AbBaha,
                Ztadil = finalBedBes.Ztadil - repair.Ztadil,
                Masraf = finalBedBes.Masraf - repair.Masraf,
                Shahrdari = finalBedBes.Shahrdari - repair.Shahrdari,
                Modat = 0,
                DateBed = currentDateJalali,
                JalaseNo = repair.JalaseNo,
                Mohlat = string.Empty,
                Baha = finalBedBes.Baha - repair.Baha,
                AbonAb = finalBedBes.AbonAb - repair.AbonAb,
                Pard = 0,
                Jam = finalBedBes.Jam - repair.Jam,
                CodVas = 0,
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = repair.CodEnshab,
                Enshab = repair.Enshab,
                Elat = 0,
                Serial = repair.Serial,
                Ser = 0,
                ZaribFasl = 0,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = repair.TedadVahd,
                TedadMas = repair.TedadMas,
                TedadTej = repair.TedadTej,
                TedKhane = repair.TedKhane,
                NoeVa = repair.NoeVa,
                Jarime = finalBedBes.Jarime - repair.Jarime,
                Masjar = 0,
                Sabt = 0,
                Rate = finalBedBes.Rate - repair.Rate,
                Operator = 0,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = finalBedBes.ZaribD - repair.ZaribD,
                Tafavot = 0,
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,
                TafBack = 0,
                TedGhabs = 0,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = finalBedBes.Bodjeh - repair.Bodjeh,
                Faz = false,
                TmpPriDate = string.Empty,
                TmpTodayDate = currentDateJalali10Char,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = currentDateJalali10Char,
            };
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaTariff(ReturnBillPartialInputDto input, double consumptionAverage, CancellationToken cancellationToken)
        {
            MeterDateInfoWithMonthlyConsumptionOutputDto meterData = new()
            {
                BillId = input.BillId,
                PreviousDateJalali = input.FromDateJalali,
                CurrentDateJalali = input.ToDateJalali,
                MonthlyAverageConsumption = consumptionAverage
            };
            AbBahaCalculationDetails abBahaResult = await _oldTariffEngine.Handle(meterData, cancellationToken);
            return abBahaResult;
        }
        private async Task<IEnumerable<BedBesCreateDto>> GetBedBesList(CustomerInfoOutputDto customerInfo, ReturnBillPartialInputDto input)
        {
            ZoneCustomerFromToDateDto bedBesGetDto = new(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali, input.ToDateJalali);
            IEnumerable<BedBesCreateDto> bedBesInfo = await _bedBesQueryService.Get(bedBesGetDto);
            if (!bedBesInfo.Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToRemoved);
            }

            return bedBesInfo;
        }
        private async Task UpdateBedBesDel(IEnumerable<BedBesCreateDto> bedBes)
        {
            IEnumerable<BedBesUpdateDelDto> bedBesUpdate = bedBes
                    .Select(s => new BedBesUpdateDelDto((int)s.Town, s.Id, true))
                    .ToList();

            await _bedBesCommandService.UpdateDel(bedBesUpdate);
        }
        private async Task<(float, float, float)> GetAbHadarMasHadar(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo, float consumptionAverage)
        {
            var (olgo, c) = await GetOlgoAndC(input, customerInfo.ZoneId);

            float _consumptionAverage = 0;
            if (IsDomestic(customerInfo.UsageId))
            {
                _consumptionAverage = olgo switch
                {
                    11 or 12 => 33,
                    13 or 14 => 34,
                    _ => consumptionAverage,
                };
            }
            else
            {
                _consumptionAverage = consumptionAverage;
            }

            int amount = IsDomestic(customerInfo.UsageId) ? 68022 : c;
            int duration = Duration(input.ToDateJalali, input.FromDateJalali);
            float v = _consumptionAverage / 30 * duration;
            float finalAmount = amount * v;
            //int consumption = (int)Math.Round(v * customerInfo.DomesticUnit);

            return (finalAmount, v, _consumptionAverage);
        }



        private async Task<(int, int)> GetOlgoAndC(ReturnBillPartialInputDto input, int zoneId)
        {
            SGetDto s = await _sQueryService.Get(input.FromDateJalali, input.ToDateJalali, zoneId);
            int olgo = s is null || s.Olgo <= 0 ? 14 : s.Olgo;

            ZaribCQueryDto zaribC = await _zaribCQueryService.GetZaribCBetweenDate(input.FromDateJalali, input.ToDateJalali);
            int c = zaribC is null || zaribC.C <= 0 ? throw new ReturnedBillException(ExceptionLiterals.CantReturn) : zaribC.C;

            return (olgo, c);
        }
        private async Task ValidationPartialInputDto(ReturnBillPartialInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private bool IsDomestic(int customerNumber)
        {
            int[] domesticId = [0, 1, 3];
            return domesticId.Contains(customerNumber);
        }
        private int Duration(string currentDateJalali, string previousDateJalali)
        {
            var previousGregorian = previousDateJalali.ToGregorianDateTime();
            var currentGregorian = currentDateJalali.ToGregorianDateTime();
            int duration = (currentGregorian.Value - previousGregorian.Value).Days;
            if (duration <= 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.CurrentDateNotMoreThanPreviousDate);
            }
            return duration;
        }
        private async Task FromToDateValidation(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo)
        {
            int fromCount = await _bedBesQueryService.GetCountInDateBed(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali);
            if (fromCount <= 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidFromDate);
            }

            int toCount = await _bedBesQueryService.GetCountInDateBed(customerInfo.ZoneId, customerInfo.Radif, input.ToDateJalali);
            if (toCount <= 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidToDate);
            }
        }
        private async Task<float> GetConsumptionAverage(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo)
        {
            float previousConsumptionAverage = await _bedBesQueryService.GetPreviousBill(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali);

            return input.CalculationType switch
            {
                ReturnedBillCalculationTypeEnum.ByPreviousConsumptionAverage => previousConsumptionAverage,
                ReturnedBillCalculationTypeEnum.UserInput => input.UserInput.Value,
                _ => input.UserInput.Value
            };
        }
        private void ValidationAmount(decimal repairSumItems, decimal previousSumItems)
        {
            if (repairSumItems > previousSumItems)
                throw new ReturnedBillException(ExceptionLiterals.RepairAmountMoreThanBedBesAmount);

        }
    }
}
