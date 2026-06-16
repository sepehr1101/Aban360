using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Implementations;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillSetConfirmHandler : AbstractBaseConnection, IReturnBillSetConfirmHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAutoBackQueryService _autoBackQueryService;
        private readonly IRepairQueryService _repairQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private static string _title = "تایید برگشتی";
        public ReturnBillSetConfirmHandler(
            IHttpContextAccessor contextAccessor,
            IAutoBackQueryService autoBackQueryService,
            IRepairQueryService repairQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IBedBesQueryService bedBesQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _autoBackQueryService = autoBackQueryService;
            _autoBackQueryService.NotNull(nameof(autoBackQueryService));

            _repairQueryService = repairQueryService;
            _repairQueryService.NotNull(nameof(repairQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> Handle(ReturnBillSetConfirmInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<AutoBackGetByBargeDto> autoBacksInfo = await _autoBackQueryService.GetByConfirmNumber(input.ConfirmedNumber);
            await _commonZoneService.IsUserInZone(appUser, (int)(autoBacksInfo?.FirstOrDefault()?.Town ?? 0));
            RepairGetDto repairInfo = await _repairQueryService.GetByConfirmNumber(input.ConfirmedNumber);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber((int)repairInfo.Town, (int)repairInfo.Radif));

            if (!input.IsConfirmed)
            {
                return await GetResult(autoBacksInfo, input, memberInfo);
            }


            string zoneDbName = GetDbName((int)(repairInfo?.Town ?? 0));
            string atlasDbName = ReportLiterals.Atlas;

            IEnumerable<BedBesWithDelOutputDto> bedBesIds = await _bedBesQueryService.GetByDateInterval(new ZoneCustomerFromToDateDto((int)repairInfo.Town, (int)repairInfo.Radif, repairInfo.PriDate, repairInfo.TodayDate), zoneDbName);
            IEnumerable<BedBesUpdateDelDto> bedBesUpdateDelDto = bedBesIds.Select(s => new BedBesUpdateDelDto((int)repairInfo.Town, s.Id, true));

            if (autoBacksInfo.FirstOrDefault().IsConfirmed || bedBesIds.Where(b => b.IsReturned).Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidReturnDuplicate);
            }
            IEnumerable<AutoBackCreateDto> autoBackCreate = autoBacksInfo.Select(a => GetAutoBackCreateDto(a));
            RepairCreateDto repairCreate = GetRepairCreateDto(repairInfo);
            string logText = string.Format(OpLogLiterals.BillReturnConfirmedOpLog, memberInfo.BillId, autoBackCreate?.FirstOrDefault()?.TedGhabs ?? 0, autoBackCreate?.FirstOrDefault()?.PriDate ?? string.Empty, autoBackCreate?.FirstOrDefault()?.TodayDate ?? string.Empty, autoBackCreate?.ElementAt(2)?.Baha ?? 0, input.ConfirmedNumber);

            await SqlCommands(autoBackCreate, bedBesUpdateDelDto, repairCreate, memberInfo, appUser, logText, zoneDbName, atlasDbName);
            return await GetResult(autoBacksInfo, input, memberInfo);
        }
        private async Task SqlCommands(IEnumerable<AutoBackCreateDto> autoBacksCreateDto, IEnumerable<BedBesUpdateDelDto> bedBesUpdateDelDto, RepairCreateDto repairCreateDto, MemberInfoGetDto memberInfo, IAppUser appUser, string logText, string zoneDbName, string atlasDbName)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    AutoBackCommandService autoBackCommandService = new(connection, transaction);
                    RepairCommandService repairCommandService = new(connection, transaction);
                    WaterDebtCommandService waterDebtCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    BillCommandService billCommandService = new(connection, transaction);
                    BedBesCommandService bedBesCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await autoBackCommandService.UpdateIsConfirmed((int)repairCreateDto.JalaseNo, atlasDbName);
                    await autoBackCommandService.Create(autoBacksCreateDto, zoneDbName, false);
                    int repairId = await repairCommandService.Insert(repairCreateDto, zoneDbName);
                    await waterDebtCommandService.UpdateAmount(memberInfo.BillId, (long)repairCreateDto.Baha);
                    await membersCommandService.UpdateBedbes(new ZoneIdAndCustomerNumber(memberInfo.ZoneId, memberInfo.CustomerNumber), (long)repairCreateDto.Baha, zoneDbName);
                    await billCommandService.InsertReturnByRepair(repairId, zoneDbName);
                    await bedBesCommandService.UpdateDel(bedBesUpdateDelDto, zoneDbName);
                    await opLogCommandService.Insert(logText, appUser);

                    transaction.Commit();
                }
            }
        }
        private AutoBackCreateDto GetAutoBackCreateDto(AutoBackGetByBargeDto input)
        {
            return new AutoBackCreateDto()
            {
                Town = input.Town,
                Radif = input.Radif,
                Eshtrak = input.Eshtrak,
                Barge = input.Barge,
                PriNo = input.PriNo,
                TodayNo = input.TodayNo,
                PriDate = input.PriDate,
                TodayDate = input.TodayDate,
                AbonFas = input.AbonFas,
                FasBaha = input.FasBaha,
                AbBaha = input.AbBaha,
                Ztadil = input.Ztadil,
                Masraf = input.Masraf,
                Shahrdari = input.Shahrdari,
                Modat = input.Modat,
                DateBed = input.DateBed,
                JalaseNo = input.JalaseNo,
                Mohlat = input.Mohlat,
                Baha = input.Baha,
                AbonAb = input.AbonAb,
                Pard = input.Pard,
                Jam = input.Jam,
                CodVas = input.CodVas,
                Ghabs = input.Ghabs,
                Del = input.Del,
                Type = input.Type,
                CodEnshab = input.CodEnshab,
                Enshab = input.Enshab,
                Elat = input.Elat,
                Serial = input.Serial,
                Ser = input.Ser,
                ZaribFasl = input.ZaribFasl,
                Ab10 = input.Ab10,
                Ab20 = input.Ab20,
                TedadVahd = input.TedadVahd,
                TedKhane = input.TedKhane,
                TedadMas = input.TedadMas,
                TedadTej = input.TedadTej,
                NoeVa = input.NoeVa,
                Jarime = input.Jarime,
                Masjar = input.Masjar,
                Sabt = input.Sabt,
                Rate = input.Rate,
                Operator = input.Operator,
                Mamor = input.Mamor,
                TavizDate = input.TavizDate,
                ZaribCntr = input.ZaribCntr,
                Zabresani = input.Zabresani,
                ZaribD = input.ZaribD,
                Tafavot = input.Tafavot,
                MasHadar = input.MasHadar,
                AbHadar = input.AbHadar,
                RangeMas = input.RangeMas,
                TafBack = input.TafBack,
                TedGhabs = input.TedGhabs,
                TabAbnA = input.TabAbnA,
                TabAbnF = input.TabAbnF,
                TabsFa = input.TabsFa,
                Bodjeh = input.Bodjeh,
                Faz = input.Faz,
                TmpPriDate = input.TmpPriDate,
                TmpTodayDate = input.TmpTodayDate,
                TmpDateBed = input.TmpDateBed,
                TmpMohlat = input.TmpMohlat,
                TmpTavizDate = input.TmpTavizDate,
                Group1 = input.Group1,
                EdarehK = input.EdarehK,
                DateSbt = input.DateSbt,
                Avarez = input.Avarez,
            };
        }
        private RepairCreateDto GetRepairCreateDto(RepairGetDto input)
        {
            return new RepairCreateDto()
            {
                Town = input.Town,
                Radif = input.Radif,
                Eshtrak = input.Eshtrak,
                Barge = input.Barge,
                PriNo = input.PriNo,
                TodayNo = input.TodayNo,
                PriDate = input.PriDate,
                TodayDate = input.TodayDate,
                AbonFas = input.AbonFas,
                FasBaha = input.FasBaha,
                AbBaha = input.AbBaha,
                Ztadil = input.Ztadil,
                Masraf = input.Masraf,
                Shahrdari = input.Shahrdari,
                Modat = input.Modat,
                DateBed = input.DateBed,
                JalaseNo = input.JalaseNo,
                Mohlat = input.Mohlat,
                Baha = input.Baha,
                AbonAb = input.AbonAb,
                Pard = input.Pard,
                Jam = input.Jam,
                CodVas = input.CodVas,
                Ghabs = input.Ghabs,
                Del = input.Del,
                Type = input.Type,
                CodEnshab = input.CodEnshab,
                Enshab = input.Enshab,
                Elat = input.Elat,
                Serial = input.Serial,
                Ser = input.Ser,
                ZaribFasl = input.ZaribFasl,
                Ab10 = input.Ab10,
                Ab20 = input.Ab20,
                TedadVahd = input.TedadVahd,
                TedKhane = input.TedKhane,
                TedadMas = input.TedadMas,
                TedadTej = input.TedadTej,
                NoeVa = input.NoeVa,
                Jarime = input.Jarime,
                Masjar = input.Masjar,
                Sabt = input.Sabt,
                Rate = input.Rate,
                Operator = input.Operator,
                Mamor = input.Mamor,
                TavizDate = input.TavizDate,
                ZaribCntr = input.ZaribCntr,
                Zabresani = input.Zabresani,
                ZaribD = input.ZaribD,
                Tafavot = input.Tafavot,
                MasHadar = input.MasHadar,
                AbHadar = input.AbHadar,
                RangeMas = input.RangeMas,
                TafBack = input.TafBack,
                TedGhabs = input.TedGhabs,
                TabAbnA = input.TabAbnA,
                TabAbnF = input.TabAbnF,
                TabsFa = input.TabsFa,
                Bodjeh = input.Bodjeh,
                Group1 = input.Group1,
                Faz = input.Faz,
                ChkKarbari = input.ChkKarbari,
                C200 = input.C200,
                TmpPriDate = input.TmpPriDate,
                TmpTodayDate = input.TmpTodayDate,
                TmpMohlat = input.TmpMohlat,
                TmpTavizDate = input.TmpTavizDate,
                TmpDateBed = input.TmpDateBed,
                EdarehK = input.EdarehK,
                Lavazem = input.Lavazem,
                DateSbt = input.DateSbt,
                Avarez = input.Avarez,
            };
        }
        private async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> GetResult(IEnumerable<AutoBackGetByBargeDto> returnedInfo, ReturnBillSetConfirmInputDto input, MemberInfoGetDto memberInfo)
        {
            AutoBackGetByBargeDto firstAutoBackInfo = returnedInfo.FirstOrDefault();
            string description = "";//todo

            AutoBackGetByBargeDto bedBes = returnedInfo.ElementAt(0);
            AutoBackGetByBargeDto newCalculation = returnedInfo.ElementAt(1);
            AutoBackGetByBargeDto different = returnedInfo.ElementAt(2);
            ReturnBillDataOutputDto previousValues = new ReturnBillDataOutputDto()
            {
                ZoneId = bedBes.Town,
                CustomerNumber = bedBes.Radif,
                ReadingNumber = bedBes.Eshtrak,
                PreviousNumber = bedBes.PriNo,
                CurrentNumber = bedBes.TodayNo,
                PreviousDateJalali = bedBes.PriDate,
                CurrentDateJalali = bedBes.TodayDate,
                MinutesNumber = (int)bedBes.JalaseNo,
                Item4 = bedBes.AbonFas,
                Item2 = bedBes.FasBaha,
                Item1 = bedBes.AbBaha,
                Item12 = bedBes.Ztadil,
                Consumption = bedBes.Masraf,
                Item5 = bedBes.Shahrdari,
                Duration = bedBes.Modat,
                RegisterDateJalali = bedBes.DateBed,
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
                BillCount = bedBes.TedGhabs,
                Item18 = bedBes.Bodjeh,
                UsageConsumption = bedBes.Group1,
                HasSewage = bedBes.Faz,
                IsSpecial = bedBes.EdarehK,
                Lavazem = 0
            };
            ReturnBillDataOutputDto currentValues = new ReturnBillDataOutputDto()
            {
                ZoneId = newCalculation.Town,
                CustomerNumber = newCalculation.Radif,
                ReadingNumber = newCalculation.Eshtrak,
                PreviousNumber = newCalculation.PriNo,
                CurrentNumber = newCalculation.TodayNo,
                PreviousDateJalali = newCalculation.PriDate,
                CurrentDateJalali = newCalculation.TodayDate,
                MinutesNumber = (int)newCalculation.JalaseNo,
                Item4 = newCalculation.AbonFas,
                Item2 = newCalculation.FasBaha,
                Item1 = newCalculation.AbBaha,
                Item12 = newCalculation.Ztadil,
                Consumption = newCalculation.Masraf,
                Item5 = newCalculation.Shahrdari,
                Duration = newCalculation.Modat,
                RegisterDateJalali = newCalculation.DateBed,
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
            ReturnBillDataOutputDto returnValues = new ReturnBillDataOutputDto()
            {
                ZoneId = different.Town,
                CustomerNumber = different.Radif,
                ReadingNumber = different.Eshtrak,
                PreviousNumber = different.PriNo,
                CurrentNumber = different.TodayNo,
                PreviousDateJalali = different.PriDate,
                CurrentDateJalali = different.TodayDate,
                MinutesNumber = (int)different.JalaseNo,
                Item4 = different.AbonFas,
                Item2 = different.FasBaha,
                Item1 = different.AbBaha,
                Item12 = different.Ztadil,
                Consumption = different.Masraf,
                Item5 = different.Shahrdari,
                Duration = different.Modat,
                RegisterDateJalali = different.DateBed,
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
            ReturnBillOutputDto data = new(previousValues, currentValues, returnValues);
            ReturnBillHeaderOutputDto header = new(description, memberInfo.ZoneTitle, input.ConfirmedNumber, string.Empty, input.IsConfirmed);

            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = new(_title, header, data);
            return result;
        }
    }
}