using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
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
    internal sealed class ReturnBillBaseHandler : IReturnBillBaseHandler
    {
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IAutoBackCommandService _autoBackCommandService;
        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly IValidator<ReturnBillFullInputDto> _returnFullValidator;
        private readonly IValidator<ReturnBillPartialInputDto> _returnPartialValidator;
        public ReturnBillBaseHandler(
            IBedBesQueryService bedBesQueryService,
            IBedBesCommandService bedBesCommandService,
            IRepairCommandService repairCommandService,
            IAutoBackCommandService autoBackCommandService,
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IValidator<ReturnBillFullInputDto> returnFullValidator,
            IValidator<ReturnBillPartialInputDto> returnPartialValidator)
        {
            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _autoBackCommandService = autoBackCommandService;
            _autoBackCommandService.NotNull(nameof(autoBackCommandService));

            _customerInfoDetailQueryService = customerInfoDetailQueryService;
            _customerInfoDetailQueryService.NotNull(nameof(customerInfoDetailQueryService));

            _returnFullValidator = returnFullValidator;
            _returnFullValidator.NotNull(nameof(returnFullValidator));

            _returnPartialValidator = returnPartialValidator;
            _returnPartialValidator.NotNull(nameof(returnPartialValidator));
        }

        public async Task<ReturnBillOutputDto> GetReturn(AutoBackCreateDto bedBes, AutoBackCreateDto newCalculation, AutoBackCreateDto different, int billCount, bool isConfirm)
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
            ReturBillDataOutputDto newCalcResult = new ReturBillDataOutputDto()
            {
                ZoneId = newCalculation.Town,
                CustomerNumber = newCalculation.Radif,
                ReadingNumber = newCalculation.Eshtrak,
                PreviousNumber = newCalculation.PriNo,
                CurrentNumber = newCalculation.TodayNo,
                PreviousDateJalali = newCalculation.PriDate,
                CurrentDateJalali = newCalculation.TodayDate,
                Item4 = newCalculation.AbonFas,
                Item2 = newCalculation.FasBaha,
                Item1 = newCalculation.AbBaha,
                Item12 = newCalculation.Ztadil,
                Consumption = newCalculation.Masraf,
                Item5 = newCalculation.Shahrdari,
                Duration = newCalculation.Modat,
                RegisterDateJalali = newCalculation.DateBed,
                Minutes = newCalculation.JalaseNo,
                SumItems = newCalculation.Baha,
                Item3 = newCalculation.AbonAb,
                PayableAmount = newCalculation.Pard,
                CounterStateCode = newCalculation.CodVas,
                BillsCount = newCalculation.Ghabs,
                Removable = newCalculation.Del,
                UsageId = newCalculation.CodEnshab,
                MeterDiameterId = newCalculation.Enshab,
                Cause = newCalculation.Elat,
                BodySerial = newCalculation.Serial,
                Item11 = newCalculation.ZaribFasl,
                OtherUnit = newCalculation.TedadVahd,
                DomesticUnit = newCalculation.TedadMas,
                CommertialUnit = newCalculation.TedadTej,
                BranchType = newCalculation.NoeVa,
                Item8 = newCalculation.Jarime,
                ConsumptionAverage = newCalculation.Rate,
                Operator = newCalculation.Operator,
                LastMeterChangeDateJalali = newCalculation.TavizDate,
                Item9 = newCalculation.Zabresani,
                Item10 = newCalculation.ZaribD,
                Difference = newCalculation.Tafavot,
                WastedWater = newCalculation.AbHadar,
                WastedConsumption = newCalculation.MasHadar,
                BillCount = newCalculation.TedGhabs,
                Item18 = newCalculation.Bodjeh,
                UsageConsumption = newCalculation.Group1,
                HasSewage = newCalculation.Faz,
                IsSpecial = newCalculation.EdarehK,
                Lavazem = 0
            };
            ReturBillDataOutputDto differentResult = new ReturBillDataOutputDto()
            {
                ZoneId = different.Town,
                CustomerNumber = different.Radif,
                ReadingNumber = different.Eshtrak,
                PreviousNumber = different.PriNo,
                CurrentNumber = different.TodayNo,
                PreviousDateJalali = different.PriDate,
                CurrentDateJalali = different.TodayDate,
                Item4 = different.AbonFas,
                Item2 = different.FasBaha,
                Item1 = different.AbBaha,
                Item12 = different.Ztadil,
                Consumption = different.Masraf,
                Item5 = different.Shahrdari,
                Duration = different.Modat,
                RegisterDateJalali = different.DateBed,
                Minutes = different.JalaseNo,
                SumItems = different.Baha,
                Item3 = different.AbonAb,
                PayableAmount = different.Pard,
                CounterStateCode = different.CodVas,
                BillsCount = different.Ghabs,
                Removable = different.Del,
                UsageId = different.CodEnshab,
                MeterDiameterId = different.Enshab,
                Cause = different.Elat,
                BodySerial = different.Serial,
                Item11 = different.ZaribFasl,
                OtherUnit = different.TedadVahd,
                DomesticUnit = different.TedadMas,
                CommertialUnit = different.TedadTej,
                BranchType = different.NoeVa,
                Item8 = different.Jarime,
                ConsumptionAverage = different.Rate,
                Operator = different.Operator,
                LastMeterChangeDateJalali = different.TavizDate,
                Item9 = different.Zabresani,
                Item10 = different.ZaribD,
                Difference = different.Tafavot,
                WastedWater = different.AbHadar,
                WastedConsumption = different.MasHadar,
                BillCount = different.TedGhabs,
                Item18 = different.Bodjeh,
                UsageConsumption = bedBes.Group1,
                HasSewage = different.Faz,
                IsSpecial = bedBes.EdarehK,
                Lavazem = 0
            };

            ReturnBillOutputDto returnDto = new(bedBesResult, newCalcResult, differentResult);

            if (!isConfirm)
            {
                return returnDto;
            }
            await CreateAutoBack(bedBes, newCalculation, different);
            return returnDto;
        }
        public AutoBackCreateDto GetFullNewCalculation(BedBesCreateDto bedBes, int returnCauseId, int bedbesCount, int jalaseNumber)
        {
            string toDayDateJalali = DateTime.Now.ToShortPersianDateString();

            return new AutoBackCreateDto()
            {
                Town = bedBes.Town,
                Radif = bedBes.Radif,
                Eshtrak = bedBes.Eshtrak,
                Barge = bedBes.Barge,    //
                PriNo = bedBes.PriNo,
                TodayNo = bedBes.TodayNo,
                PriDate = bedBes.PriDate,
                TodayDate = bedBes.TodayDate,
                AbonFas = 0,
                FasBaha = 0,
                AbBaha = 0,
                Ztadil = 0,
                Masraf = 0,
                Shahrdari = 0,
                Modat = bedBes.Modat,
                DateBed = toDayDateJalali,
                JalaseNo = jalaseNumber,
                Mohlat = string.Empty,//
                Baha = 0,
                AbonAb = 0,
                Pard = 0,
                Jam = 0,
                CodVas = bedBes.CodVas,
                Ghabs = string.Empty,//
                Del = false,
                Type = "4",
                CodEnshab = bedBes.CodEnshab,
                Enshab = bedBes.Enshab,
                Elat = returnCauseId,
                Ser = bedBes.Ser,
                Serial = bedBes.Serial,
                ZaribFasl = 0,
                Ab10 = 0,
                Ab20 = 0,
                TedadMas = bedBes.TedadMas,
                TedadTej = bedBes.TedadTej,
                TedadVahd = bedBes.TedadVahd,
                TedGhabs = bedbesCount,
                TedKhane = bedBes.TedKhane,
                NoeVa = bedBes.NoeVa,
                Jarime = 0,
                Masjar = 0,
                Sabt = 0,
                Rate = 0,
                Operator = bedBes.Operator,
                Mamor = bedBes.Mamor,
                TavizDate = bedBes.TavizDate,
                ZaribCntr = 0,
                ZaribD = 0,
                Zabresani = 0,
                Tafavot = 0,
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,
                TafBack = 0,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = 0,
                Faz = bedBes.Faz,//
                TmpDateBed = string.Empty,
                TmpMohlat = string.Empty,
                TmpPriDate = string.Empty,
                TmpTavizDate = string.Empty,
                TmpTodayDate = string.Empty,
                EdarehK = bedBes.EdarehK,
                Group1 = bedBes.Group1,
            };
        }
        public AutoBackCreateDto GetNewCalculation(AbBahaCalculationDetails tariffInfo, BedBesCreateDto bedBes, int returnCauseId, int bedbesCount, float? consumptionHadar, long? abHadarAmount, int jalaseNumber)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string currentDateJalali10Char = currentDateJalali.Substring(2);

            return new AutoBackCreateDto()
            {
                Town = tariffInfo.Customer.ZoneId,
                Radif = tariffInfo.Customer.Radif,
                Eshtrak = tariffInfo.Customer.ReadingNumber,
                Barge = 0,
                PriNo = bedBes.PriNo,
                TodayNo = bedBes.TodayNo,
                PriDate = tariffInfo.MeterInfo.PreviousDateJalali,
                TodayDate = tariffInfo.MeterInfo.CurrentDateJalali,
                AbonFas = (decimal)tariffInfo.AbonmanFazelabAmount,
                FasBaha = (decimal)tariffInfo.FazelabAmount + (decimal)tariffInfo.HotSeasonFazelabAmount,
                AbBaha = (decimal)tariffInfo.AbBahaAmount,
                Ztadil = bedBes.Ztadil,//todo
                Masraf = (decimal)tariffInfo.Consumption,
                Shahrdari = (decimal)tariffInfo.MaliatAmount,
                Modat = tariffInfo.Duration,
                DateBed = currentDateJalali,
                JalaseNo = jalaseNumber,
                Mohlat = string.Empty,
                Baha = (decimal)tariffInfo.SumItems,
                AbonAb = (decimal)tariffInfo.AbonmanAbAmount,
                Pard = (decimal)tariffInfo.SumItems / 1000 * 1000,
                Jam = (decimal)tariffInfo.SumItems,
                CodVas = tariffInfo.MeterInfo.CounterStateCode ?? 0,
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = tariffInfo.Customer.UsageId,
                Enshab = tariffInfo.Customer.MeterDiameterId,
                Elat = returnCauseId,
                Serial = bedBes.Serial,
                Ser = 0,
                ZaribFasl = (decimal)tariffInfo.HotSeasonAbBahaAmount,
                Ab10 = 0,
                Ab20 = 0,
                TedadMas = tariffInfo.Customer.DomesticUnit,
                TedadTej = tariffInfo.Customer.CommertialUnit,
                TedadVahd = tariffInfo.Customer.OtherUnit,
                NoeVa = tariffInfo.Customer.BranchType,
                Jarime = 0,//
                Masjar = 0,
                Sabt = 0,
                Rate = (decimal)tariffInfo.MonthlyConsumption,
                Operator = bedBes.Operator,
                Mamor = bedBes.Mamor,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,//
                Tafavot = 0,//
                MasHadar = (decimal)(consumptionHadar ?? 0),//
                AbHadar = abHadarAmount ?? 0,
                RangeMas = 0,//
                TafBack = 0,
                TedGhabs = bedbesCount,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = (decimal)tariffInfo.SumBoodje,
                Group1 = tariffInfo.Customer.UsageId,
                Faz = tariffInfo.Customer.SewageCalcState > 0 ? true : false,
                TmpPriDate = string.Empty,
                TmpTodayDate = string.Empty,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = string.Empty,
                EdarehK = tariffInfo.Customer.IsSpecial,
                DateSbt = currentDateJalali,
                Avarez = (decimal)tariffInfo.AvarezAmount,
                TedKhane = tariffInfo.Customer.HouseholdNumber
            };
        }
        public AutoBackCreateDto GetDifferent(BedBesCreateDto bedBes, AutoBackCreateDto repair, int jalaseNumber)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string currentDateJalali10Char = currentDateJalali.Substring(2);

            return new AutoBackCreateDto()
            {
                Town = repair.Town,
                Radif = repair.Radif,
                Eshtrak = repair.Eshtrak,
                Barge = 0,
                PriNo = bedBes.PriNo,
                TodayNo = bedBes.TodayNo,
                PriDate = repair.PriDate,
                TodayDate = repair.TodayDate,
                AbonFas = Diff(bedBes.AbonFas, repair.AbonFas),
                FasBaha = Diff(bedBes.FasBaha, repair.FasBaha),
                AbBaha = Diff(bedBes.AbBaha, repair.AbBaha),
                Ztadil = Diff(bedBes.Ztadil, repair.Ztadil),
                Masraf = Diff(bedBes.Masraf, repair.Masraf),
                Shahrdari = Diff(bedBes.Shahrdari, repair.Shahrdari),
                Modat = bedBes.Modat,
                DateBed = currentDateJalali,
                JalaseNo = jalaseNumber,
                Mohlat = string.Empty,
                Baha = Diff(bedBes.Baha, repair.Baha),
                AbonAb = Diff(bedBes.AbonAb, repair.AbonAb),
                Pard = Diff(bedBes.Pard, repair.Pard),
                Jam = Diff(bedBes.Jam, repair.Jam),
                CodVas = bedBes.CodVas,
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = repair.CodEnshab,
                Enshab = repair.Enshab,
                Elat = repair.Elat,
                Serial = repair.Serial,
                Ser = 0,
                ZaribFasl = Diff(bedBes.ZaribFasl, repair.ZaribFasl),
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
                Operator = bedBes.Operator,
                Mamor = bedBes.Mamor,
                TavizDate = bedBes.TavizDate,
                ZaribCntr = Diff(bedBes.ZaribCntr, repair.ZaribCntr),
                Zabresani = Diff(bedBes.Zabresani, repair.Zabresani),
                ZaribD = Diff(bedBes.ZaribD, repair.ZaribD),
                Tafavot = 0,
                MasHadar = repair.MasHadar,
                AbHadar = repair.AbHadar,
                RangeMas = 0,
                TafBack = 0,
                TedGhabs = repair.TedGhabs,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = Diff(bedBes.Bodjeh, repair.Bodjeh),
                Faz = bedBes.Faz,
                TmpPriDate = string.Empty,
                TmpTodayDate = currentDateJalali10Char,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = currentDateJalali10Char,
            };
        }
        public async Task<IEnumerable<BedBesCreateDto>> GetBedBesList(CustomerInfoOutputDto customerInfo, string fromDateJalali, string toDateJalali)
        {
            ZoneCustomerFromToDateDto bedBesGetDto = new(customerInfo.ZoneId, customerInfo.Radif, fromDateJalali, toDateJalali);
            IEnumerable<BedBesCreateDto> bedBesInfo = await _bedBesQueryService.Get(bedBesGetDto);

            if (!bedBesInfo.Any())
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToRemoved);

            if (bedBesInfo.Any(x => x.Del))
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidBillWithDel);
            }

            return bedBesInfo;
        }
        public AutoBackCreateDto GetBedBes(BedBesCreateDto bedBes, int bedBesCount, int jalaseNumber,int returnCauseId)
        {
            string toDayDateJalali = DateTime.Now.ToShortPersianDateString();

            return new AutoBackCreateDto()
            {
                Town = bedBes.Town,
                Radif = bedBes.Radif,
                Eshtrak = bedBes.Eshtrak,
                Barge = bedBes.Barge,
                PriNo = bedBes.PriNo,
                TodayNo = bedBes.TodayNo,
                PriDate = bedBes.PriDate,
                TodayDate = bedBes.TodayDate,
                AbonFas = bedBes.AbonFas,
                FasBaha = bedBes.FasBaha,
                AbBaha = bedBes.AbBaha,
                Ztadil = bedBes.Ztadil,
                Masraf = bedBes.Masraf,
                Shahrdari = bedBes.Shahrdari,
                Modat = bedBes.Modat,
                DateBed = toDayDateJalali,
                JalaseNo = jalaseNumber,
                Mohlat = string.Empty,//
                Baha = bedBes.Baha,
                AbonAb = bedBes.AbonAb,
                Pard = bedBes.Pard,
                Jam = bedBes.Jam,
                CodVas = bedBes.CodVas,
                Ghabs = string.Empty,//
                Del = false,
                Type = "4",
                CodEnshab = bedBes.CodEnshab,
                Enshab = bedBes.Enshab,
                Elat = returnCauseId,
                Ser = bedBes.Ser,
                Serial = bedBes.Serial,
                ZaribFasl = bedBes.ZaribFasl,
                Ab10 = bedBes.Ab10,
                Ab20 = bedBes.Ab20,
                TedadMas = bedBes.TedadMas,
                TedadTej = bedBes.TedadTej,
                TedadVahd = bedBes.TedadVahd,
                TedGhabs = bedBesCount,
                TedKhane = bedBes.TedKhane,
                NoeVa = bedBes.NoeVa,
                Jarime = bedBes.Jarime,
                Masjar = bedBes.Masjar,
                Sabt = 0,
                Rate = bedBes.Rate,
                Operator = bedBes.Operator,
                Mamor = bedBes.Mamor,
                TavizDate = bedBes.TavizDate,
                ZaribCntr = bedBes.ZaribCntr,
                ZaribD = bedBes.ZaribD,
                Zabresani = bedBes.Zabresani,
                Tafavot = 0,
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,
                TafBack = 0,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = bedBes.Bodjeh,
                Faz = bedBes.Faz,//
                TmpDateBed = string.Empty,
                TmpMohlat = string.Empty,
                TmpPriDate = string.Empty,
                TmpTavizDate = string.Empty,
                TmpTodayDate = string.Empty,
                EdarehK = bedBes.EdarehK,
                Group1 = bedBes.Group1,
            };
        }
        public BedBesCreateDto GetBedbes(IEnumerable<BedBesCreateDto> input, CustomerInfoOutputDto customerInfo)
        {
            var r = input.MaxBy(x => x.DateBed);

            r.Id = 0;
           
            r.Barge = 0;
            r.PriNo = input.Min(x => x.PriNo);
            r.TodayNo = input.Max(x => x.TodayNo);
            r.PriDate = input.Min(x => x.PriDate);
            r.TodayDate = input.Max(x => x.TodayDate);

            r.AbonFas = input.Sum(x => x.AbonFas);
            r.FasBaha = input.Sum(x => x.FasBaha);
            r.AbBaha = input.Sum(x => x.AbBaha);
            r.Ztadil = input.Sum(x => x.Ztadil);
            r.Masraf = input.Sum(x => x.Masraf);
            r.Shahrdari = input.Sum(x => x.Shahrdari);
            r.Modat = input.Sum(x => x.Modat);
            r.JalaseNo = 0;

            r.Baha = input.Sum(x => x.Baha);
            r.AbonAb = input.Sum(x => x.AbonAb);
            r.Pard = input.Sum(x => x.Pard);
            r.Jam = input.Sum(x => x.Jam);

            r.ZaribFasl = input.Sum(x => x.ZaribFasl);
            r.Ab10 = input.Sum(x => x.Ab10);
            r.Ab20 = input.Sum(x => x.Ab20);

            r.Jarime = input.Sum(x => x.Jarime);
            r.Masjar = input.Sum(x => x.Masjar);

            r.Rate = input.Average(x => x.Rate);
            r.ZaribCntr = input.Sum(x => x.ZaribCntr);
            r.Zabresani = input.Sum(x => x.Zabresani);
            r.ZaribD = input.Sum(x => x.ZaribD);
            r.Tafavot = input.Sum(x => x.Tafavot);
            r.KasrHa = input.Sum(x => x.KasrHa);

            r.TabAbnA = input.Sum(x => x.TabAbnA);
            r.TabAbnF = input.Sum(x => x.TabAbnF);
            r.TabsFa = input.Sum(x => x.TabsFa);
            r.NewAb = input.Sum(x => x.NewAb);
            r.NewFa = input.Sum(x => x.NewFa);
            r.Bodjeh = input.Sum(x => x.Bodjeh);

            r.C200 = input.Sum(x => x.C200);
            r.AbSevom = input.Sum(x => x.AbSevom);
            r.AbSevom1 = input.Sum(x => x.AbSevom1);
            r.C70 = input.Sum(x => x.C70);
            r.C80 = input.Sum(x => x.C80);
            r.C90 = input.Sum(x => x.C90);
            r.C101 = input.Sum(x => x.C101);
            r.Tafa402 = input.Sum(x => x.Tafa402);
            r.Avarez = input.Sum(x => x.Avarez);

            r.TedadMas = customerInfo.DomesticUnit;
            r.TedadTej = customerInfo.CommertialUnit;
            r.TedadVahd = customerInfo.OtherUnit;
            r.TedKhane = customerInfo.HouseholdNumber;
            r.KhaliS = customerInfo.EmptyUnit;
            r.NoeVa = customerInfo.BranchType;
            r.CodEnshab = customerInfo.UsageId;

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
        public async Task<int> GetJalaliNumber(int zoneId, int customerNumber)//Todo
        {
            ZoneIdAndCustomerNumberOutputDto customerInfo = new(zoneId, customerNumber);
            int? currentDaypreviousMinutes = await _bedBesQueryService.GetLatestJalaseNumber(customerInfo);
            if (!currentDaypreviousMinutes.HasValue || currentDaypreviousMinutes <= 0)
            {
                var currentDateJalali = DateTime.Now.ToShortPersianDateString();
                string jalaseNumber = currentDateJalali.Substring(3, 1) + currentDateJalali.Substring(5, 2) + currentDateJalali.Substring(8, 2) + 0;
                return int.Parse(jalaseNumber);
            }
            else
            {
                return currentDaypreviousMinutes.Value + 1;
            }
        }
        public async Task FullValidation(ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _returnFullValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        public async Task PartialValidation(ReturnBillPartialInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _returnPartialValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        public void ValidationAmount(decimal repairSumItems, decimal previousSumItems)
        {
            _ = repairSumItems > previousSumItems ? throw new ReturnedBillException(ExceptionLiterals.RepairAmountMoreThanBedBesAmount) : 0;
        }
        public async Task<CustomerInfoOutputDto> Validation(string billId, string fromDateJalali, string toDateJalali)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(billId);

            async Task DateValidation(string date, string exceptionMessage, bool isPreviousDate)
            {
                int count = await _bedBesQueryService.GetCountInDateBed(customerInfo.ZoneId, customerInfo.Radif, date, isPreviousDate);
                _ = count <= 0 ? throw new ReturnedBillException(exceptionMessage) : 0;
            }
            await DateValidation(fromDateJalali, ExceptionLiterals.InvalidFromDate, true);
            await DateValidation(toDateJalali, ExceptionLiterals.InvalidToDate, false);

            return customerInfo;
        }
        public bool IsDomestic(int customerNumber)
        {
            int[] domesticId = [0, 1, 3];
            return domesticId.Contains(customerNumber);
        }
        public async Task<float> GetConsumptionAverage(string fromDateJalali, ReturnedBillCalculationTypeEnum calculationType, float? userInput, CustomerInfoOutputDto customerInfo)
        {
            float previousConsumptionAverage = await _bedBesQueryService.GetPreviousBill(customerInfo.ZoneId, customerInfo.Radif, fromDateJalali);

            return calculationType switch
            {
                ReturnedBillCalculationTypeEnum.ByPreviousConsumptionAverage => previousConsumptionAverage,
                ReturnedBillCalculationTypeEnum.UserInput => userInput.Value,
                _ => userInput.Value
            };
        }
        private async Task CreateAutoBack(AutoBackCreateDto bedBes, AutoBackCreateDto newCalculation, AutoBackCreateDto different)
        {
            ICollection<AutoBackCreateDto> datas = new List<AutoBackCreateDto>() { bedBes, newCalculation, different };
            await _autoBackCommandService.Create(datas);//حتما به ترتیب باید ایجاد شود
        }
        private static decimal Diff(decimal firstValue, decimal secondValue) => firstValue - secondValue;
    }
}
