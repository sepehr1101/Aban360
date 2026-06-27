using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations;
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
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class CalculationConfirmationHandler : AbstractBaseConnection, ICalculationConfirmationHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IVariabService _variabService;
        private readonly ICounterStateQueryService _counterStateQueryService;
        private readonly IT51QueryService _t51QueryService;
        private readonly IT5QueryService _t5QueryService;
        private readonly IT7QueryService _t7QueryService;
        private readonly IT41QueryService _t41QueryService;
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
            IOldTariffEngine oldTariffEngine,
            IVariabService variabService,
            ICounterStateQueryService counterStateQueryService,
            IT51QueryService t51QueryService,
            IT7QueryService t7QueryService,
            IT5QueryService t5QueryService,
            IT41QueryService t41QueryService,
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

            _oldTariffEngine = oldTariffEngine;
            _oldTariffEngine.NotNull(nameof(oldTariffEngine));

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
        }

        public async Task<MeterReadingCheckedOutputDto> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, MeterFlowStepEnum.ConsumptionChecked, cancellationToken);

            int firstFlowId = await _meterFlowQueryService.GetFirstFlowId(latestFlowId);
            IEnumerable<MeterReadingDetailDataOutputDto> meterReadings = await _meterReadingDetailService.GetWithoutExcluded(firstFlowId);

            if (!meterReadings.Any())
            {
                throw new ReadingException(ExceptionLiterals.NotFoundMeterReadingDetail);
            }
            int zoneId = meterReadings.FirstOrDefault().ZoneId;
            var (bedBesBatch, kasrHaBatch) = await GetBedBesAndKasrHaDto(meterReadings, cancellationToken);

            var (warningMessageForDuplicateBills, duplicateBillIds) = await CheckDuplicateBill(bedBesBatch);
            ICollection<BedBesCreateDto> bedBesBatchWithoutDuplicate = bedBesBatch.Where(s => !duplicateBillIds.Contains(s.ShGhabs1)).ToList();
            ICollection<KasrHaDto> kasrhasBatchWithoutDuplicate = kasrHaBatch.Where(s => !duplicateBillIds.Contains(s.ShGhabs)).ToList();
            ICollection<BillInsertDto> billsBatch = await GetBillsInsertDto(bedBesBatchWithoutDuplicate, kasrHaBatch);
            ICollection<MembersDebtAmountUpdateDto> memberDebtAmountBatch = bedBesBatchWithoutDuplicate.Select(b => new MembersDebtAmountUpdateDto((int)b.Town, (int)b.Radif, b.ShGhabs1, (long)b.Pard)).ToList();
            ICollection<ContorUpdateDto> contorsUpcateBatch = GetContorsUpdateDto(bedBesBatchWithoutDuplicate);
            string opLogText = string.Format(OpLogLiterals.GenerateBatchBillOpLog, billsBatch?.FirstOrDefault()?.ZoneTitle, bedBesBatchWithoutDuplicate?.Count() ?? 0);
            int newMeterFlowId = await ExceSql(bedBesBatchWithoutDuplicate, kasrhasBatchWithoutDuplicate, billsBatch, memberDebtAmountBatch, contorsUpcateBatch, zoneId, latestFlowId, appUser, opLogText);

            return GetResult(newMeterFlowId, warningMessageForDuplicateBills);
        }
        private async Task<(string?, IEnumerable<string>)> CheckDuplicateBill(ICollection<BedBesCreateDto> input)
        {
            IEnumerable<string> duplicateBillIds = await _bedBesQueryService.GetDuplicateBill(input);
            string? billIds = string.Join(",", duplicateBillIds);
            string warningMessage = string.IsNullOrWhiteSpace(billIds) ? string.Empty : ExceptionLiterals.InvalidDuplicateGenerateBill(billIds);
            return (warningMessage, duplicateBillIds);
        }
        private async Task<(ICollection<BedBesCreateDto>, ICollection<KasrHaDto>)> GetBedBesAndKasrHaDto(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, CancellationToken cancellationToken)
        {
            ICollection<BedBesCreateDto> BedBesBatch = new List<BedBesCreateDto>();
            ICollection<KasrHaDto> kasrHaBatch = new List<KasrHaDto>();
            string currnetDateJalali = DateTime.Now.ToShortPersianDateString();
            string month = currnetDateJalali.Substring(5, 2);

            foreach (var mr in meterReadings)
            {
                BedBesCreateDto bedBes = await GetBedBes(mr, $"1{month}");
                BedBesBatch.Add(bedBes);

                if (mr.DiscountSum > 0)
                {
                    KasrHaDto kasrHa = GerKasrHa(mr, bedBes.ShPard1);
                    kasrHaBatch.Add(kasrHa);
                }
            }
            return (BedBesBatch, kasrHaBatch);
        }
        private async Task<int> ExceSql(ICollection<BedBesCreateDto> BedBesBatch, ICollection<KasrHaDto> kasrHaBatch, ICollection<BillInsertDto> billsBatch, ICollection<MembersDebtAmountUpdateDto> memberDebtAmountBatch, ICollection<ContorUpdateDto> contorsUpdateBatch, int zoneId, int latestFlowId, IAppUser appUser, string opLogText)
        {
            //string dbName = GetDbName(zoneId);
            string dbName = "Atlas";
            MeterFlowGetDto meterFlow = await _meterFlowQueryService.Get(latestFlowId);
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.CalculationConfirmed,
                ZoneId = meterFlow.ZoneId,
                FileName = meterFlow.FileName,
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
                    await kasrHaCommandService.InsertByBulk(kasrHaBatch, dbName);
                    await billCommandService.InsertByBulk(billsBatch);
                    await membersCommandService.UpdateBedbes(memberDebtAmountBatch, dbName);//todo:not found any record in atlas.members
                    await contorCommandService.Update(contorsUpdateBatch, dbName, false);
                    await waterDebtCommandService.UpdateAmount(memberDebtAmountBatch);
                    await opLogCommandService.Insert(opLogText, appUser);

                    await meterFlowCommandService.Update(meterFlowUpdate);
                    int newMeterFlowId = await meterFlowCommandService.Insert(newMeterFlow);

                    transaction.Commit();
                    return newMeterFlowId;
                }
            }
        }

        private async Task<BedBesCreateDto> GetBedBes(MeterReadingDetailDataOutputDto meterReading, string paymentIdOption)
        {
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(meterReading.ZoneId, meterReading.CustomerNumber));
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();
            decimal barge = await _variabService.GetAndRenew(meterReading.ZoneId);
            string paymentId = TransactionIdGenerator.GeneratePaymentId((long)meterReading.Pard, meterReading.BillId, paymentIdOption);

            return new BedBesCreateDto()
            {
                Town = meterReading.ZoneId,
                Radif = meterReading.CustomerNumber,
                Eshtrak = meterReading.ReadingNumber,
                Barge = barge,
                PriNo = meterReading.PreviousNumber,
                TodayNo = meterReading.CurrentNumber,
                PriDate = meterReading.PreviousDateJalali,
                TodayDate = meterReading.CurrentDateJalali,
                AbonFas = (decimal)meterReading.AbonFas,
                FasBaha = (decimal)meterReading.FasBaha,
                AbBaha = (decimal)meterReading.AbBaha,
                Ztadil = (decimal)meterReading.Ztadil,
                Masraf = (decimal)meterReading.Consumption,
                Shahrdari = (decimal)meterReading.Shahrdari,
                Modat = meterReading.Modat ?? 0,
                DateBed = currentDateJalali,
                JalaseNo = 0,//todo
                Mohlat = mohlatDateJalali,
                AbonAb = (decimal)meterReading.AbonAb,
                Baha = (decimal)meterReading.SumItems,
                Pard = ((long)(meterReading.SumItems.Value + memberInfo.DebtAmount) / 1000) * 1000,//bedehi gahbli+currentSumItems   => check
                Jam = (decimal)(meterReading.SumItems.Value + memberInfo.DebtAmount),//bedehi gahbli+currentSumItems  => check
                CodVas = meterReading.CurrentCounterStateCode,
                Ghabs = "1",
                Del = false,
                Type = "1",
                CodEnshab = meterReading.UsageId,
                Enshab = meterReading.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = (decimal)meterReading.ZaribFasl,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = meterReading.OtherUnit,
                TedKhane = meterReading.HouseholdNumber,
                TedadMas = meterReading.DomesticUnit,
                TedadTej = meterReading.CommercialUnit,
                NoeVa = meterReading.BranchTypeId,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,
                Rate = (decimal)meterReading.MonthlyConsumption,
                Operator = 0,
                Mamor = meterReading.AgentCode,
                TavizDate = meterReading.TavizDateJalali ?? string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = (decimal)meterReading.ZaribD,
                Tafavot = 0,
                KasrHa = (decimal)meterReading.DiscountSum,
                FixMas = meterReading.ContractualCapacity,
                ShGhabs1 = meterReading.BillId,
                ShPard1 = paymentId.Length <= _maxPayIdLen ? paymentId : string.Empty,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = (decimal)meterReading.Bodjeh,
                Group1 = meterReading.ConsumptionUsageId,
                MasFas = (decimal)meterReading.Consumption,
                Faz = false,
                ChkKarbari = (decimal)meterReading.ChkKarbari,
                C200 = 0,
                DateIns = currentDateJalali,
                AbSevom = 0,
                AbSevom1 = 0,
                C70 = 0,
                C80 = 0,
                TmpDateBed = "",
                TmpPriDate = "",
                TmpTodayDate = "",
                TmpMohlat = "",
                TmpTavizDate = "",
                C90 = 0,
                C101 = 0,
                KhaliS = meterReading.EmptyUnit,
                EdarehK = meterReading.IsSpecial,
                Tafa402 = 0,
                Avarez = (decimal)meterReading.Avarez,
                TrackNumber = long.Parse(paymentId)//Todo
            };
        }
        private KasrHaDto GerKasrHa(MeterReadingDetailDataOutputDto meterReading, string paymentId)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();

            return new KasrHaDto()
            {
                Town = meterReading.ZoneId,
                IdBedbes = 0,
                Radif = meterReading.CustomerNumber,
                CodEnshab = meterReading.UsageId,
                Barge = 0,
                PriDate = meterReading.PreviousDateJalali,
                TodayDate = meterReading.CurrentDateJalali,
                PriNo = meterReading.PreviousNumber,
                TodayNo = meterReading.CurrentNumber,
                Masraf = (decimal)meterReading.Consumption,
                AbBaha = (decimal)meterReading.AbBahaDiscount,
                FasBaha = (decimal)meterReading.FazelabDiscount + (decimal)meterReading.HotSeasonFazelabDiscount,
                AbonAb = (decimal)meterReading.AbonmanAbDiscount,
                AbonFas = (decimal)meterReading.AbonmanFazelabDiscount,
                TabAbnA = 0,
                TabAbnF = 0,
                Ab10 = 0,
                Shahrdari = (decimal)meterReading.MaliatDiscount,
                Rate = (decimal)meterReading.MonthlyConsumption,
                Baha = (decimal)meterReading.DiscountSum,
                ShGhabs = meterReading.BillId,
                ShPard = paymentId,//todo
                DateBed = currentDateJalali,
                TmpDateBed = "",
                TmpTodayDate = "",
                TedVahd = meterReading.OtherUnit,
                TedKhane = meterReading.HouseholdNumber,
                TedadMas = meterReading.DomesticUnit,
                TedadTej = meterReading.CommercialUnit,
                ZaribFasl = 0,
                NoeVa = meterReading.BranchTypeId,
                Bodjeh = (decimal)meterReading.BoodjeDiscount,
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
                    Item1 = (long)(b.AbBaha),
                    Item2 = (long)(b.FasBaha),
                    Item3 = (long)(b.AbonAb),
                    Item4 = (long)(b.AbonFas),
                    Item5 = (long)(b.Shahrdari),
                    Item6 = 0,
                    Item7 = 0,
                    Item8 = (long)(b.Jarime),
                    Item9 = (long)(b.Zabresani),
                    Item10 = (long)(b.ZaribD),
                    Item11 = (long)(b.ZaribFasl),
                    Item12 = (long)(b.Ztadil),
                    Item13 = 0,
                    Item14 = 0,
                    Item15 = 0,
                    Item16 = (long)(b.Bodjeh),
                    Item17 = 0,
                    Item18 = (long)(b.Avarez),
                    SumItems = (long)(b.Baha),
                    Payable = (long)(b.Pard),
                    PreDebt = (long)(memberInfo.DebtAmount ?? 0),
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
        private ICollection<ContorUpdateDto> GetContorsUpdateDto(ICollection<BedBesCreateDto> input)
        {
            return input.Select(b => new ContorUpdateDto()
            {
                ZoneId = (int)b.Town,
                CustomerNumber = (int)b.Radif,
                CurrentDateJalali = b.DateBed,
                CurrentNumber = (int)b.TodayNo,
                Consumption = (int)b.Masraf,
                ConsumptionAverage = (float)b.Rate,
                PreviousCounterState = (int)b.CodVas,
            })
            .ToList();
        }
        private async Task<int> CreateCalculationConfirmedFlow(IDbConnection connection, IDbTransaction transaction, int latestFlowId, IAppUser appUser)
        {
            MeterFlowCommandService meterFlowCommandService = new(connection, transaction);

            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            await meterFlowCommandService.Update(meterFlowUpdate);

            MeterFlowGetDto meterFlow = await _meterFlowQueryService.Get(latestFlowId);
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.CalculationConfirmed,
                ZoneId = meterFlow.ZoneId,
                FileName = meterFlow.FileName,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
                Description = meterFlow.Description
            };
            int newMeterFlowId = await meterFlowCommandService.Insert(newMeterFlow);
            return newMeterFlowId;
        }
        private MeterImaginaryInputDto GetMeterImaginary(MeterReadingDetailDataOutputDto readingDetail)
        {
            CustomerDetailInfoInputDto customerInfo = new()
            {
                ZoneId = readingDetail.ZoneId,
                Radif = readingDetail.CustomerNumber,
                BranchType = readingDetail.BranchTypeId,
                UsageId = readingDetail.UsageId,
                DomesticUnit = readingDetail.DomesticUnit,
                CommertialUnit = readingDetail.CommercialUnit,
                OtherUnit = readingDetail.OtherUnit,
                EmptyUnit = readingDetail.EmptyUnit,
                WaterInstallationDateJalali = readingDetail.WaterInstallationDateJalali,
                SewageInstallationDateJalali = readingDetail.SewageInstallationDateJalali,
                WaterRegisterDate = readingDetail.WaterRegisterDate,
                SewageRegisterDate = readingDetail.SewageRegisterDate,
                SewageCalcState = readingDetail.SewageCalcState,
                ContractualCapacity = readingDetail.ContractualCapacity,
                HouseholdDate = readingDetail.HouseholdDate,
                HouseholdNumber = readingDetail.HouseholdNumber,
                ReadingNumber = readingDetail.ReadingNumber,
                VillageId = readingDetail.VillageId,
                IsSpecial = readingDetail.IsSpecial,
                VirtualCategoryId = readingDetail.VirtualCategoryId,
                CounterStateCode = readingDetail.CurrentCounterStateCode,
            };
            MeterInfoByPreviousDataInputDto meterInfo = new()
            {
                BillId = readingDetail.BillId,
                PreviousDateJalali = readingDetail.PreviousDateJalali,
                PreviousNumber = readingDetail.PreviousNumber,
                CurrentDateJalali = readingDetail.CurrentDateJalali,
                CurrentMeterNumber = readingDetail.CurrentNumber,
                CounterStateCode = readingDetail.CurrentCounterStateCode
            };
            return new MeterImaginaryInputDto()
            {
                CustomerInfo = customerInfo,
                MeterPreviousData = meterInfo,
            };
        }
        private MeterReadingCheckedOutputDto GetResult(int flowId, string? warningDuplicateBills)
        {
            return new MeterReadingCheckedOutputDto(flowId, MeterFlowStepEnum.ClientNotification, string.Join(" . ", MessageLiterals.SuccessfullOperation, warningDuplicateBills));
        }
        private bool CounterStateValidation(int counterStateCode, int currentNumber, int previousNumber)
        {
            int[] invalidCounterStateCode = [4, /*6,*/ 7, 8, 9, 10];

            if (counterStateCode == 6 && previousNumber != currentNumber)
            {
                return false;
            }
            if (invalidCounterStateCode.Contains(counterStateCode))
            {
                return false;
            }
            else if ((counterStateCode == 3 || counterStateCode == 5) && currentNumber > previousNumber)
            {
                return false;
            }
            return true;
        }
    }
}
