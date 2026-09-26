using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;
using Aban360.Common.Timing;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class CalculationConfirmationHandler : AbstractBaseConnection, ICalculationConfirmationHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IVariabService _variabService;
        private readonly ICounterStateQueryService _counterStateQueryService;
        private readonly IT51QueryService _t51QueryService;
        private readonly IT5QueryService _t5QueryService;
        private readonly IT7QueryService _t7QueryService;
        private readonly IT41QueryService _t41QueryService;
        private readonly IIdempotentOperationService _idempotentOperationService;
        static int[] _invalidCounterStateCode = { (int)CounterStateCodeEnum.Close, (int)CounterStateCodeEnum.Block, (int)CounterStateCodeEnum.NonRead };
        const int _paymentDeadline = 7;
        const int _maxPayIdLen = 13;
        const int _type = 1;
        const string _typeTitle = "قبض";
        const string _readingStateTitle = "دارای کد مامور";
        public CalculationConfirmationHandler(
            IHttpContextAccessor contextAccessor,
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterReadingDetailQueryService meterReadingDetailService,
            ICommonMemberQueryService commonMemberQueryService,
            IMeterFlowQueryService meterFlowQueryService,
            IBedBesQueryService bedBesQueryService,
            IVariabService variabService,
            ICounterStateQueryService counterStateQueryService,
            IT51QueryService t51QueryService,
            IT7QueryService t7QueryService,
            IT5QueryService t5QueryService,
            IT41QueryService t41QueryService,
            IIdempotentOperationService idempotentOperationService,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _counterStateQueryService = counterStateQueryService;
            _counterStateQueryService.NotNull(nameof(counterStateQueryService));

            _t51QueryService = t51QueryService;
            _t51QueryService.NotNull(nameof(t51QueryService));

            _t5QueryService = t5QueryService;
            _t5QueryService.NotNull(nameof(t5QueryService));

            _t7QueryService = t7QueryService;
            _t7QueryService.NotNull(nameof(t7QueryService));

            _t41QueryService = t41QueryService;
            _t41QueryService.NotNull(nameof(t41QueryService));

            _idempotentOperationService = idempotentOperationService;
            _idempotentOperationService.NotNull(nameof(idempotentOperationService));
        }

        public async Task<MeterReadingCheckedOutputDto> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            string operationKey = $"MeterFlowAmountConfirmed:{latestFlowId}";
            Guid lockToken = Guid.NewGuid();
            IdempotentOperationResultDto operation = await _idempotentOperationService.TryBegin(operationKey, lockToken);

            if (!operation.Acquired)
            {
                if (operation.Status == IdempotentOperationStatusEnum.Completed && !string.IsNullOrWhiteSpace(operation.ResponseJson))
                {
                    MeterReadingCheckedOutputDto? previousResult = JsonSerializer.Deserialize<MeterReadingCheckedOutputDto>(operation.ResponseJson);
                    if (previousResult is not null)
                    {
                        return previousResult;
                    }
                }

                throw new IdempotentOperationInProgressException("عملیات تایید مبلغ برای این جریان در حال انجام است.");
            }

            try
            {
                return await Execute(latestFlowId, appUser, operationKey, lockToken, cancellationToken);
            }
            catch
            {
                try
                {
                    await _idempotentOperationService.Fail(operationKey, lockToken);
                }
                catch
                {
                    // Preserve the original business exception.
                }
                throw;
            }
        }

        private async Task<MeterReadingCheckedOutputDto> Execute(int latestFlowId, IAppUser appUser, string operationKey, Guid lockToken, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, MeterFlowStepEnum.ConsumptionChecked, cancellationToken);

            int firstFlowId = await _meterFlowQueryService.GetFirstFlowId(latestFlowId);
            IEnumerable<MeterReadingDetailDataOutputDto> meterReadings = await _meterReadingDetailService.Get(firstFlowId, false);
            if (!meterReadings.Any())
            {
                throw new ReadingException(ExceptionLiterals.NotFoundMeterReadingDetail);
            }
           
            int zoneId = meterReadings.FirstOrDefault().ZoneId;
            IEnumerable<BedBesPreviousNumberAndDateOutputDto> previousBillsInfo = await GetPreviousBills(meterReadings, zoneId);
            var (finalWarning, invalidMeterReadingsExcludedList) = await GetDuplicateBills(meterReadings, previousBillsInfo, zoneId, appUser);
            IEnumerable<MeterReadingDetailDataOutputDto> allMeterReadingWithoutInvalids = meterReadings.Where(r => !invalidMeterReadingsExcludedList.Any(invalid => invalid.Id == r.Id)).ToList();

            var (bedBesBatch, kasrHaBatch) = await GetBedBesAndKasrHaDto(allMeterReadingWithoutInvalids, cancellationToken);
            if ((bedBesBatch?.Count() ?? 0) <= 0)
            {
                throw new ReadingException(ExceptionLiterals.NotFoundBillsToConfirm);
            }
            ICollection<BillInsertDto> billsBatch = await GetBillsInsertDto(bedBesBatch, kasrHaBatch);
            ICollection<MembersFazelabCountAndDebtAmountUpdateDto> memberDebtAmountBatch = bedBesBatch.Select(b => new MembersFazelabCountAndDebtAmountUpdateDto((int)b.Town, (int)b.Radif, b.ShGhabs1, _invalidCounterStateCode.Contains((int)b.CodVas) ? 0 : (long)b.Baha, b.TodayDate)).ToList();
            ICollection<ContorUpdateDto> contorsUpdateBatch = GetContorsUpdateDto(bedBesBatch, previousBillsInfo);
            string opLogText = string.Format(OpLogLiterals.GenerateBatchBillOpLog, billsBatch?.FirstOrDefault()?.ZoneTitle, bedBesBatch?.Count() ?? 0);
            return await ExceSql(bedBesBatch, kasrHaBatch, billsBatch, memberDebtAmountBatch, contorsUpdateBatch, zoneId, firstFlowId, latestFlowId, appUser, opLogText, operationKey, lockToken, finalWarning);
        }
        private async Task<(ICollection<BedBesCreateDto>, ICollection<KasrHaDto>)> GetBedBesAndKasrHaDto(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, CancellationToken cancellationToken)
        {
            ICollection<BedBesCreateDto> BedBesBatch = new List<BedBesCreateDto>();
            ICollection<KasrHaDto> kasrHaBatch = new List<KasrHaDto>();
            string currnetDateJalali = DateTime.Now.ToShortPersianDateString();
            string month = currnetDateJalali.Substring(5, 2);

            foreach (var mr in meterReadings)
            {
                BedBesCreateDto bedBes = await GetBedBes(mr, $"{CommonLiterals.WaterPayIdUniqueCode}{month}");
                BedBesBatch.Add(bedBes);

                if (mr.DiscountSum > 0)
                {
                    KasrHaDto kasrHa = GerKasrHa(mr, bedBes);
                    kasrHaBatch.Add(kasrHa);
                }
            }
            return (BedBesBatch, kasrHaBatch);
        }
        private async Task<MeterReadingCheckedOutputDto> ExceSql(ICollection<BedBesCreateDto> BedBesBatch, ICollection<KasrHaDto> kasrHaBatch, ICollection<BillInsertDto> billsBatch, ICollection<MembersFazelabCountAndDebtAmountUpdateDto> memberDebtAmountBatch, ICollection<ContorUpdateDto> contorsUpdateBatch, int zoneId, int firstFlowId, int latestFlowId, IAppUser appUser, string opLogText, string operationKey, Guid lockToken, string finalWarning)
        {
            string dbName = GetDbName(zoneId);
            //string dbName = "Atlas";
            MeterFlowGetDto meterFlow = await _meterFlowQueryService.Get(latestFlowId);
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.CalculationConfirmed,
                FirstFlowId = firstFlowId,
                ZoneId = meterFlow.ZoneId,
                FileName = meterFlow.FileName,
                FromReadingNumber = meterFlow.FromReadingNumber,
                ToReadingNumber = meterFlow.ToReadingNumber,
                PrimaryCount = meterFlow.PrimaryCount,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
                Description = meterFlow.Description
            };

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BedBesCommandService bedBesCreateService = new(connection, transaction);
                    KasrHaCommandService kasrHaCommandService = new(connection, transaction);
                    MeterFlowCommandService meterFlowCommandService = new(connection, transaction);
                    BillCommandService billCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ContorCommandService contorCommandService = new(connection, transaction);
                    WaterDebtCommandService waterDebtCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);


                    await bedBesCreateService.InsertByBulk(BedBesBatch, dbName);
                    if ((kasrHaBatch?.Count() ?? 0) > 0)
                    {
                        await kasrHaCommandService.InsertByBulk(kasrHaBatch, dbName);
                    }
                    await billCommandService.InsertByBulk(billsBatch);
                    await membersCommandService.UpdateBedbes(memberDebtAmountBatch, dbName);
                    await contorCommandService.Update(contorsUpdateBatch, dbName, false);
                    await waterDebtCommandService.UpdateAmount(memberDebtAmountBatch);
                    await opLogCommandService.Insert(opLogText, appUser);

                    await meterFlowCommandService.Update(meterFlowUpdate);
                    int newMeterFlowId = await meterFlowCommandService.Insert(newMeterFlow);
                    MeterReadingCheckedOutputDto result = GetResult(newMeterFlowId, finalWarning);
                    await _idempotentOperationService.Complete(operationKey, lockToken, JsonSerializer.Serialize(result), connection, transaction);

                    transaction.Commit();
                    return result;
                }
            }
        }
        private async Task<(string, ICollection<MeterReadingDetailExcludedDto>)> GetDuplicateBills(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, IEnumerable<BedBesPreviousNumberAndDateOutputDto> previousBillsInfo, int zoneId, IAppUser appUser)
        {
            int firstFlowId = meterReadings?.FirstOrDefault()?.FlowImportedId ?? 0;
            var (invalidDuplicateReadingToExcludeList, invalidDuplicateCount, invalidLessThan5DayCount) = await GetInvlaidDuplicateReading(meterReadings, previousBillsInfo, appUser);
            await ExcludeExecSql(invalidDuplicateReadingToExcludeList, firstFlowId);
            string warningInvalidDuplicateMessage = invalidDuplicateCount == 0 ? string.Empty : ExceptionLiterals.InvalidDuplicateGenerateBill(invalidDuplicateCount);
            string warningInvalidLessThan5DayMessage = invalidLessThan5DayCount == 0 ? string.Empty : ExceptionLiterals.InvalidToleranceGenerateBill(invalidLessThan5DayCount);
            string finalWarning = $"{warningInvalidDuplicateMessage} - {warningInvalidLessThan5DayMessage}";

            return (finalWarning, invalidDuplicateReadingToExcludeList);
        }
        private async Task<(ICollection<MeterReadingDetailExcludedDto>, int, int)> GetInvlaidDuplicateReading(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, IEnumerable<BedBesPreviousNumberAndDateOutputDto> previousBillsInfo, IAppUser appUser)
        {
            ICollection<MeterReadingDetailExcludedDto> invalidDuplicateMeterReadingToExcludeList = new List<MeterReadingDetailExcludedDto>();
            DateTime currentDateTime = DateTime.Now;
            int invalidDuplicateCount = 0;
            int invalidLessThan5DayCount = 0;
            foreach (var item in meterReadings)
            {
                BedBesPreviousNumberAndDateOutputDto? previousBills = previousBillsInfo.Where(m => m.CustomerNumber == item.CustomerNumber).FirstOrDefault();
                if (previousBills is null)
                {
                    throw new ReadingException(ExceptionLiterals.InvalidPreviousBillInfo(item.BillId));
                }
                if (item.PreviousDateJalali.CompareTo(previousBills.PreviousDateJalali) != 0)//previousBills.PreviousDateJalali: تا تاریخ قرائت قبلی
                {
                    MeterReadingDetailExcludedDto excludeDto = new(item.Id, appUser.UserId, currentDateTime, ExcludedCauseEnum.DuplicateBill, ReportLiterals.DuplicateBill);
                    invalidDuplicateMeterReadingToExcludeList.Add(excludeDto);
                    invalidDuplicateCount++;
                }
                DateTime previousRegisterDate = ConvertDate.JalaliToDateTime(previousBills.RegisterDateJalali);
                if (item.DateBed.CompareTo(previousRegisterDate.AddDays(5).ToShortPersianDateString()) < 0)//تاریخ صدور قبض
                {
                    MeterReadingDetailExcludedDto excludeDto = new(item.Id, appUser.UserId, currentDateTime, ExcludedCauseEnum.DuplicateBill5, ReportLiterals.DuplicateBill5);
                    invalidDuplicateMeterReadingToExcludeList.Add(excludeDto);
                    invalidLessThan5DayCount++;
                }
            }

            return (invalidDuplicateMeterReadingToExcludeList, invalidDuplicateCount, invalidLessThan5DayCount);
        }
        private async Task<IEnumerable<BedBesPreviousNumberAndDateOutputDto>> GetPreviousBills(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, int zoneId)
        {
            IEnumerable<BedBesPreviousNumberAndDateOutputDto> previousBillsInfo;
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    previousBillsInfo = await _bedBesQueryService.GetPreviousDateAndNumber(connection, transaction, zoneId, meterReadings.Select(m => m.CustomerNumber).ToList());
                }
            }
            return previousBillsInfo;
        }
        private async Task ExcludeExecSql(ICollection<MeterReadingDetailExcludedDto> invalidDupliateReading, int firstFlowId)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterReadingDetailCommandService meterReadingDetailCommandService = new(connection, transaction);
                    await meterReadingDetailCommandService.Exclude(invalidDupliateReading, firstFlowId);

                    transaction.Commit();
                }
            }
        }
        private async Task<BedBesCreateDto> GetBedBes(MeterReadingDetailDataOutputDto meterReading, string paymentIdOption)
        {
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(meterReading.ZoneId, meterReading.CustomerNumber));
            var (sumItems, jam, pard) = TransactionIdGenerator.GetAmounts(memberInfo.DebtAmount ?? 0, meterReading.SumItems ?? 0);
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();
            decimal barge = await _variabService.GetAndRenew(meterReading.ZoneId);
            string paymentId = jam < CommonLiterals.BedBesConditionPayableAmount ? string.Empty : TransactionIdGenerator.GeneratePaymentId((long)pard, meterReading.BillId, paymentIdOption);

            var s = new BedBesCreateDto() { };

            s.Town = meterReading.ZoneId;
            s.Radif = meterReading.CustomerNumber;
            s.Eshtrak = meterReading.ReadingNumber;
            s.Barge = barge;
            s.PriNo = meterReading.PreviousNumber;
            s.TodayNo = meterReading.CurrentNumber;
            s.PriDate = meterReading.PreviousDateJalali;
            s.TodayDate = meterReading.CurrentDateJalali;
            s.AbonFas = (decimal)meterReading.AbonFas;
            s.FasBaha = (decimal)meterReading.FasBaha;
            s.AbBaha = (decimal)meterReading.AbBaha;
            s.Ztadil = (decimal)meterReading.Ztadil;
            s.Masraf = (decimal)meterReading.Consumption;
            s.Shahrdari = (decimal)meterReading.Shahrdari;
            s.Modat = meterReading.Modat ?? 0;
            s.DateBed = currentDateJalali;
            s.JalaseNo = 0;//todo
            s.Mohlat = mohlatDateJalali;
            s.AbonAb = (decimal)meterReading.AbonAb;
            s.Baha = (decimal)sumItems;
            s.Pard = (decimal)pard;
            s.Jam = (decimal)jam;
            s.CodVas = meterReading.CurrentCounterStateCode;
            s.Ghabs = "1";
            s.Del = false;
            s.Type = "1";
            s.CodEnshab = meterReading.UsageId;
            s.Enshab = meterReading.MeterDiameterId;
            s.Elat = 0;
            s.Serial = 0;
            s.Ser = 0;
            s.ZaribFasl = (decimal)meterReading.ZaribFasl;
            s.Ab10 = 0;
            s.Ab20 = 0;
            s.TedadVahd = meterReading.OtherUnit;
            s.TedKhane = meterReading.HouseholdNumber;
            s.TedadMas = meterReading.DomesticUnit;
            s.TedadTej = meterReading.CommercialUnit;
            s.NoeVa = meterReading.BranchTypeId;
            s.Jarime = 0;
            s.Masjar = 0;
            s.Sabt = 1;
            s.Rate = (decimal)meterReading.MonthlyConsumption;
            s.Operator = 666;
            s.Mamor = meterReading.AgentCode;
            s.TavizDate = meterReading.TavizDateJalali ?? string.Empty;
            s.ZaribCntr = 0;
            s.Zabresani = 0;
            s.ZaribD = (decimal)meterReading.ZaribD;
            s.Tafavot = 0;
            s.KasrHa = (decimal)meterReading.DiscountSum;
            s.FixMas = meterReading.ContractualCapacity;
            s.ShGhabs1 = meterReading.BillId;
            s.ShPard1 = paymentId.Length <= _maxPayIdLen ? paymentId : string.Empty;
            s.TabAbnA = 0;
            s.TabAbnF = 0;
            s.TabsFa = 0;
            s.NewAb = 0;
            s.NewFa = 0;
            s.Bodjeh = (decimal)meterReading.Bodjeh;
            s.Group1 = meterReading.ConsumptionUsageId;
            s.MasFas = (decimal)meterReading.Consumption;
            s.Faz = false;
            s.ChkKarbari = (decimal)meterReading.ChkKarbari;
            s.C200 = 0;
            s.DateIns = currentDateJalali;
            s.AbSevom = 0;
            s.AbSevom1 = 0;
            s.C70 = 0;
            s.C80 = 0;
            s.TmpDateBed = "";
            s.TmpPriDate = "";
            s.TmpTodayDate = "";
            s.TmpMohlat = "";
            s.TmpTavizDate = "";
            s.C90 = 0;
            s.C101 = 0;
            s.KhaliS = meterReading.EmptyUnit;
            s.EdarehK = meterReading.IsSpecial;
            s.Tafa402 = 0;
            s.Avarez = (decimal)meterReading.Avarez;
            s.TrackNumber = string.IsNullOrWhiteSpace(paymentId) ? 0 : long.Parse(paymentId);//Todo
            return s;
        }
        private KasrHaDto GerKasrHa(MeterReadingDetailDataOutputDto meterReading, BedBesCreateDto bedBes)
        {
            return new KasrHaDto()
            {
                Town = meterReading.ZoneId,
                IdBedbes = 0,
                Radif = meterReading.CustomerNumber,
                CodEnshab = meterReading.UsageId,
                Barge = bedBes.Barge,
                PriDate = meterReading.PreviousDateJalali,
                TodayDate = meterReading.CurrentDateJalali,
                PriNo = meterReading.PreviousNumber,
                TodayNo = meterReading.CurrentNumber,
                Masraf = (decimal)meterReading.Consumption,
                AbBaha = (decimal)(meterReading?.AbBahaDiscount ?? 0),
                FasBaha = (decimal)(meterReading?.FazelabDiscount ?? 0) + (decimal)(meterReading?.HotSeasonFazelabDiscount ?? 0),
                AbonAb = (decimal)(meterReading?.AbonmanAbDiscount ?? 0),
                AbonFas = (decimal)(meterReading?.AbonmanFazelabDiscount ?? 0),
                TabAbnA = 0,
                TabAbnF = 0,
                Ab10 = 0,
                Shahrdari = (decimal)(meterReading?.MaliatDiscount ?? 0),
                Rate = (decimal)(meterReading?.MonthlyConsumption ?? 0),
                Baha = (decimal)(meterReading?.DiscountSum ?? 0),
                ShGhabs = meterReading.BillId,
                ShPard = bedBes.ShPard1,
                DateBed = bedBes.DateBed,
                TmpDateBed = "",
                TmpTodayDate = "",
                TedVahd = meterReading?.OtherUnit ?? 0,
                TedKhane = meterReading?.HouseholdNumber ?? 0,
                TedadMas = meterReading?.DomesticUnit ?? 0,
                TedadTej = meterReading?.CommercialUnit ?? 0,
                ZaribFasl = 0,
                NoeVa = meterReading?.BranchTypeId ?? 0,
                Bodjeh = (decimal)(meterReading?.BoodjeDiscount ?? 0),
            };
        }
        private async Task<ICollection<BillInsertDto>> GetBillsInsertDto(ICollection<BedBesCreateDto> bedBes, ICollection<KasrHaDto> kasrHa)
        {
            DateTime currentDate = DateTime.Now;
            IEnumerable<NumericDictionary> meterDiameterIds = await _t5QueryService.Get();
            NumericDictionary zoneInfo = await _t51QueryService.Get((int)(bedBes?.FirstOrDefault()?.Town ?? 0), true);
            IEnumerable<NumericDictionary> branchTypeIds = await _t7QueryService.Get();
            IEnumerable<NumericDictionary> usageIds = await _t41QueryService.Get();
            IEnumerable<CounterStateCodeDto> counterStateCodes = await _counterStateQueryService.Get();//todo: change Dto -> NumericDictionary


            ICollection<BillInsertDto> bills = new List<BillInsertDto>();
            foreach (var b in bedBes)
            {
                MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber((int)b.Town, (int)b.Radif));
                KasrHaDto? discountInfo = kasrHa.Where(k => k.Radif == b.Radif && k.Town == b.Town).FirstOrDefault();
                bool isVillageId = (int)b.Town > 140000;

                BillInsertDto newBill = new()
                {
                    ZoneId = (int)b.Town,
                    ZoneTitle = zoneInfo.Title,
                    UsageId = b.CodEnshab,
                    CustomerNumber = b.Radif,
                    BillId = b.ShGhabs1,
                    ReadingNumber = b.Eshtrak,
                    PreviousNumber = (int)b.PriNo,
                    NextNumber = (int)b.TodayNo,
                    PreviousDay = b.PriDate,
                    NextDay = b.TodayDate,
                    RegisterDay = b.DateBed,
                    RegisterDayGregorian = currentDate,
                    CounterStateTitle = counterStateCodes.Where(x => x.Id == b.CodVas).FirstOrDefault()?.Title ?? string.Empty,
                    UsageId2 = b.Group1,
                    UsageTitle = usageIds.Where(x => x.Id == b.CodEnshab).FirstOrDefault()?.Title ?? string.Empty,
                    UsageTitle2 = usageIds.Where(x => x.Id == b.Group1).FirstOrDefault()?.Title ?? string.Empty,
                    BranchType = branchTypeIds.Where(x => x.Id == b.NoeVa).FirstOrDefault()?.Title ?? string.Empty,
                    WaterDiameterId = b.Enshab,
                    WaterDiameterTitle = meterDiameterIds.Where(x => x.Id == b.Enshab).FirstOrDefault()?.Title ?? string.Empty,
                    Siphon100 = memberInfo.Siphon100,
                    Siphon125 = memberInfo.Siphon125,
                    Siphon150 = memberInfo.Siphon150,
                    Siphon200 = memberInfo.Siphon200,
                    Siphon5 = memberInfo.Siphon5,
                    Siphon6 = memberInfo.Siphon6,
                    Siphon7 = memberInfo.Siphon7,
                    Siphon8 = memberInfo.Siphon8,
                    ContractCapacity = b.FixMas,
                    DomesticCount = b.TedadMas,
                    CommercialCount = b.TedadTej,
                    OtherCount = b.TedadVahd,
                    EmptyCount = b.KhaliS,
                    Consumption = (int)b.Masraf,
                    Duration = (int)b.Modat,
                    ConsumptionAverage = (float)b.Rate,
                    Deadline = b.Mohlat,
                    Item1 = (long)b.AbBaha,
                    Item2 = (long)b.FasBaha,
                    Item3 = (long)b.AbonAb,
                    Item4 = (long)b.AbonFas,
                    Item5 = (long)b.Shahrdari,
                    Item6 = 0,
                    Item7 = 0,
                    Item8 = (long)b.Jarime,
                    Item9 = (long)b.Zabresani,
                    Item10 = (long)b.ZaribD,
                    Item11 = (long)b.ZaribFasl,
                    Item12 = (long)b.Ztadil,
                    Item13 = 0,
                    Item14 = 0,
                    Item15 = 0,
                    Item16 = (long)b.Bodjeh,
                    Item17 = 0,
                    Item18 = (long)b.Avarez,
                    SumItems = (long)b.Baha,
                    Payable = (long)b.Pard,
                    PreDebt = memberInfo.DebtAmount ?? 0,
                    TypeId = _typeTitle,
                    ItemOff1 = (long)(discountInfo?.AbBaha ?? 0),
                    ItemOff2 = (long)(discountInfo?.FasBaha ?? 0),
                    ItemOff3 = (long)(discountInfo?.AbonAb ?? 0),
                    ItemOff4 = (long)(discountInfo?.AbonFas ?? 0),
                    ItemOff5 = (long)(discountInfo?.Shahrdari ?? 0),
                    ItemOff6 = 0,
                    ItemOff7 = 0,
                    ItemOff8 = 0,
                    ItemOff9 = 0,
                    ItemOff10 = 0,
                    ItemOff11 = (long)(discountInfo?.ZaribFasl ?? 0),
                    ItemOff12 = 0,
                    ItemOff13 = 0,
                    ItemOff14 = 0,
                    ItemOff15 = 0,
                    ItemOff16 = (long)(discountInfo?.Bodjeh ?? 0),
                    ItemOff17 = 0,
                    ItemOff18 = 0,
                    IsFree = false,
                    VillageId = isVillageId ? b.Town.ToString() : string.Empty,
                    VillageName = isVillageId ? zoneInfo.Title : string.Empty,
                    ZoneId2 = b.Town.ToString(),
                    ReadingStateTitle = _readingStateTitle,
                    PayId = b.ShPard1,
                    CounterStateCode = (int)b.CodVas,
                    TypeCode = _type,
                    TypeTitle = _typeTitle,
                    ReturnCauseId = null,
                    ReturnCauseTitle = null,
                    BranchTypeId = (int)b.NoeVa,
                    IsSettlement = false,
                };

                bills.Add(newBill);
            }
            return bills;
        }
        private ICollection<ContorUpdateDto> GetContorsUpdateDto(ICollection<BedBesCreateDto> bedBesInfo, IEnumerable<BedBesPreviousNumberAndDateOutputDto> previousBillsInfo)
        {
            return bedBesInfo.Select(b =>
            {
                BedBesPreviousNumberAndDateOutputDto? priInfo = previousBillsInfo.Where(p => p.CustomerNumber == b.Radif).FirstOrDefault();
                ContorUpdateDto contorDto;
                if (priInfo is null)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidPreviousBillsDataToGenerateContro(b.ShGhabs1));
                }
                else
                {
                    contorDto = new()
                    {
                        ZoneId = (int)b.Town,
                        CustomerNumber = (int)b.Radif,
                        //منظور از CurrentDateJalali, CurrentNumber تاریخ و رقم آخرین قرائت است.
                        //بهتر است درآینده این 2 فیلد به PreviousDateJalali,PreviousNumber تغیر کنند.
                        CurrentDateJalali = IsInvalidCounterStateCode((int)b.CodVas) ? priInfo.PreviousDateJalali : b.TodayDate,
                        CurrentNumber = IsInvalidCounterStateCode((int)b.CodVas) ? priInfo.PreviousNumber : (int)b.TodayNo,
                        Consumption = IsInvalidCounterStateCode((int)b.CodVas) ? priInfo.Consumption : (int)b.Masraf,
                        ConsumptionAverage = IsInvalidCounterStateCode((int)b.CodVas) ? priInfo.ConsumptionAverage : (float)b.Rate,
                        PreviousCounterState = (int)b.CodVas,//این فیلد به OldVas نگاشت خواهد شد.
                    };
                    return contorDto;
                }
            })
            .ToList();

            bool IsInvalidCounterStateCode(int curretnCounterStateCode) => _invalidCounterStateCode.Contains(curretnCounterStateCode);
        }
        private MeterReadingCheckedOutputDto GetResult(int flowId, string finalMessage)
        {
            return new MeterReadingCheckedOutputDto(flowId, MeterFlowStepEnum.ClientNotification, string.Join(" . ", MessageLiterals.SuccessfullOperation, finalMessage));
        }
    }
}
