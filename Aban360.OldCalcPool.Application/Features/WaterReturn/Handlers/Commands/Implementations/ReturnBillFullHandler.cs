using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillFullHandler : IReturnBillFullHandler
    {
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IBedBesCommandService _bedBesCommandService;
        private readonly IRepairCommandService _repairCommandService;
        private readonly IAutoBackCommandService _autoBackCommandService;
        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly IValidator<ReturnBillFullInputDto> _validator;
        public ReturnBillFullHandler(
            IBedBesQueryService bedBesQueryService,
            IBedBesCommandService bedBesCommandService,
            IRepairCommandService repairCommandService,
            IAutoBackCommandService autoBackCommandService,
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IValidator<ReturnBillFullInputDto> validator)
        {
            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _bedBesCommandService = bedBesCommandService;
            _bedBesCommandService.NotNull(nameof(bedBesCommandService));

            _repairCommandService = repairCommandService;
            _repairCommandService.NotNull(nameof(repairCommandService));

            _autoBackCommandService = autoBackCommandService;
            _autoBackCommandService.NotNull(nameof(autoBackCommandService));

            _customerInfoDetailQueryService = customerInfoDetailQueryService;
            _customerInfoDetailQueryService.NotNull(nameof(customerInfoDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReturnBillOutputDto> Handle(ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            await ValidationFullInputDto(input, cancellationToken);
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            await FromToDateValidation(input, customerInfo);

            IEnumerable<BedBesCreateDto> bedBesInfo = await GetBedBesList(customerInfo, input);
            BedBesCreateDto bedBesResult = GetBedbes(bedBesInfo);

            await UpdateBedBesDel(bedBesInfo);
            RepairCreateDto repairCreate = GetRepairCreateDto(bedBesInfo, customerInfo, input);
            AutoBackCreateDto autoBackCreate = GetAutoBackCreateDto(bedBesInfo, repairCreate);
            if (!input.IsConfirm)
            {
                return GetReturn(bedBesResult, repairCreate, autoBackCreate);
            }

            await _repairCommandService.Create(repairCreate);//todo : remove comment
            await _autoBackCommandService.Create(autoBackCreate);

            return GetReturn(bedBesResult, repairCreate, autoBackCreate);
        }
        private async Task<IEnumerable<BedBesCreateDto>> GetBedBesList(CustomerInfoOutputDto customerInfo, ReturnBillFullInputDto input)
        {
            ZoneCustomerFromToDateDto bedBesGetDto = new(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali, input.ToDateJalali);
            IEnumerable<BedBesCreateDto> bedBesInfo = await _bedBesQueryService.Get(bedBesGetDto);

            //return bedBesInfo.Any() ? bedBesInfo : throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToRemoved);

            if (!bedBesInfo.Any())
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToRemoved);

            if (bedBesInfo.Any(x => x.Del))
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidBillWithDel);
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
        private RepairCreateDto GetRepairCreateDto(IEnumerable<BedBesCreateDto> bedBes, CustomerInfoOutputDto customerInfo, ReturnBillFullInputDto input)
        {
            BedBesCreateDto latestBedbes = bedBes.OrderByDescending(x => x.DateBed).FirstOrDefault();
            BedBesCreateDto finalBedBes = bedBes.Aggregate(new BedBesCreateDto(), (a, b) =>
            {
                a.AbonFas += b.AbonFas;
                a.FasBaha += b.FasBaha;
                a.Ztadil += b.Ztadil;
                a.Masraf += b.Masraf;
                a.Shahrdari += b.Shahrdari;
                a.Baha += b.Baha;
                a.AbonAb += b.AbonAb;
                a.Pard += b.Pard;
                a.Jam += b.Jam;
                a.ZaribFasl += b.ZaribFasl;
                a.Jarime += b.Jarime;
                a.AbBaha += b.AbBaha;
                a.Ab10 += b.Ab10;
                a.Ab20 += b.Ab20;
                a.Tafavot += b.Tafavot;
                a.Bodjeh += b.Bodjeh;
                a.Rate += b.Rate;
                a.ZaribD += b.ZaribD;
                a.Avarez += b.Avarez;
                a.CodVas = b.CodVas;

                return a;
            });

            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            decimal previousNumber = bedBes.Min(x => x.PriNo);
            decimal currentNumber = bedBes.Max(x => x.TodayNo);

            string firstDateJalali = bedBes.Min(x => x.PriDate);
            string latestDateJalali = bedBes.Max(x => x.TodayDate);
            int duration = Duration(latestDateJalali, firstDateJalali,(int)finalBedBes.CodVas);

            return new RepairCreateDto()
            {
                Town = customerInfo.ZoneId,
                Radif = customerInfo.Radif,
                Eshtrak = customerInfo.ReadingNumber,
                Barge = 0,
                PriNo = previousNumber,
                TodayNo = currentNumber,
                PriDate = firstDateJalali,
                TodayDate = latestDateJalali,
                AbonFas = finalBedBes.AbonFas,
                FasBaha = finalBedBes.FasBaha,
                AbBaha = finalBedBes.AbBaha,
                Ztadil = finalBedBes.Ztadil,
                Masraf = finalBedBes.Masraf,
                Shahrdari = finalBedBes.Shahrdari,
                Modat = duration,
                DateBed = currentDateJalali,
                JalaseNo = input.Minutes,
                Mohlat = string.Empty,
                Baha = finalBedBes.Baha,
                AbonAb = finalBedBes.AbonAb,
                Pard = (finalBedBes.Pard / 1000) * 1000,
                Jam = finalBedBes.Jam,
                CodVas = 02,
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = customerInfo.UsageId,
                Enshab = customerInfo.MeterDiameterId,
                Elat = input.ReturnCauseId,
                Serial = latestBedbes.Serial,
                Ser = 0,
                ZaribFasl = 0,//
                Ab10 = 0,
                Ab20 = 0,
                TedadMas = customerInfo.DomesticUnit,
                TedadTej = customerInfo.CommertialUnit,
                TedadVahd = customerInfo.OtherUnit,
                NoeVa = customerInfo.BranchType,
                Jarime = 0,//
                Masjar = 0,
                Sabt = 0,
                Rate = finalBedBes.Rate,
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
                Bodjeh = finalBedBes.Bodjeh,
                Group1 = customerInfo.UsageId,
                Faz = customerInfo.SewageCalcState > 0 ? true : false,
                ChkKarbari = 0,//
                C200 = 0,//
                TmpPriDate = string.Empty,
                TmpTodayDate = string.Empty,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = string.Empty,
                EdarehK = customerInfo.IsSpecial,
                Lavazem = 0,//
                DateSbt = currentDateJalali,
                Avarez = finalBedBes.Avarez,
                TedKhane = customerInfo.HouseholdNumber
            };
        }
        private AutoBackCreateDto GetAutoBackCreateDto(IEnumerable<BedBesCreateDto> bedBes, RepairCreateDto repair)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string currentDateJalali10Char = currentDateJalali.Substring(2);

            return new AutoBackCreateDto()
            {
                Town = repair.Town,
                Radif = repair.Radif,
                Eshtrak = repair.Eshtrak,
                Barge = 0,
                PriNo = repair.PriNo,
                TodayNo = repair.TodayNo,
                PriDate = repair.PriDate,
                TodayDate = repair.TodayDate,
                AbonFas = 0,
                FasBaha = 0,
                AbBaha = 0,
                Ztadil = 0,
                Masraf = 0,
                Shahrdari = 0,
                Modat = 0,
                DateBed = currentDateJalali,
                JalaseNo = repair.JalaseNo,
                Mohlat = string.Empty,
                Baha = 0,
                AbonAb = 0,
                Pard = 0,
                Jam = 0,
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
                Jarime = 0,
                Masjar = 0,
                Sabt = 0,
                Rate = 0,
                Operator = 0,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,
                Tafavot = 0,
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,
                TafBack = 0,
                TedGhabs = 0,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = 0,
                Faz = false,
                TmpPriDate = string.Empty,
                TmpTodayDate = currentDateJalali10Char,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = currentDateJalali10Char,
            };
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
        private async Task FromToDateValidation(ReturnBillFullInputDto input, CustomerInfoOutputDto customerInfo)
        {
            async Task Validation(string date, string exceptionMessage)
            {
                int count = await _bedBesQueryService.GetCountInDateBed(customerInfo.ZoneId, customerInfo.Radif, date);
                _ = count <= 0 ? throw new ReturnedBillException(exceptionMessage) : 0;
            }

            await Validation(input.FromDateJalali, ExceptionLiterals.InvalidFromDate);
            await Validation(input.ToDateJalali, ExceptionLiterals.InvalidToDate);
        }
        private async Task ValidationFullInputDto(ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
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
        private ReturnBillOutputDto GetReturn(BedBesCreateDto bedBes, RepairCreateDto repair, AutoBackCreateDto autoBack)
        {
            BedBesOutputDto bedBesResult = new BedBesOutputDto()
            {
                Id = bedBes.Id,
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
                Removable = bedBes.Del,
                UsageId = bedBes.CodEnshab,
                MeterDiameterId = bedBes.Enshab,
                Cause = bedBes.Elat,
                BodySerial = bedBes.Serial,
                Item11 = bedBes.ZaribFasl,
                OtherUnit = bedBes.TedadVahd,
                CommertialUnit = bedBes.TedadTej,
                DomesticUnit = bedBes.TedadMas,
                HouseholdNumber = bedBes.TedKhane,
                BranchType = bedBes.NoeVa,
                Item8 = bedBes.Jarime,
                ConsumptionAverage = bedBes.Rate,
                Operator = bedBes.Operator,
                LastMeterChangeDateJalali = bedBes.TavizDate,
                Item9 = bedBes.Zabresani,
                Item10 = bedBes.ZaribD,
                Discount = bedBes.KasrHa,
                ContractualCapacity = bedBes.FixMas,
                BillId = bedBes.ShGhabs1,
                PayId = bedBes.ShPard1,
                Item18 = bedBes.Bodjeh,
                UsageConsumption = bedBes.Group1,
                HasSewage = bedBes.Faz,
                EmptyUnit = bedBes.KhaliS,
                IsSpecial = bedBes.EdarehK,
                TrackNumber = bedBes.TrackNumber,
            };
            RepairOutputDto repairResult = new RepairOutputDto()
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
            AutoBackOutputDto autoBackResult = new AutoBackOutputDto()
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
                HasSewage = autoBack.Faz,
                HouseholdNumber = autoBack.TedKhane,

            };

            return new ReturnBillOutputDto(bedBesResult, repairResult, autoBackResult);
        }
        private bool IsReverse(int counterStateCode)
        {
            int[] reverse = [3];
            return reverse.Contains(counterStateCode);
        }
    }
}