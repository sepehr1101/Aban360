using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Db.Services.Dtos;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Implementations;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillManualHandler : AbstractBaseConnection, IReturnBillManualHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly ICommonZoneService _zoneService;
        private readonly IVariabService _variabService;
        private readonly IReturnBillBaseHandler _returnBillBaseHandler;
        private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IBillQueryService _billQueryService;
        private readonly IValidator<ReturnBillManualInputDto> _validator;
        private static int _operator = ReportLiterals.RayabOperator;
        private static string _currentDateJalali = DateTime.Now.ToShortPersianDateString();
        public ReturnBillManualHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService memberQueryService,
            ICommonZoneService zoneService,
            IVariabService variabService,
            IReturnBillBaseHandler returnBillBaseHandler,
            IBedBesQueryService bedBesQueryService,
            IBillQueryService billQueryService,
            IBillReturnCauseQueryService billReturnCauseQueryService,
            IValidator<ReturnBillManualInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

            _billReturnCauseQueryService = billReturnCauseQueryService;
            _billReturnCauseQueryService.NotNull(nameof(billReturnCauseQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _returnBillBaseHandler = returnBillBaseHandler;
            _returnBillBaseHandler.NotNull(nameof(returnBillBaseHandler));
        }


        public async Task<BillReturnManualOutputDto> Handle(ReturnBillManualInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            string returnCauseTitle = await ValidateAndGetReturnCause(inputDto, cancellationToken);
            CustomerInfoOutputDto customerInfo = await _returnBillBaseHandler.RealDateValidate(appUser, inputDto.BillId, inputDto.FromDateJalali, inputDto.ToDateJalali);
            CustomerLastInfoGetDto memberInfo = await _memberQueryService.GetMembersBedBesTavizFlatInfo(customerInfo.ZoneId, customerInfo.Radif, true);

            int minutesNumber = await _returnBillBaseHandler.GetMinutesNumber(null, customerInfo.ZoneId, customerInfo.Radif);
            string dbName = GetDbName(customerInfo.ZoneId);

            var (bedBesResult, bedBesUpdateDelDto) = await ValidateAndGetBedBesInfo(inputDto, customerInfo);
            RepairCreateDto repairCreateDto = GetRepairDto(bedBesResult, memberInfo, inputDto, minutesNumber, bedBesUpdateDelDto?.Count() ?? 0, memberInfo.DebtAmount ?? 0);
            AutoBackCreateDto autoBackDto = GetAutoBackCreate(repairCreateDto, inputDto.Description);
            ICollection<AutoBackCreateDto> autoBackCreateDto = new List<AutoBackCreateDto>() { autoBackDto, autoBackDto, autoBackDto };
            IEnumerable<BillsOldDbDelUpdateDto> billsIdToReturn = await GetBillsToReturn(customerInfo, repairCreateDto);
            if ((billsIdToReturn?.Count() ?? 0) != (bedBesUpdateDelDto?.Count() ?? 0))
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidBillIdToReturn);
            }

            if (!inputDto.IsConfirm)
            {
                return GetResult(repairCreateDto, customerInfo, returnCauseTitle);
            }

            string opLogText = string.Format(OpLogLiterals.BillReturnedManualInsertOpLog, customerInfo.BillId, repairCreateDto.TedGhabs, repairCreateDto.PriDate, repairCreateDto.TodayDate, repairCreateDto.Baha, repairCreateDto.JalaseNo);
            long amount = inputDto.ReturnType == ReturnBillManualTypeEnum.Returned ? -1 * (long)repairCreateDto.Baha : (long)repairCreateDto.Baha;
            await ExecSql(autoBackCreateDto, bedBesUpdateDelDto, billsIdToReturn, repairCreateDto, customerInfo, appUser, opLogText, dbName, amount);

            return GetResult(repairCreateDto, customerInfo, returnCauseTitle);
        }
        private async Task ExecSql(ICollection<AutoBackCreateDto> autoBackCreateDto, IEnumerable<BedBesUpdateDelDto> bedBesUpdateDelDto, IEnumerable<BillsOldDbDelUpdateDto> billsIdToReturn, RepairCreateDto repairCreateDto, CustomerInfoOutputDto customerInfo, IAppUser appUser, string logText, string dbName, long amount)
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


                    await autoBackCommandService.Create(autoBackCreateDto, ReportLiterals.Atlas, true);
                    int repairId = await repairCommandService.Insert(repairCreateDto, dbName);
                    await waterDebtCommandService.UpdateAmount(customerInfo.BillId, amount);
                    await membersCommandService.UpdateBedbes(new ZoneIdAndCustomerNumber(customerInfo.ZoneId, customerInfo.Radif), amount, dbName);
                    await billCommandService.InsertReturnByRepair(repairId, dbName);
                    await billCommandService.UpdateOldDbDel(billsIdToReturn);
                    await bedBesCommandService.UpdateDel(bedBesUpdateDelDto, dbName);
                    await opLogCommandService.Insert(logText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task<(BedBesCreateDto, IEnumerable<BedBesUpdateDelDto>)> ValidateAndGetBedBesInfo(ReturnBillManualInputDto inputDto, CustomerInfoOutputDto customerInfo)
        {
            ZoneCustomerFromToDateDto bedBesGetDto = new(customerInfo.ZoneId, customerInfo.Radif, inputDto.FromDateJalali, inputDto.ToDateJalali);
            IEnumerable<BedBesCreateDto> bedBesInfo = await _bedBesQueryService.Get(bedBesGetDto);
            if (!bedBesInfo.Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToRemoved);
            }

            BedBesCreateDto bedBesResult = _returnBillBaseHandler.GetBedbes(bedBesInfo, customerInfo);
            long inputAmounts = inputDto.Item1 + inputDto.Item2 + inputDto.Item3 + inputDto.Item4 + inputDto.Item5 + inputDto.Item6 + inputDto.Item10 + inputDto.Item11 + inputDto.Item13 + inputDto.Item14 + inputDto.Item16;
            if (inputAmounts > bedBesResult.Baha)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidInputAmountGTBedBes);
            }
            IEnumerable<BedBesUpdateDelDto> bedBesUpdateDelDto = bedBesInfo.Select(s => new BedBesUpdateDelDto(customerInfo.ZoneId, (long)s.Id, true));

            return (bedBesResult, bedBesUpdateDelDto);
        }
        private async Task<IEnumerable<BillsOldDbDelUpdateDto>> GetBillsToReturn(CustomerInfoOutputDto customerInfo, RepairCreateDto repairCreateDto)
        {
            BillFindReturnedInputDto returnedValues = new(customerInfo.ZoneId, customerInfo.Radif, (int)repairCreateDto.PriNo, (int)repairCreateDto.TodayNo, repairCreateDto.PriDate, repairCreateDto.TodayDate);
            IEnumerable<BillsOldDbDelUpdateDto> billsIdToReturn = await _billQueryService.GetReturnedId(returnedValues);
            foreach (var item in billsIdToReturn)
            {
                item.OldDbDel = true;
            }

            return billsIdToReturn;
        }
        private AutoBackCreateDto GetAutoBackCreate(RepairCreateDto repairDto, string? description)
        {
            return new AutoBackCreateDto()
            {
                Town = repairDto.Town,
                Radif = repairDto.Radif,
                Eshtrak = repairDto.Eshtrak,
                Barge = repairDto.Barge,
                PriNo = repairDto.PriNo,
                TodayNo = repairDto.TodayNo,
                PriDate = repairDto.PriDate,
                TodayDate = repairDto.TodayDate,
                AbonFas = repairDto.AbonFas,
                FasBaha = repairDto.FasBaha,
                AbBaha = repairDto.AbBaha,
                Ztadil = repairDto.Ztadil,
                Masraf = repairDto.Masraf,
                Shahrdari = repairDto.Shahrdari,
                Modat = repairDto.Modat,
                DateBed = repairDto.DateBed,
                JalaseNo = repairDto.JalaseNo,
                Mohlat = string.Empty,//
                Baha = repairDto.Baha,
                AbonAb = repairDto.AbonAb,
                Pard = repairDto.Pard,
                Jam = repairDto.Jam,
                CodVas = repairDto.CodVas,
                Ghabs = "0",//
                Del = false,
                Type = repairDto.Type,
                CodEnshab = repairDto.CodEnshab,
                Enshab = repairDto.Enshab,
                Elat = repairDto.Elat,
                Ser = repairDto.Ser,
                Serial = repairDto.Serial,
                ZaribFasl = repairDto.ZaribFasl,
                Ab10 = repairDto.Ab10,
                Ab20 = repairDto.Ab20,
                TedadMas = repairDto.TedadMas,
                TedadTej = repairDto.TedadTej,
                TedadVahd = repairDto.TedadVahd,
                TedGhabs = repairDto.TedGhabs,
                TedKhane = repairDto.TedKhane,
                NoeVa = repairDto.NoeVa,
                Jarime = repairDto.Jarime,
                Masjar = repairDto.Masjar,
                Sabt = 0,
                Rate = repairDto.Rate,
                Operator = repairDto.Operator,
                Mamor = repairDto.Mamor,
                TavizDate = repairDto.TavizDate,
                ZaribCntr = repairDto.ZaribCntr,
                ZaribD = repairDto.ZaribD,
                Zabresani = repairDto.Zabresani,
                Tafavot = repairDto.Tafavot,
                MasHadar = repairDto.MasHadar,
                AbHadar = repairDto.AbHadar,
                RangeMas = repairDto.RangeMas,
                TafBack = repairDto.TafBack,
                TabAbnA = repairDto.TabAbnA,
                TabAbnF = repairDto.TabAbnF,
                TabsFa = repairDto.TabsFa,
                Bodjeh = repairDto.Bodjeh,
                Faz = repairDto.Faz,//
                TmpDateBed = repairDto.TmpDateBed,
                TmpMohlat = repairDto.TmpMohlat,
                TmpPriDate = repairDto.TmpPriDate,
                TmpTavizDate = repairDto.TmpTavizDate,
                TmpTodayDate = repairDto.TmpTodayDate,
                EdarehK = repairDto.EdarehK,
                Group1 = repairDto.Group1,
                Avarez = repairDto.Avarez,
                DateSbt = repairDto.DateSbt,
                Description = description
            };
        }
        private RepairCreateDto GetRepairDto(BedBesCreateDto bedBesDto, CustomerLastInfoGetDto memberInfo, ReturnBillManualInputDto inputDto, int minutesNumber, int billCount, long debtAmount)
        {
            long amount = inputDto.Item1 + inputDto.Item2 + inputDto.Item3 + inputDto.Item4 + inputDto.Item5 + inputDto.Item6 + inputDto.Item10 + inputDto.Item11 + inputDto.Item13 + inputDto.Item14 + inputDto.Item16;
            return new RepairCreateDto()
            {
                Town = bedBesDto.Town,
                Radif = bedBesDto.Radif,
                Eshtrak = bedBesDto.Eshtrak,
                Barge = bedBesDto.Barge,
                PriNo = bedBesDto.PriNo,
                TodayNo = bedBesDto.TodayNo,
                PriDate = bedBesDto.PriDate,
                TodayDate = bedBesDto.TodayDate,

                AbonFas = inputDto.Item4,
                FasBaha = inputDto.Item2,
                AbBaha = inputDto.Item1,
                Ztadil = 0,
                Masraf = 0,//todo
                Shahrdari = inputDto.Item5,
                Modat = bedBesDto.Modat,
                DateBed = _currentDateJalali,
                JalaseNo = minutesNumber,
                Mohlat = bedBesDto.Mohlat,
                Baha = amount,
                AbonAb = inputDto.Item3,
                Pard = amount + debtAmount,
                Jam = amount + debtAmount,
                CodVas = bedBesDto.CodVas,
                Ghabs = "0",
                Del = false,
                Type = ((int)inputDto.ReturnType).ToString(),
                CodEnshab = memberInfo.UsageId,
                Enshab = memberInfo.MeterDiameterId,
                Elat = inputDto.ReturnCauseId,
                Serial = bedBesDto.Serial,
                Ser = bedBesDto.Ser,
                ZaribFasl = inputDto.Item11,
                Ab10 = inputDto.Item6,
                Ab20 = 0,
                TedadVahd = memberInfo.OtherUnit,
                TedKhane = memberInfo.HouseholdNumber,
                TedadMas = memberInfo.DomesticUnit,
                TedadTej = memberInfo.CommercialUnit,
                NoeVa = memberInfo.UseStateId,
                Jarime = 0,
                Masjar = 0,
                Sabt = 0,
                Rate = 0,
                Operator = _operator,
                Mamor = 0,
                TavizDate = memberInfo.ChangeDateJalali,
                ZaribCntr = 0,//todo
                Zabresani = 0,
                ZaribD = inputDto.Item10,
                Tafavot = 0,
                MasHadar = 0,//todo
                AbHadar = 0,//todo
                RangeMas = 0,//todo
                TafBack = 0,//todoe
                TedGhabs = billCount,
                TabAbnA = inputDto.Item13,
                TabAbnF = inputDto.Item14,
                TabsFa = 0,
                Bodjeh = inputDto.Item16,
                Group1 = memberInfo.UsageConsumptionId,
                Faz = bedBesDto.Faz,
                ChkKarbari = 0,//todo
                C200 = 0,
                TmpPriDate = string.Empty,
                TmpTodayDate = string.Empty,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = string.Empty,
                EdarehK = memberInfo.IsSpecial,
                Lavazem = 0,//todo
                DateSbt = _currentDateJalali,
                Avarez = 0,
            };
        }
        private BillReturnManualOutputDto GetResult(RepairCreateDto repairDto, CustomerInfoOutputDto customerInfo, string returnCauseTitle)
        {
            return new BillReturnManualOutputDto()
            {
                BillId = customerInfo.BillId,
                ReportDateJalali = _currentDateJalali,
                Title = ReportLiterals.ReturnManual,
                Amount = (long)repairDto.Baha,
                ReturnCauseTitle = returnCauseTitle,
                FullName = customerInfo.FullName,
                ZoneTitle = customerInfo.ZoneTitle,
            };
        }
        private async Task<string> ValidateAndGetReturnCause(ReturnBillManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            IEnumerable<NumericDictionary> returnPartialCauseList = await _billReturnCauseQueryService.Get(false);
            NumericDictionary? returnCauseSelected = returnPartialCauseList?.Where(r => r.Id == inputDto.ReturnCauseId)?.FirstOrDefault();
            if (returnCauseSelected is null)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidReturnCauseId);
            }

            return returnCauseSelected.Title;
        }
        private async Task InputValidate(ReturnBillManualInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}