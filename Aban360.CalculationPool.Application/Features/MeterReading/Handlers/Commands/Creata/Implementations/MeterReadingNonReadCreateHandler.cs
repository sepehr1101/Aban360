using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingNonReadCreateHandler : AbstractBaseConnection, IMeterReadingNonReadCreateHandler
    {
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly IPreviousAverageHandler _previousAverageHandler;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IBillQueryService _billQueryService;
        private readonly IValidator<MeterReadingNonReadInputDto> _validator;
        const string _reportTitle = "قبض دسته‌ای علی‌الحساب";
        private int _agentCode = 0;
        private short _nonReadCounterStateId = 8;//todo:?????
        private double _maxAmount = 999_999_999_999;
        private int _conditionPayableAmount = 10000;
        private int _paymentDeadline = 7;
        private int _maxDayCondition = -15;
        public MeterReadingNonReadCreateHandler(
           IMeterFlowQueryService meterFlowService,
           ICustomerInfoService customerInfoService,
           IMeterReadingDetailQueryService meterReadingDetailService,
           IOldTariffEngine tariffEngine,
           IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
           IPreviousAverageHandler previousAverageHandler,
           IBedBesQueryService bedBesQueryService,
           IBillQueryService billQueryService,
           IValidator<MeterReadingNonReadInputDto> validator,
           IConfiguration configuration)
           : base(configuration)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(_meterFlowService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(_customerInfoService));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(_meterReadingDetailService));

            _tariffEngine = tariffEngine;
            _tariffEngine.NotNull(nameof(tariffEngine));

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _previousAverageHandler = previousAverageHandler;
            _previousAverageHandler.NotNull(nameof(_previousAverageHandler));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(MeterReadingNonReadInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(input, cancellationToken);
            Guid fileNameGuid = Guid.NewGuid();
            string fileName = $"{ReportLiterals.NonRead}-{fileNameGuid}";
            string dateJalaliCondition = ConvertDate.JalaliToDateTime(input.CurrentDateJalali).AddDays(_maxDayCondition).ToShortPersianDateString();

            IEnumerable<BillLatestListDataOutputDto> latestBills = await _billQueryService.GetLatestForNonRead(new BillLatestListInputDto(input.ZoneId, input.FromReadingNumber, input.ToReadingNumber), dateJalaliCondition);
            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(latestBills, input, appUser, fileName);
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = new List<MeterReadingDetailCreateDto>();
            foreach (var readingDetail in readingDetails)
            {
                try
                {
                    MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto()
                    {
                        BillId = readingDetail.BillId,
                        CurrentDateJalali = readingDetail.CurrentDateJalali,
                        MonthlyAverageConsumption = readingDetail.MonthlyConsumption ?? 0,
                        PreviousDateJalali = readingDetail.PreviousDateJalali,
                    };

                    AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterInfo, cancellationToken);
                    if (abBahaCalc.SumItems > _maxAmount)
                    {
                        throw new InvalidBillCommandException(ExceptionLiterals.InvalidDisallowedAmount(readingDetail.BillId, _maxAmount));
                    }
                    readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false, null));
                }
                catch (Exception ex) when (IsInException(ex))
                {
                    readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, appUser.UserId));
                }
            }
            await ExecSql(readingDetailsCreate, appUser, fileName);
            return GetReturnData(readingDetailsCreate);
        }
        private async Task ExecSql(ICollection<MeterReadingDetailCreateDto> readingDetailsCreate, IAppUser appUser, string fileName)
        {
            int firstFlowId = readingDetailsCreate.FirstOrDefault().FlowImportedId;
            int zoneId = readingDetailsCreate.FirstOrDefault().ZoneId;

            MeterFlowDeleteDto meterFlowDeleteDto = new(firstFlowId, appUser.UserId, DateTime.Now);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterReadingDetailCommandService meterReadingDetailService = new(connection, transaction);
                    MeterFlowCommandService meterFlowCommand = new(connection, transaction);

                    try
                    {
                        await meterReadingDetailService.Insert(readingDetailsCreate);
                        await MeterFlowCommands(connection, transaction, firstFlowId, zoneId, fileName, appUser, string.Empty);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        await meterFlowCommand.Delete(meterFlowDeleteDto);
                        throw ex;
                        //DeleteFromDisk(filePath);//todo:Error
                    }
                }
            }

        }
        private ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> GetReturnData(IEnumerable<MeterReadingDetailCreateDto> data)
        {
            int[] closedAndObstacleCounterState = { 4, 7, 8 };
            MeterReadingDetailHeaderOutputDto header = new MeterReadingDetailHeaderOutputDto()
            {
                Amount = data.Sum(m => m.SumItems) ?? 0,
                Consumption = data.Sum(m => m.Consumption) ?? 0,
                RecordCount = data.Count(),
                Closed = data.Count(r => r.CurrentCounterStateCode == 4),
                Obstacle = data.Count(r => r.CurrentCounterStateCode == 7),
                Temporarily = data.Count(r => r.CurrentCounterStateCode == 8),
                PureReading = data.Count(r => !closedAndObstacleCounterState.Contains(r.CurrentCounterStateCode)),
                Malfunction = data.Count(r => r.CurrentCounterStateCode == 1)
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = new(_reportTitle, header, data);

            return result;
        }

        private async Task MeterFlowCommands(IDbConnection connection, IDbTransaction transaction, int latestFlowId, int ZoneId, string fileName, IAppUser appUser, string? description)
        {
            MeterFlowCommandService meterFlowService = new(connection, transaction);

            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            await meterFlowService.Update(meterFlowUpdate);

            MeterFlowCreateDto newMeterFlow = GetMeterFlowCreateDto(MeterFlowStepEnum.Calculated, fileName, ZoneId, appUser.UserId, description);
            await meterFlowService.Insert(newMeterFlow);
        }
        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(IEnumerable<BillLatestListDataOutputDto> latestBills, MeterReadingNonReadInputDto input, IAppUser appUser, string fileName)
        {
            MeterFlowCreateDto importedMeterFlow = GetMeterFlowCreateDto(MeterFlowStepEnum.Imported, fileName, input.ZoneId, appUser.UserId, string.Empty);
            CustomersInfoGetDto customersInfo;
            IEnumerable<ZoneIdAndCustomerNumber> customersByInvalidPreviousBedBes = new List<ZoneIdAndCustomerNumber>();
            int meterFlowId = 0;

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterFlowCommandService meterflowCommandService = new(connection, transaction);
                    meterFlowId = await meterflowCommandService.Insert(importedMeterFlow);
                    customersInfo = await _customerInfoService.GetByBulkCopy(connection, transaction, input.ZoneId, latestBills.Select(m => m.CustomerNumber).ToList());
                    transaction.Commit();
                }
            }
            IEnumerable<MeterReadingDetailCreateDto> meterReadingsDetailCreate = GetReadingMeterDetails(latestBills, customersInfo, appUser, meterFlowId, input.CurrentDateJalali);

            return meterReadingsDetailCreate;
        }
        private IEnumerable<MeterReadingDetailCreateDto> GetReadingMeterDetails(IEnumerable<BillLatestListDataOutputDto> latestBills, CustomersInfoGetDto customersInfo, IAppUser appUser, int meterFlowId, string currentDateJalali)
        {
            return from latestBill in latestBills
                   join members in customersInfo.MembersInfo
                       on latestBill.CustomerNumber equals members.CustomerNumber
                       into membersJoin
                   from members in membersJoin.DefaultIfEmpty()
                   join taviz in customersInfo.TavizInfo
                       on members.CustomerNumber equals taviz.CustomerNumber
                       into tavizJoin
                   from taviz in tavizJoin.DefaultIfEmpty()
                   select new MeterReadingDetailCreateDto()
                   {
                       FlowImportedId = meterFlowId,
                       ZoneId = latestBill.ZoneId,
                       CustomerNumber = latestBill.CustomerNumber,
                       ReadingNumber = latestBill.ReadingNumber,
                       BillId = members.BillId,
                       AgentCode = _agentCode,
                       CurrentCounterStateCode = _nonReadCounterStateId,
                       PreviousDateJalali = latestBill.PreviousDateJalali,
                       CurrentDateJalali = currentDateJalali,
                       PreviousNumber = latestBill.PreviousNumber,
                       CurrentNumber = 0,//todo:true?
                       InsertByUserId = appUser.UserId,
                       InsertDateTime = DateTime.Now,

                       BranchTypeId = members.BranchTypeId,
                       UsageId = members.UsageId,
                       ConsumptionUsageId = members.ConsumptionUsageId,
                       DomesticUnit = members.DomesticUnit,
                       CommercialUnit = members.CommercialUnit,
                       OtherUnit = members.OtherUnit,
                       EmptyUnit = members.EmptyUnit,
                       WaterInstallationDateJalali = members.WaterInstallationDateJalali,
                       SewageInstallationDateJalali = members.SewageInstallationDateJalali,
                       WaterRegisterDate = members.WaterRegisterDate,
                       SewageRegisterDate = members.WaterRegisterDate,
                       WaterCount = members.WaterCount,
                       SewageCalcState = members.SewageCalcState,
                       ContractualCapacity = members.ContractualCapacity,
                       HouseholdDate = members.HouseholdDate,
                       HouseholdNumber = members.HouseholdNumber,
                       VillageId = members.VillageId,
                       IsSpecial = members.IsSpecial,
                       MeterDiameterId = members.MeterDiameterId,
                       VirtualCategoryId = members.VirtualCategoryId,
                       BodySerial = members.BodySerial,

                       TavizCause = taviz?.TavizCause,
                       TavizDateJalali = taviz?.TavizDateJalali,
                       TavizNumber = taviz?.TavizNumber,
                       TavizRegisterDateJalali = taviz?.TavizRegisterDateJalali,

                       MonthlyConsumption = latestBill.ConsumptionAverage,
                       LastMeterDateJalali = latestBill is null ? members.WaterInstallationDateJalali : latestBill.PreviousDateJalali,
                       LastMeterNumber = latestBill?.PreviousNumber ?? 0,
                       LastConsumption = latestBill?.Consumption ?? 0,
                       LastMonthlyConsumption = latestBill.ConsumptionAverage,
                       LastCounterStateCode = latestBill?.CounterStateCode ?? 0,
                       LastSumItems = latestBill?.PreviousSumItems ?? 0
                   };
        }
        private async Task<MeterReadingDetailCreateDto> GetMeterReadingDetailByAbBahaValue(MeterReadingDetailCreateDto r, AbBahaCalculationDetails? abBahaCalc, bool hasZeroValue, Guid? userIdExclude)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(new ZoneIdAndCustomerNumber(r.ZoneId, r.CustomerNumber));//checkResult: changeDto
            var (sumItems, jam, pard) = GetAmounts(preDebtAmount, abBahaCalc?.SumItems ?? 0);
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();

            r.ExcludedByUserId = userIdExclude;
            r.ExcludedDateTime = userIdExclude.HasValue ? DateTime.Now : null;
            r.ExcludedCauseId = userIdExclude.HasValue ? (int)ExcludedCauseEnum.Error : null;
            r.ExcludedCauseTitle = userIdExclude.HasValue ? ReportLiterals.Error : null;
            r.Barge = 0;
            r.PriNo = r.PreviousNumber;
            r.TodayNo = r.CurrentNumber;
            r.PriDate = r.PreviousDateJalali;
            r.TodayDate = r.CurrentDateJalali;
            r.AbonAb = (decimal)(abBahaCalc?.AbonmanAbAmount ?? 0);
            r.AbonFas = (decimal)(abBahaCalc?.AbonmanFazelabAmount ?? 0);
            r.FasBaha = ((decimal)(abBahaCalc?.FazelabAmount ?? 0)) + ((decimal)(abBahaCalc?.HotSeasonFazelabAmount ?? 0));
            r.AbBaha = (decimal)(abBahaCalc?.AbBahaAmount ?? 0);
            r.Ztadil = 0;//todo
            r.Masraf = (decimal)(abBahaCalc?.Consumption ?? 0);
            r.Shahrdari = (decimal)(abBahaCalc?.MaliatAmount ?? 0);
            r.Modat = abBahaCalc?.Duration ?? 0;
            r.DateBed = DateTime.Now.ToShortPersianDateString();
            r.JalaseNo = 0;//todo
            r.Mohlat = mohlatDateJalali;
            r.Baha = (decimal)sumItems;
            r.Pard = (decimal)pard;
            r.Jam = (decimal)jam;
            r.CodVas = r.CurrentCounterStateCode;
            r.Ghabs = "1";
            r.Del = false;
            r.Type = "1";
            r.CodEnshab = r.UsageId;
            r.Enshab = r.MeterDiameterId;
            r.Elat = 0;
            r.Serial = 0;// string.IsNullOrWhiteSpace(meterReaing.BodySerial) ? 0 : int.Parse(meterReaing.BodySerial);//todo
            r.Ser = 0;// string.IsNullOrWhiteSpace(meterReaing.BodySerial) ? 0 : int.Parse(meterReaing.BodySerial);//todo
            r.ZaribFasl = (decimal)(abBahaCalc?.HotSeasonAbBahaAmount ?? 0);
            r.Ab10 = 0;
            r.Ab20 = 0;
            r.TedadVahd = r.OtherUnit;
            r.TedKhane = r.HouseholdNumber;
            r.TedadMas = r.DomesticUnit;
            r.TedadTej = r.CommercialUnit;
            r.NoeVa = r.BranchTypeId;
            r.Jarime = 0;
            r.Masjar = 0;
            r.Sabt = 0;
            r.Rate = (decimal)(abBahaCalc?.MonthlyConsumption ?? 0);
            r.Operator = 0;//todo
            r.Mamor = 0;//todo
            r.TavizDate = "";//todo
            r.ZaribCntr = 0;
            r.Zabresani = 0;
            r.ZaribD = 0;
            r.Tafavot = 0;
            r.KasrHa = (decimal)(abBahaCalc?.DiscountSum ?? 0);
            r.FixMas = r.ContractualCapacity;
            r.ShGhabs1 = r.BillId;
            r.ShPard1 = "";//todo
            r.TabAbnA = 0;
            r.TabAbnF = 0;
            r.TabsFa = 0;
            r.NewAb = 0;
            r.NewFa = 0;
            r.Bodjeh = (decimal)(abBahaCalc?.SumBoodje ?? 0);
            r.Group1 = r.ConsumptionUsageId;
            r.MasFas = 0;
            r.Faz = (abBahaCalc?.FazelabAmount ?? 0) > 0;
            r.ChkKarbari = 0;
            r.C200 = 0;
            r.AbSevom = 0;
            r.AbSevom1 = 0;
            r.C70 = 0;
            r.C80 = 0;
            r.C90 = 0;
            r.C101 = 0;
            r.KhaliS = r.EmptyUnit;
            r.EdarehK = r.IsSpecial;
            r.Avarez = (decimal)(abBahaCalc?.AvarezAmount ?? 0);

            r.AbBahaDiscount = abBahaCalc?.AbBahaDiscount ?? 0;
            r.HotSeasonDiscount = abBahaCalc?.HotSeasonDiscount ?? 0;
            r.HotSeasonFazelabDiscount = abBahaCalc?.HotSeasonFazelabDiscount ?? 0;
            r.FazelabDiscount = abBahaCalc?.FazelabDiscount ?? 0;
            r.AbonmanAbDiscount = abBahaCalc?.AbonmanAbDiscount ?? 0;
            r.AbonmanFazelabDiscount = abBahaCalc?.AbonmanFazelabDiscount ?? 0;
            r.AvarezDiscount = abBahaCalc?.AvarezDiscount ?? 0;
            r.JavaniDiscount = abBahaCalc?.JavaniDiscount ?? 0;
            r.BoodjeDiscount = abBahaCalc?.BoodjeDiscount ?? 0;
            r.MaliatDiscount = abBahaCalc?.MaliatDiscount ?? 0;

            if (hasZeroValue)
            {
                r.SumItemsBeforeDiscount = 0;
                r.SumItems = 0;
                r.DiscountSum = 0;
                r.Consumption = 0;
                r.MonthlyConsumption = 0;
                r.CurrentCounterStateCode = 4;
            }
            else
            {
                r.SumItemsBeforeDiscount = abBahaCalc?.SumItemsBeforeDiscount ?? 0;
                r.SumItems = abBahaCalc?.SumItems ?? 0;
                r.DiscountSum = abBahaCalc?.DiscountSum ?? 0;
                r.Consumption = abBahaCalc?.Consumption ?? 0;
                r.MonthlyConsumption = abBahaCalc?.MonthlyConsumption ?? 0;
            }
            return r;
        }
        private (double, double, double) GetAmounts(double preDebt, double sumItems)
        {
            double jam = preDebt + sumItems;
            if (jam > _conditionPayableAmount)
            {
                long divideJam = (long)(jam / 1000);
                double payable = divideJam * 1000;
                double remained = sumItems - payable;
                return (sumItems, jam, payable);
            }
            else
            {
                return (sumItems, jam, 0);
            }
        }
        private MeterFlowCreateDto GetMeterFlowCreateDto(MeterFlowStepEnum step, string fileName, int zoneId, Guid userId, string description)
        {
            return new MeterFlowCreateDto()
            {
                MeterFlowStepId = step,
                FileName = fileName,
                ZoneId = zoneId,
                InsertByUserId = userId,
                InsertDateTime = DateTime.Now,
                Description = description
            };
        }
        private async Task Validate(MeterReadingNonReadInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private bool IsInException(Exception ex)
        {
            return ex is InvalidDateException ||
            ex is InvalidBillIdException ||
            ex is TariffDateException ||
            ex is InvalidBillCommandException ||
            ex is TariffCalcException;
        }
    }
}
