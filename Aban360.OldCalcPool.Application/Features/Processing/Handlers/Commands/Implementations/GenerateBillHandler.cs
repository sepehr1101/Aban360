using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
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
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class GenerateBillHandler : AbstractBaseConnection, IGenerateBillHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IValidator<GenerateBillInputDto> _validator;
        private readonly IVariabService _variabService;
        static int[] _domesticUsage = { (int)UsageEnum.Domestic, (int)UsageEnum.DomesticCommercial };//todo: IsTrue?
        static int[] _allowedZeroMeterNumberCounterState = { (int)CounterStateCodeEnum.Close, (int)CounterStateCodeEnum.Block };
        const int _paymentDeadline = 7;
        const float _domesticMaltiplier = 0.7f;
        const int _payIdMaxChar = 13;
        public GenerateBillHandler(
            IHttpContextAccessor contextAccessor,
            ICustomerInfoService customerInfoService,
            IOldTariffEngine tariffEngine,
            ICommonMemberQueryService commonMemberQueryService,
            IBedBesQueryService bedBesQueryService,
            IConfiguration configuration,
            IValidator<GenerateBillInputDto> validator,
            IVariabService variabService)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _tariffEngine = tariffEngine;
            _tariffEngine.NotNull(nameof(tariffEngine));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));
        }

        public async Task<NewBillOutputDto> Handle(GenerateBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await GetZoneIdANdCustomerNumber(inputDto.BillId);
            CustomerLastInfoGetDto customerInfo = await _commonMemberQueryService.GetMembersBedBesTavizFlatInfo(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, true);
            await Validate(inputDto, zoneIdAndCustomerNumber, customerInfo);

            AbBahaCalculationDetails abBahaCalcResult = await GetAbBahaCalc(inputDto, customerInfo, cancellationToken);
            abBahaCalcResult.MeterInfo.CounterStateCode = inputDto.CounterStateCode ?? 0;
            NewBillOutputDto result = new()
            {
                AbBahaCalculationDetail = abBahaCalcResult,
                PreviousBillsInfo = await _bedBesQueryService.GetPreviousBillsInfo(zoneIdAndCustomerNumber),
                PreviousMeterChangeDateJalali = customerInfo?.ChangeDateJalali ?? string.Empty,
                PreviousMeterNumber = customerInfo?.LastMeterNumber ?? 0,
                PreviousReadingDateJalali = customerInfo?.LastMeterDateJalali ?? string.Empty
            };


            if (!inputDto.IsConfirm)
            {
                return result;
            }
            BedBesCreateDto bedBes = await GetBedBes(customerInfo, abBahaCalcResult, inputDto, zoneIdAndCustomerNumber, inputDto.CounterStateCode);
            KasrHaDto kasrHa = GerKasrHa(customerInfo, abBahaCalcResult, inputDto);
            ContorUpdateDto contorUpdate = GetControUpdateDto(customerInfo, bedBes, inputDto.CounterStateCode ?? 0);
            string logtext = string.Format(OpLogLiterals.GenerateBillOpLog, bedBes.ShGhabs1, bedBes.ShPard1, bedBes.Pard);

            await SqlCommands(zoneIdAndCustomerNumber, bedBes, kasrHa, contorUpdate, abBahaCalcResult, appUser, inputDto.CounterStateCode, logtext);

            return result;
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaCalc(GenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails abBahaCalcResult;
            if (IsAllowedZeroMeterNumber(inputDto.CounterStateCode))
            {
                abBahaCalcResult = GetAbBahaCalcWithZeroValues(inputDto, customerInfo);
            }
            else if (inputDto.CounterStateCode == (int)CounterStateCodeEnum.Change)
            {
                abBahaCalcResult = await GetChangeCounterStateData(inputDto, customerInfo, cancellationToken);
            }
            else if (inputDto.CounterStateCode == (int)CounterStateCodeEnum.Malfunction && inputDto.ConsumptionAverage.HasValue)
            {
                MeterDateInfoWithMonthlyConsumptionOutputDto tariffMeterInfoByConsumptionAverage = new()
                {
                    BillId = inputDto.BillId,
                    PreviousDateJalali = customerInfo?.LastMeterDateJalali ?? customerInfo.MeterInstallationDateJalali,
                    CurrentDateJalali = inputDto.CurrentDateJalali,
                    MonthlyAverageConsumption = (double)inputDto.ConsumptionAverage
                };
                abBahaCalcResult = await _tariffEngine.Handle(tariffMeterInfoByConsumptionAverage, cancellationToken);
                abBahaCalcResult.MeterInfo.CounterStateCode = inputDto.CounterStateCode;
            }
            else
            {
                MeterInfoByPreviousDataInputDto tariffMeterInfoByPreviousData = GetMeterInfoByPreviousData(customerInfo, inputDto);
                abBahaCalcResult = await _tariffEngine.Handle(tariffMeterInfoByPreviousData, cancellationToken);
            }
            return abBahaCalcResult;
        }
        private async Task<AbBahaCalculationDetails> GetChangeCounterStateData(GenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, CancellationToken cancellationToken)
        {
            if (inputDto.CounterStateCode == (int)CounterStateCodeEnum.Change && string.IsNullOrWhiteSpace(customerInfo?.ChangeDateJalali ?? string.Empty))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }
            else if (inputDto.CounterStateCode == (int)CounterStateCodeEnum.Change && customerInfo.ChangeDateJalali.CompareTo(inputDto.CurrentDateJalali) > 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }
            else if (inputDto.CounterStateCode == (int)CounterStateCodeEnum.Change && customerInfo.ChangeDateJalali.CompareTo(customerInfo.LastMeterDateJalali) < 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }
            int previousNumber = customerInfo.LastMeterNumber ?? 0;
            string previousDateJalali = customerInfo.LastMeterDateJalali;

            MeterInfoByPreviousDataInputDto meterByPreviousData = new()
            {
                BillId = inputDto.BillId,
                PreviousDateJalali = customerInfo.ChangeDateJalali,
                PreviousNumber = 0,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                CurrentMeterNumber = inputDto.MeterNumber,
                CounterStateCode = 0,
            };
            AbBahaCalculationDetails abBahaCalcTmp = await _tariffEngine.Handle(meterByPreviousData, cancellationToken);

            MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto()
            {
                BillId = inputDto.BillId,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                MonthlyAverageConsumption = abBahaCalcTmp.MonthlyConsumption,
                PreviousDateJalali = previousDateJalali,
            };
            AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterInfo, cancellationToken);
            return abBahaCalc;
        }
        private AbBahaCalculationDetails GetAbBahaCalcWithZeroValues(GenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            inputDto.MeterNumber = 0;
            string previousDate = customerInfo?.LastMeterDateJalali ?? customerInfo.MeterInstallationDateJalali;
            int previousNumber = customerInfo?.LastMeterNumber ?? 0;
            int finalUnit = GetFinalDomesticUnit(customerInfo, inputDto.CurrentDateJalali);
            ConsumptionInfo consumptionInfo = new(previousDate, inputDto.CurrentDateJalali, 0, GetDuration(previousDate, inputDto.CurrentDateJalali), 0, finalUnit);
            MeterInfoOutputDto meterInfo = new(previousDate, inputDto.CurrentDateJalali, previousNumber, 0, inputDto.CounterStateCode);

            CustomerDetailInfoInputDto customerDetailInfo = new()
            {
                ZoneId = customerInfo.ZoneId,
                Radif = customerInfo.CustomerNumber,
                BranchType = customerInfo.UseStateId,
                UsageId = customerInfo.UsageId,
                DomesticUnit = customerInfo.DomesticUnit,
                CommertialUnit = customerInfo.CommercialUnit,
                OtherUnit = customerInfo.OtherUnit,
                EmptyUnit = customerInfo.EmptyUnit,
                WaterInstallationDateJalali = customerInfo.MeterInstallationDateJalali,
                SewageInstallationDateJalali = customerInfo.SiphonInstallationDateJalali,
                WaterRegisterDate = customerInfo.MeterInstalltionRegisterDateJalali,
                SewageRegisterDate = customerInfo.SiphonInstalltionRegisterDateJalali,
                SewageCalcState = customerInfo.SewageCalcState,
                ContractualCapacity = customerInfo.ContractualCapacity,
                HouseholdNumber = customerInfo.HouseholdNumber,
                HouseholdDate = customerInfo.HouseholdDateJalali,
                ReadingNumber = customerInfo.ReadingNumber,
                VillageId = customerInfo.VillageId,
                IsSpecial = customerInfo.IsSpecial,
                VirtualCategoryId = customerInfo.VirtualCategoryId,
                CounterStateCode = inputDto.CounterStateCode,
            };
            MeterInfoByPreviousDataInputDto previousMeterInfo = new()
            {
                BillId = customerInfo.BillId,
                PreviousDateJalali = previousDate,
                PreviousNumber = previousNumber,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                CurrentMeterNumber = inputDto.MeterNumber,
                CounterStateCode = inputDto.CounterStateCode,
            };
            MeterImaginaryInputDto meterImaginaryDto = new() { CustomerInfo = customerDetailInfo, MeterPreviousData = previousMeterInfo };
            CustomerInfoOutputDto customerInfoOutputDto = new(meterImaginaryDto);
            stopWatch.Stop();


            AbBahaCalculationDetails abBahaCalcResult = new AbBahaCalculationDetails(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            new List<NerkhGetDto>(), new List<AbAzadFormulaDto>(), new List<ZaribGetDto>(),
            consumptionInfo, meterInfo, customerInfoOutputDto, stopWatch.ElapsedMilliseconds, 0);

            return abBahaCalcResult;
        }
        private int GetFinalDomesticUnit(CustomerLastInfoGetDto customerInfo, string readingDateJalali)
        {
            if (IsGardenAndResidence(customerInfo.UsageId))
            {
                return customerInfo.DomesticUnit < 1 ? 1 : customerInfo.DomesticUnit;//((/*customerInfo.OtherUnit + */customerInfo.DomesticUnit) == 0 ? 1 : /*customerInfo.OtherUnit + */ customerInfo.DomesticUnit);
            }
            int finalHousehold = GetHouseholdUnit(customerInfo.HouseholdNumber, customerInfo.HouseholdDateJalali, readingDateJalali);
            if (finalHousehold > 1)
            {
                return customerInfo.HouseholdNumber;//customerInfo.DomesticUnit;
            }
            return customerInfo.DomesticUnit - customerInfo.EmptyUnit < 1 ? 1 : customerInfo.DomesticUnit - customerInfo.EmptyUnit;
        }
        internal static bool IsGardenAndResidence(int usageId)
        {
            int[] s = [25, 34];
            return s.Contains(usageId);
        }
        private int GetHouseholdUnit(int householdUnit, string? householdDate, string readingDateJalali)
        {
            if (householdUnit <= 0)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(householdDate))
            {
                return 0;
            }
            DateTime? expireHouseHoldGregorian = householdDate.ToGregorianDateTime();
            if (!expireHouseHoldGregorian.HasValue)
            {
                return 0;
            }
            DateTime? readingDateGregorian = readingDateJalali.ToGregorianDateTime();
            if (!readingDateGregorian.HasValue)
            {
                throw new InvalidDateException(readingDateJalali);
            }
            if (readingDateGregorian.Value < expireHouseHoldGregorian.Value)//تاریخ قرائت قبل از تاریخ ثبت خانوار
            {
                return 0;
            }
            if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value)// تاریخ قرائت بعد از تاریخ انقضای خانوار
            {
                return 0;
            }
            return householdUnit;
        }
        private async Task SqlCommands(ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, BedBesCreateDto bedBes, KasrHaDto kasrHa, ContorUpdateDto contorUpdate, AbBahaCalculationDetails abBahaCalcResult, IAppUser appUser, int? counterStateCode, string logText)
        {
            string dbName = GetDbName(zoneIdAndCustomerNumber.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        BedBesCommandService bedBedCommandService = new(connection, transaction);
                        KasrHaCommandService kasrHasCommandService = new(connection, transaction);
                        MembersCommandService membersCommandService = new(connection, transaction);
                        WaterDebtCommandService waterDebtCommandService = new(connection, transaction);
                        BillCommandService billCommandService = new(connection, transaction);
                        ContorCommandService controCommandService = new(connection, transaction);
                        OpLogWithTransactionCommandService opLogcommandService = new(_contextAccessor, connection, transaction);

                        int bedBesRecordId = await bedBedCommandService.Insert(bedBes, dbName);
                        BillByBedBedIdInsertDto billInsertByBedBesIdDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, GetTypeId(counterStateCode), bedBesRecordId);
                        await billCommandService.InsertByBedBesId(billInsertByBedBesIdDto, dbName);
                        await opLogcommandService.Insert(logText, appUser);

                        if (abBahaCalcResult.DiscountSum > 0)
                        {
                            await kasrHasCommandService.Insert(kasrHa, dbName);
                        }
                        if (!IsAllowedZeroMeterNumber(counterStateCode))
                        {
                            await waterDebtCommandService.UpdateAmount(bedBes.ShGhabs1, (long)bedBes.Baha);
                            await membersCommandService.UpdateBedbes(zoneIdAndCustomerNumber, (long)bedBes.Baha, bedBes.TodayDate, dbName);
                            await controCommandService.Update(contorUpdate, dbName, true);                                                                                                   //update contro
                        }

                        transaction.Commit();
                    }
                    catch (Exception es)
                    {
                        transaction.Rollback();
                        throw es;
                    }
                }
            }
        }
        private async Task<ZoneIdAndCustomerNumber> GetZoneIdANdCustomerNumber(string billId)
        {
            ZoneIdAndCustomerNumber result = await _commonMemberQueryService.Get(billId);
            if (result.DeletionStateId == 1)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidDeletionState);
            }
            return result;
        }
        private ContorUpdateDto GetControUpdateDto(CustomerLastInfoGetDto customerInfo, BedBesCreateDto bedBes, int counterStateCode)
        {
            return new ContorUpdateDto()
            {
                ZoneId = customerInfo.ZoneId,
                CustomerNumber = customerInfo.CustomerNumber,
                CurrentDateJalali = bedBes.TodayDate,
                CurrentNumber = (int)bedBes.TodayNo,
                Consumption = (int)bedBes.Masraf,
                ConsumptionAverage = (float)bedBes.Rate,
                MeterChangeDateJalali = customerInfo?.ChangeDateJalali ?? string.Empty,
                MeterChangeNumber = customerInfo?.ChangeNumber ?? 0,
                PreviousCounterState = counterStateCode
            };
        }
        private MeterInfoByPreviousDataInputDto GetMeterInfoByPreviousData(CustomerLastInfoGetDto customerInfo, GenerateBillInputDto generateBillInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            if (!string.IsNullOrWhiteSpace(generateBillInfo.CurrentDateJalali))
            {
                currentDateJalali = generateBillInfo.CurrentDateJalali;
            }
            return new MeterInfoByPreviousDataInputDto()
            {
                BillId = customerInfo.BillId,
                PreviousDateJalali = customerInfo?.LastMeterDateJalali ?? customerInfo.MeterInstallationDateJalali,
                PreviousNumber = customerInfo?.LastMeterNumber ?? 0,
                CurrentDateJalali = currentDateJalali,
                CurrentMeterNumber = generateBillInfo.MeterNumber,
                CounterStateCode = generateBillInfo.CounterStateCode
            };
        }
        private async Task<BedBesCreateDto> GetBedBes(CustomerLastInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, GenerateBillInputDto generateBillInfo, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, int? counterSatetCode)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(zoneIdAndCustomerNumber);//checkResult: changeDto
            var (sumItems, jam, pard) = TransactionIdGenerator.GetAmounts(preDebtAmount, abBahaCalc.SumItems);
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();
            string paymentIdOption = $"{CommonLiterals.WaterPayIdUniqueCode}{currentDateJalali.Substring(5, 2)}";
            string paymentId = IsAllowedZeroMeterNumber(counterSatetCode) || jam < CommonLiterals.BedBesConditionPayableAmount ?
                string.Empty :
                TransactionIdGenerator.GeneratePaymentId((long)pard, abBahaCalc.Customer.BillId, paymentIdOption);
            if (paymentId.ToString().Length > _payIdMaxChar)
            {
                paymentId = string.Empty;
            }
            decimal barge = await _variabService.GetAndRenew(abBahaCalc.Customer.ZoneId);

            return new BedBesCreateDto()// ToDo :check
            {
                Town = customerInfo.ZoneId,
                Radif = customerInfo.CustomerNumber,
                Eshtrak = customerInfo.ReadingNumber,
                Barge = barge,
                PriNo = (decimal)customerInfo.LastMeterNumber,
                TodayNo = generateBillInfo.MeterNumber,
                PriDate = customerInfo.LastMeterDateJalali,
                TodayDate = string.IsNullOrWhiteSpace(generateBillInfo.CurrentDateJalali) ? currentDateJalali : generateBillInfo.CurrentDateJalali,
                AbonFas = (decimal)abBahaCalc.AbonmanFazelabAmount,
                FasBaha = (decimal)abBahaCalc.FazelabAmount + (decimal)abBahaCalc.HotSeasonFazelabAmount,
                AbBaha = (decimal)abBahaCalc.AbBahaAmount,
                Ztadil = 0,//
                Masraf = (decimal)abBahaCalc.Consumption,
                Shahrdari = (decimal)abBahaCalc.MaliatAmount,
                Modat = abBahaCalc.Duration,
                DateBed = currentDateJalali,
                JalaseNo = 0,
                Mohlat = mohlatDateJalali,
                Baha = (decimal)sumItems,
                AbonAb = (decimal)abBahaCalc.AbonmanAbAmount,
                Pard = (decimal)pard,
                Jam = (decimal)jam,
                CodVas = counterSatetCode ?? 0,
                Ghabs = "1",
                Del = false,
                Type = "1",
                CodEnshab = customerInfo.UsageId,
                Enshab = customerInfo.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = (decimal)abBahaCalc.HotSeasonAbBahaAmount,//
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = customerInfo.OtherUnit,
                TedKhane = customerInfo.HouseholdNumber,
                TedadMas = customerInfo.DomesticUnit,
                TedadTej = customerInfo.CommercialUnit,
                NoeVa = abBahaCalc.Customer.BranchType,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,//todo
                Rate = (decimal)abBahaCalc.MonthlyConsumption,
                Operator = ReportLiterals.RayabOperator,
                Mamor = 0,
                TavizDate = customerInfo?.ChangeDateJalali ?? string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = (decimal)abBahaCalc.JavaniAmount,
                Tafavot = 0,
                KasrHa = (decimal)abBahaCalc.DiscountSum,
                FixMas = customerInfo.ContractualCapacity,
                ShGhabs1 = customerInfo.BillId,
                ShPard1 = paymentId,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = (decimal)abBahaCalc.SumBoodje,
                Group1 = customerInfo.UsageConsumptionId,
                MasFas = GetSewageConsumption(abBahaCalc.Customer.UsageId, abBahaCalc.Consumption),
                Faz = customerInfo.SewageCalcState >= 1,
                ChkKarbari = 0,
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
                KhaliS = customerInfo.EmptyUnit,
                EdarehK = customerInfo.IsSpecial,
                Tafa402 = 0,
                Avarez = (decimal)abBahaCalc.AvarezAmount,
                TrackNumber = 0
            };
        }
        private decimal GetSewageConsumption(int usageId, double consumption) => IsDomestic(usageId) ? (decimal)(consumption * _domesticMaltiplier) : (decimal)consumption;
        private KasrHaDto GerKasrHa(CustomerLastInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, GenerateBillInputDto generateBillInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string paymentId = string.Empty;//TransactionIdGenerator.GeneratePaymentId((long)abBahaCalc.SumItems, abBahaCalc.Customer.BillId);

            return new KasrHaDto()
            {
                Town = customerInfo.ZoneId,
                IdBedbes = 0,
                Radif = customerInfo.CustomerNumber,
                CodEnshab = customerInfo.UsageId,
                Barge = 0,
                PriDate = customerInfo.LastMeterDateJalali,
                TodayDate = DateTime.Now.ToShortPersianDateString(),
                PriNo = (decimal)customerInfo.LastMeterNumber,
                TodayNo = generateBillInfo.MeterNumber,
                Masraf = (decimal)abBahaCalc.Consumption,
                AbBaha = (decimal)abBahaCalc.AbBahaDiscount,
                FasBaha = (decimal)abBahaCalc.FazelabDiscount + (decimal)abBahaCalc.HotSeasonFazelabDiscount,
                AbonAb = (decimal)abBahaCalc.AbonmanAbDiscount,
                AbonFas = (decimal)abBahaCalc.AbonmanFazelabDiscount,
                TabAbnA = 0,
                TabAbnF = 0,
                Ab10 = 0,
                Shahrdari = (decimal)abBahaCalc.MaliatDiscount,
                Rate = (decimal)abBahaCalc.MonthlyConsumption,
                Baha = (decimal)abBahaCalc.SumItems,
                ShGhabs = customerInfo.BillId,
                ShPard = paymentId,
                DateBed = currentDateJalali,
                TmpDateBed = "",
                TmpTodayDate = "",
                TedVahd = customerInfo.OtherUnit,
                TedKhane = customerInfo.HouseholdNumber,
                TedadMas = customerInfo.DomesticUnit,
                TedadTej = customerInfo.CommercialUnit,
                ZaribFasl = 0,
                NoeVa = abBahaCalc.Customer.BranchType,
                Bodjeh = (decimal)abBahaCalc.BoodjeDiscount,
            };
        }
        private async Task Validate(GenerateBillInputDto inputDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, CustomerLastInfoGetDto customerInfo)
        {
            await DeletionStateValidation(zoneIdAndCustomerNumber);
            CounterStateValidation(inputDto.CounterStateCode, inputDto.MeterNumber, customerInfo.LastMeterNumber);
        }
        private async Task InputValidate(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            if (!string.IsNullOrWhiteSpace(inputDto.CurrentDateJalali))
            {
                DateOnly? currentDate = inputDto.CurrentDateJalali.ToGregorianDateOnly();
                if (!currentDate.HasValue)
                {
                    var message = string.Join(',', "تاریخ ناصحیح");
                    throw new BaseException(message);
                }
                if (currentDate.Value > DateTime.Now.ToDateOnly())
                {
                    var message = string.Join(',', "تاریخ ناصحیح");
                    throw new BaseException(message);
                }
            }
        }
        private async Task DeletionStateValidation(ZoneIdAndCustomerNumber input)
        {
            MemberInfoGetDto customerInfo = await _commonMemberQueryService.Get(input);
            if (customerInfo.DeletionStateId == (int)DeletionStateEnum.HazfMovaghat)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidTemporaryDeletionState);
            }
            if (customerInfo.DeletionStateId == (int)DeletionStateEnum.JamAvari)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidCollectedDeletionState);
            }
        }
        private void CounterStateValidation(int? counterStateCode, int currentNumber, int? previousNumber)
        {
            if ((counterStateCode is null || !IsChangedOrReverse(counterStateCode)) &&
                (previousNumber.HasValue) &&
                !IsAllowedZeroMeterNumber(counterStateCode) &&
                (currentNumber < previousNumber))
            {
                throw new TariffCalcException(ExceptionLiterals.CurrentNumberLessThanPreviousNumber);
            }

        }
        private bool IsChangedOrReverse(int? counterStateCode) => counterStateCode == (int)CounterStateCodeEnum.Reverse || counterStateCode == (int)CounterStateCodeEnum.NextRound || counterStateCode == (int)CounterStateCodeEnum.Change;
        private bool IsDomestic(int usageId) => _domesticUsage.Contains(usageId);
        private bool IsAllowedZeroMeterNumber(int? counterStateCode) => _allowedZeroMeterNumberCounterState.Contains(counterStateCode ?? 0);
        private int GetDuration(string previousDate, string currentDate)
        {
            int thresholdDay = 1;
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            int duration = (currentGregorian.Value - previousGregorian.Value).Days;
            if (duration < thresholdDay)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidDuration);
            }
            return duration;
        }
        private int GetTypeId(int? counterStateCode)
        {
            if (_allowedZeroMeterNumberCounterState.Contains(counterStateCode ?? 0))
            {
                return 8;
            }
            return 1;//todo: set other 
        }

    }
}
