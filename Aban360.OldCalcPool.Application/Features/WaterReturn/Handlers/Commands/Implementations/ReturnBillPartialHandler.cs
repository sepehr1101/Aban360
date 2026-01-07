using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
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

            return inputDto.ReturnCauseId switch
            {
                1 or 4 => await BurstPipe(inputDto, customerInfo, consumptionAverage, cancellationToken),
                _ => await OtherCause(inputDto, customerInfo, consumptionAverage, cancellationToken)
            };
        }
        public async Task<ReturnBillOutputDto> OtherCause(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo, float consumptionAverage, CancellationToken cancellationToken)
        {
            IEnumerable<BedBesCreateDto> bedBesInfo = await GetBedBesList(customerInfo, input);
            BedBesCreateDto bedBesResult = GetBedbes(bedBesInfo);

            AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(input, bedBesInfo, consumptionAverage, cancellationToken);

            await UpdateBedBesDel(bedBesInfo);

            RepairCreateDto repairCreate = GetRepairCreateDto(abBahaResult, input, bedBesInfo, null, null);
            AutoBackCreateDto autoBackCreate = GetAutoBackCreateDto(bedBesResult, repairCreate);

            ValidationAmount(repairCreate.Baha, bedBesInfo.Sum(s => s.Baha));
            if (!input.IsConfirm)
            {
                return GetReturn(bedBesResult, repairCreate, autoBackCreate, bedBesInfo.Count());
            }

            await _repairCommandService.Create(repairCreate);//todo : remove comment
            await _autoBackCommandService.Create(autoBackCreate);

            return GetReturn(bedBesResult, repairCreate, autoBackCreate, bedBesInfo.Count());
        }
        private async Task<ReturnBillOutputDto> BurstPipe(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo, float consumptionAverage, CancellationToken cancellationToken)
        {
            IEnumerable<BedBesCreateDto> bedBesInfo = await GetBedBesList(customerInfo, input);
            BedBesCreateDto latestBill = bedBesInfo.OrderByDescending(r => r.TodayDate).FirstOrDefault();
            BedBesCreateDto bedBesResult = GetBedbes(bedBesInfo);

            var (finalAmount, hadarConsumption, _consumptionAverage) = await GetAbHadarMasHadar(bedBesResult, customerInfo, consumptionAverage, latestBill.PriDate, latestBill.TodayDate);
            AbBahaCalculationDetails abBahaResult = await GetAbBahaTariff(input, bedBesInfo, _consumptionAverage, cancellationToken);

            await UpdateBedBesDel(bedBesInfo);

            RepairCreateDto repairCreate = GetRepairCreateDto(abBahaResult, input, bedBesInfo, hadarConsumption, (long)finalAmount);
            AutoBackCreateDto autoBackCreate = GetAutoBackCreateDto(bedBesResult, repairCreate);

            ValidationAmount(repairCreate.Baha, bedBesInfo.Sum(s => s.Baha));
            if (!input.IsConfirm)
            {
                return GetReturn(bedBesResult, repairCreate, autoBackCreate, bedBesInfo.Count());
            }

            await _repairCommandService.Create(repairCreate);
            await _autoBackCommandService.Create(autoBackCreate);

            return GetReturn(bedBesResult, repairCreate, autoBackCreate, bedBesInfo.Count());
        }
        private ReturnBillOutputDto GetReturn(BedBesCreateDto bedBes, RepairCreateDto repair, AutoBackCreateDto autoBack, int billCount)
        {
            ReturBillDataOutputDto bedBesResult = new ReturBillDataOutputDto()
            {
                ZoneId = bedBes.Town,
                CustomerNumber = bedBes.Radif,
                ReadingNumber = bedBes.Eshtrak,
                PreviousNumber = bedBes.PriNo,
                CurrentNumber = bedBes.TodayNo,
                PreviousDateJalali = bedBes.PriDate,
                CurrentDateJalali = bedBes.TodayDate,
                Item4 = bedBes.AbonFas,
                Item2 = bedBes.FasBaha,
                Item1 = bedBes.AbBaha,
                Item12 = bedBes.Ztadil,
                Consumption = bedBes.Masraf,
                Item5 = bedBes.Shahrdari,
                Duration = bedBes.Modat,
                RegisterDateJalali = bedBes.DateBed,
                Minutes = bedBes.JalaseNo,
                SumItems = bedBes.Baha,
                Item3 = bedBes.AbonAb,
                PayableAmount = bedBes.Pard,
                CounterStateCode = bedBes.CodVas,
                BillsCount = bedBes.Ghabs,
                Removable = bedBes.Del,
                UsageId = bedBes.CodEnshab,
                MeterDiameterId = bedBes.Enshab,
                Cause = bedBes.Elat,
                BodySerial = bedBes.Serial,
                Item11 = bedBes.ZaribFasl,
                OtherUnit = bedBes.TedadVahd,
                DomesticUnit = bedBes.TedadMas,
                CommertialUnit = bedBes.TedadTej,
                BranchType = bedBes.NoeVa,
                Item8 = bedBes.Jarime,
                ConsumptionAverage = bedBes.Rate,
                Operator = bedBes.Operator,
                LastMeterChangeDateJalali = bedBes.TavizDate,
                Item9 = bedBes.Zabresani,
                Item10 = bedBes.ZaribD,
                Difference = bedBes.Tafavot,
                WastedWater = 0,
                WastedConsumption = 0,
                BillCount = billCount,
                Item18 = bedBes.Bodjeh,
                UsageConsumption = bedBes.Group1,
                HasSewage = bedBes.Faz,
                IsSpecial = bedBes.EdarehK,
                Lavazem = 0
            };
            ReturBillDataOutputDto repairResult = new ReturBillDataOutputDto()
            {
                ZoneId = repair.Town,
                CustomerNumber = repair.Radif,
                ReadingNumber = repair.Eshtrak,
                PreviousNumber = repair.PriNo,
                CurrentNumber = repair.TodayNo,
                PreviousDateJalali = repair.PriDate,
                CurrentDateJalali = repair.TodayDate,
                Item4 = repair.AbonFas,
                Item2 = repair.FasBaha,
                Item1 = repair.AbBaha,
                Item12 = repair.Ztadil,
                Consumption = repair.Masraf,
                Item5 = repair.Shahrdari,
                Duration = repair.Modat,
                RegisterDateJalali = repair.DateBed,
                Minutes = repair.JalaseNo,
                SumItems = repair.Baha,
                Item3 = repair.AbonAb,
                PayableAmount = repair.Pard,
                CounterStateCode = repair.CodVas,
                BillsCount = repair.Ghabs,
                Removable = repair.Del,
                UsageId = repair.CodEnshab,
                MeterDiameterId = repair.Enshab,
                Cause = repair.Elat,
                BodySerial = repair.Serial,
                Item11 = repair.ZaribFasl,
                OtherUnit = repair.TedadVahd,
                DomesticUnit = repair.TedadMas,
                CommertialUnit = repair.TedadTej,
                BranchType = repair.NoeVa,
                Item8 = repair.Jarime,
                ConsumptionAverage = repair.Rate,
                Operator = repair.Operator,
                LastMeterChangeDateJalali = repair.TavizDate,
                Item9 = repair.Zabresani,
                Item10 = repair.ZaribD,
                Difference = repair.Tafavot,
                WastedWater = repair.AbHadar,
                WastedConsumption = repair.MasHadar,
                BillCount = repair.TedGhabs,
                Item18 = repair.Bodjeh,
                UsageConsumption = repair.Group1,
                HasSewage = repair.Faz,
                IsSpecial = repair.EdarehK,
                Lavazem = repair.Lavazem
            };
            ReturBillDataOutputDto autoBackResult = new ReturBillDataOutputDto()
            {
                ZoneId = autoBack.Town,
                CustomerNumber = autoBack.Radif,
                ReadingNumber = autoBack.Eshtrak,
                PreviousNumber = autoBack.PriNo,
                CurrentNumber = autoBack.TodayNo,
                PreviousDateJalali = autoBack.PriDate,
                CurrentDateJalali = autoBack.TodayDate,
                Item4 = autoBack.AbonFas,
                Item2 = autoBack.FasBaha,
                Item1 = autoBack.AbBaha,
                Item12 = autoBack.Ztadil,
                Consumption = autoBack.Masraf,
                Item5 = autoBack.Shahrdari,
                Duration = autoBack.Modat,
                RegisterDateJalali = autoBack.DateBed,
                Minutes = autoBack.JalaseNo,
                SumItems = autoBack.Baha,
                Item3 = autoBack.AbonAb,
                PayableAmount = autoBack.Pard,
                CounterStateCode = autoBack.CodVas,
                BillsCount = autoBack.Ghabs,
                Removable = autoBack.Del,
                UsageId = autoBack.CodEnshab,
                MeterDiameterId = autoBack.Enshab,
                Cause = autoBack.Elat,
                BodySerial = autoBack.Serial,
                Item11 = autoBack.ZaribFasl,
                OtherUnit = autoBack.TedadVahd,
                DomesticUnit = autoBack.TedadMas,
                CommertialUnit = autoBack.TedadTej,
                BranchType = autoBack.NoeVa,
                Item8 = autoBack.Jarime,
                ConsumptionAverage = autoBack.Rate,
                Operator = autoBack.Operator,
                LastMeterChangeDateJalali = autoBack.TavizDate,
                Item9 = autoBack.Zabresani,
                Item10 = autoBack.ZaribD,
                Difference = autoBack.Tafavot,
                WastedWater = autoBack.AbHadar,
                WastedConsumption = autoBack.MasHadar,
                BillCount = autoBack.TedGhabs,
                Item18 = autoBack.Bodjeh,
                UsageConsumption = bedBes.Group1,
                HasSewage = autoBack.Faz,
                IsSpecial = bedBes.EdarehK,
                Lavazem = 0
            };

            return new ReturnBillOutputDto(bedBesResult, repairResult, autoBackResult);
        }

        private RepairCreateDto GetRepairCreateDto(AbBahaCalculationDetails tariffInfo, ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBes, float? consumptionHadar, long? abHadarAmount)
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
                MasHadar = (decimal)(consumptionHadar ?? 0),//
                AbHadar = abHadarAmount ?? 0,
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
        private AutoBackCreateDto GetAutoBackCreateDto(BedBesCreateDto bedBes, RepairCreateDto repair)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string currentDateJalali10Char = currentDateJalali.Substring(2);

            //BedBesCreateDto finalBedBes = bedBes.Aggregate(new BedBesCreateDto(), (a, b) =>
            //{
            //    a.AbonFas += b.AbonFas;
            //    a.FasBaha += b.FasBaha;
            //    a.Ztadil += b.Ztadil;
            //    a.Masraf += b.Masraf;
            //    a.Shahrdari += b.Shahrdari;
            //    a.Baha += b.Baha;
            //    a.AbonAb += b.AbonAb;
            //    a.Pard += b.Pard;
            //    a.Jam += b.Jam;
            //    a.ZaribFasl += b.ZaribFasl;
            //    a.Jarime += b.Jarime;
            //    a.AbBaha += b.AbBaha;
            //    a.Ab10 += b.Ab10;
            //    a.Ab20 += b.Ab20;
            //    a.Tafavot += b.Tafavot;
            //    a.Bodjeh += b.Bodjeh;
            //    a.Rate += b.Rate;
            //    a.ZaribD += b.ZaribD;

            //    return a;
            //});

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
                AbonFas = Diff(bedBes.AbonFas, repair.AbonFas),
                FasBaha = Diff(bedBes.FasBaha, repair.FasBaha),
                AbBaha = Diff(bedBes.AbBaha, repair.AbBaha),
                Ztadil = Diff(bedBes.Ztadil, repair.Ztadil),
                Masraf = Diff(bedBes.Masraf, repair.Masraf),
                Shahrdari = Diff(bedBes.Shahrdari, repair.Shahrdari),
                Modat = 0,
                DateBed = currentDateJalali,
                JalaseNo = repair.JalaseNo,
                Mohlat = string.Empty,
                Baha = Diff(bedBes.Baha, repair.Baha),
                AbonAb = Diff(bedBes.AbonAb, repair.AbonAb),
                Pard = 0,
                Jam = Diff(bedBes.Jam, repair.Jam),
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
                Jarime = Diff(bedBes.Jarime, repair.Jarime),
                Masjar = 0,
                Sabt = 0,
                Rate = Diff(bedBes.Rate, repair.Rate),
                Operator = 0,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = Diff(bedBes.ZaribD, repair.ZaribD),
                Tafavot = 0,
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,
                TafBack = 0,
                TedGhabs = 0,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = Diff(bedBes.Bodjeh, repair.Bodjeh),
                Faz = false,
                TmpPriDate = string.Empty,
                TmpTodayDate = currentDateJalali10Char,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = currentDateJalali10Char,
            };
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaTariff(ReturnBillPartialInputDto input, IEnumerable<BedBesCreateDto> bedBes, double consumptionAverage, CancellationToken cancellationToken)
        {
            string firstDateJalali = bedBes.Min(x => x.PriDate);
            string latestDateJalali = bedBes.Max(x => x.TodayDate);

            MeterDateInfoWithMonthlyConsumptionOutputDto meterData = new(input.BillId, firstDateJalali, latestDateJalali, consumptionAverage);
            AbBahaCalculationDetails abBahaResult = await _oldTariffEngine.Handle(meterData, cancellationToken);
            return abBahaResult;
        }
        private async Task<IEnumerable<BedBesCreateDto>> GetBedBesList(CustomerInfoOutputDto customerInfo, ReturnBillPartialInputDto input)
        {
            ZoneCustomerFromToDateDto bedBesGetDto = new(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali, input.ToDateJalali);
            IEnumerable<BedBesCreateDto> bedBesInfo = await _bedBesQueryService.Get(bedBesGetDto);

            return bedBesInfo.Any() ? bedBesInfo : throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToRemoved);
        }
        private async Task UpdateBedBesDel(IEnumerable<BedBesCreateDto> bedBes)
        {
            IEnumerable<BedBesUpdateDelDto> bedBesUpdate = bedBes
                    .Select(s => new BedBesUpdateDelDto((int)s.Town, s.Id, true))
                    .ToList();

            await _bedBesCommandService.UpdateDel(bedBesUpdate);
        }
        private async Task<(float, float, float)> GetAbHadarMasHadar(BedBesCreateDto bedBes, CustomerInfoOutputDto customerInfo, float consumptionAverage, string priDateLatestBill, string todayDateLatestBill)
        {
            var (olgo, c) = await GetOlgoAndC(priDateLatestBill, todayDateLatestBill, customerInfo.ZoneId);

            float _consumptionAverage = IsDomestic(customerInfo.UsageId) ?
                   olgo switch
                   {
                       11 or 12 => 33,
                       13 or 14 => 34,
                       _ => consumptionAverage,
                   } :
                  consumptionAverage;

            int customerConsumption = Math.Abs((int)(bedBes.TodayNo - bedBes.PriNo));
            int amount = IsDomestic(customerInfo.UsageId) ? 68022 : c;
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

            ZaribCQueryDto zaribC = await _zaribCQueryService.GetZaribCBetweenDate(fromDate, toDate);
            //int c = zaribC is null || zaribC.C <= 0 ? 1 : zaribC.C;
            int c = zaribC is null || zaribC.C <= 0 ? throw new ReturnedBillException(ExceptionLiterals.CantReturn) : zaribC.C;

            //todooo: when c is null how can i do??

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
        private async Task FromToDateValidation(ReturnBillPartialInputDto input, CustomerInfoOutputDto customerInfo)
        {
            async Task Validatoin(string date, string exceptionMessage)
            {
                int count = await _bedBesQueryService.GetCountInDateBed(customerInfo.ZoneId, customerInfo.Radif, date);
                _ = count <= 0 ? throw new ReturnedBillException(exceptionMessage) : 0;
            }

            await Validatoin(input.FromDateJalali, ExceptionLiterals.InvalidFromDate);
            await Validatoin(input.ToDateJalali, ExceptionLiterals.InvalidToDate);
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
            _ = repairSumItems > previousSumItems ? throw new ReturnedBillException(ExceptionLiterals.RepairAmountMoreThanBedBesAmount) : 0;
        }
        private static decimal Diff(decimal firstValue, decimal secondValue) => firstValue - secondValue;
        private static BedBesCreateDto GetBedbes(IEnumerable<BedBesCreateDto> c)
        {
            var r = c.MaxBy(x => x.DateBed);

            r.Id = 0;

            r.Barge = 0;
            r.PriNo = c.Min(x => x.PriNo);
            r.TodayNo = c.Max(x => x.TodayNo);
            r.PriDate = c.Min(x => x.PriDate);
            r.TodayDate = c.Max(x => x.TodayDate);

            r.AbonFas = c.Sum(x => x.AbonFas);
            r.FasBaha = c.Sum(x => x.FasBaha);
            r.AbBaha = c.Sum(x => x.AbBaha);
            r.Ztadil = c.Sum(x => x.Ztadil);
            r.Masraf = c.Sum(x => x.Masraf);
            r.Shahrdari = c.Sum(x => x.Shahrdari);
            r.Modat = c.Sum(x => x.Modat);
            r.JalaseNo = 0;

            r.Baha = c.Sum(x => x.Baha);
            r.AbonAb = c.Sum(x => x.AbonAb);
            r.Pard = c.Sum(x => x.Pard);
            r.Jam = c.Sum(x => x.Jam);

            r.ZaribFasl = c.Sum(x => x.ZaribFasl);
            r.Ab10 = c.Sum(x => x.Ab10);
            r.Ab20 = c.Sum(x => x.Ab20);

            r.Jarime = c.Sum(x => x.Jarime);
            r.Masjar = c.Sum(x => x.Masjar);

            r.Rate = c.Average(x => x.Rate);
            r.ZaribCntr = c.Sum(x => x.ZaribCntr);
            r.Zabresani = c.Sum(x => x.Zabresani);
            r.ZaribD = c.Sum(x => x.ZaribD);
            r.Tafavot = c.Sum(x => x.Tafavot);
            r.KasrHa = c.Sum(x => x.KasrHa);

            r.TabAbnA = c.Sum(x => x.TabAbnA);
            r.TabAbnF = c.Sum(x => x.TabAbnF);
            r.TabsFa = c.Sum(x => x.TabsFa);
            r.NewAb = c.Sum(x => x.NewAb);
            r.NewFa = c.Sum(x => x.NewFa);
            r.Bodjeh = c.Sum(x => x.Bodjeh);

            r.C200 = c.Sum(x => x.C200);
            r.AbSevom = c.Sum(x => x.AbSevom);
            r.AbSevom1 = c.Sum(x => x.AbSevom1);
            r.C70 = c.Sum(x => x.C70);
            r.C80 = c.Sum(x => x.C80);
            r.C90 = c.Sum(x => x.C90);
            r.C101 = c.Sum(x => x.C101);
            r.Tafa402 = c.Sum(x => x.Tafa402);
            r.Avarez = c.Sum(x => x.Avarez);

            r.DateBed = string.Empty;
            r.Mohlat = string.Empty;
            r.TavizDate = string.Empty;
            r.Ghabs = string.Empty;
            r.TmpDateBed = string.Empty;
            r.TmpPriDate = string.Empty;
            r.TmpTodayDate = string.Empty;
            r.TmpMohlat = string.Empty;
            r.TmpTavizDate = string.Empty;

            return r;
        }
        private bool IsReverse(int counterStateCode)
        {
            int[] reverse = [3];
            return reverse.Contains(counterStateCode);
        }
    }
}
