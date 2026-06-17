using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class FreeGenerateBillHandler : AbstractBaseConnection, IFreeGenerateBillHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IValidator<FreeGenerateBillInputDto> _validator;
        private readonly IVariabService _variabService;
        static int[] _domesticUsage = { 1, 3 };//todo: IsTrue?
        static int[] _allowedZeroMeterNumberCounterState = { 4, 7 };
        const int _paymentDeadline = 7;
        const int _conditionPayableAmount = 10000;
        const float _domesticMaltiplier = 0.7f;
        const int _collectedDeletionStateId = 1;
        const int _temporaryDeletionStateId = 5;
        const int _malfunctionCounterState = 1;
        const int _changeCounterState = 2;
        const int _reverseCounterState = 3;
        const int _nextRoundCounterSatate = 5;
        const int _operator = 666;
        const int _payIdMaxChar = 13;
        public FreeGenerateBillHandler(
            IHttpContextAccessor contextAccessor,
            ICustomerInfoService customerInfoService,
            IOldTariffEngine tariffEngine,
            ICommonMemberQueryService commonMemberQueryService,
            IBedBesQueryService bedBesQueryService,
            IConfiguration configuration,
            IValidator<FreeGenerateBillInputDto> validator,
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

        public async Task<NewBillOutputDto> Handle(FreeGenerateBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await GetZoneIdANdCustomerNumber(inputDto.BillId);
            CustomerInfoGetDto customerInfo = await _customerInfoService.Get(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            await Validate(inputDto, zoneIdAndCustomerNumber, customerInfo);

            AbBahaCalculationDetails abBahaCalcResult = await GetAbBahaCalc(inputDto, customerInfo, cancellationToken);
            abBahaCalcResult.MeterInfo.CounterStateCode = inputDto.CounterStateCode ?? 0;
            NewBillOutputDto result = new()
            {
                AbBahaCalculationDetail = abBahaCalcResult,
                PreviousBillsInfo = await _bedBesQueryService.GetPreviousBillsInfo(zoneIdAndCustomerNumber),
                PreviousMeterChangeDateJalali = customerInfo.TavizInfo?.TavizDateJalali ?? string.Empty,
                PreviousMeterNumber = inputDto.PreviousMeterNumber,
                PreviousReadingDateJalali = inputDto.PreviousDateJalali
            };


            if (!inputDto.IsConfirm)
            {
                return result;
            }
            BedBesCreateDto bedBes = await GetBedBes(customerInfo, abBahaCalcResult, inputDto, zoneIdAndCustomerNumber, inputDto.CounterStateCode);
            KasrHaDto kasrHa = GerKasrHa(customerInfo, abBahaCalcResult, inputDto);
            ContorUpdateDto contorUpdate = GetControUpdateDto(customerInfo, bedBes, inputDto.CounterStateCode ?? 0);
            string logtext = string.Format(OpLogLiterals.GenerateFreeBillOpLog, bedBes.ShGhabs1, bedBes.ShPard1, bedBes.Pard);

            await ExecSql(zoneIdAndCustomerNumber, bedBes, kasrHa, contorUpdate, abBahaCalcResult, appUser, inputDto.CounterStateCode, logtext);

            return result;
        }
        private async Task<AbBahaCalculationDetails> GetAbBahaCalc(FreeGenerateBillInputDto inputDto, CustomerInfoGetDto customerInfo, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails abBahaCalcResult;
            if (IsAllowedZeroMeterNumber(inputDto.CounterStateCode))
            {
                abBahaCalcResult = GetAbBahaCalcWithZeroValues(inputDto, customerInfo);
            }
            else if (inputDto.CounterStateCode == _changeCounterState)
            {
                abBahaCalcResult = await GetChangeCounterStateData(inputDto, customerInfo, cancellationToken);
            }
            else if (inputDto.CounterStateCode == _malfunctionCounterState && inputDto.ConsumptionAverage.HasValue)
            {
                MeterDateInfoWithMonthlyConsumptionOutputDto tariffMeterInfoByConsumptionAverage = new()
                {
                    BillId = inputDto.BillId,
                    PreviousDateJalali = customerInfo.BedBesInfo?.LastMeterDateJalali ?? customerInfo.MembersInfo.WaterInstallationDateJalali,
                    CurrentDateJalali = inputDto.CurrentDateJalali,
                    MonthlyAverageConsumption = (double)inputDto.ConsumptionAverage
                };
                abBahaCalcResult = await _tariffEngine.Handle(tariffMeterInfoByConsumptionAverage, cancellationToken);
                abBahaCalcResult.MeterInfo.PreviousNumber = inputDto.PreviousMeterNumber;
                abBahaCalcResult.MeterInfo.CurrentNumber = inputDto.CurrentMeterNumber;
                abBahaCalcResult.MeterInfo.CounterStateCode = inputDto.CounterStateCode;
            }
            else
            {
                MeterInfoByPreviousDataInputDto tariffMeterInfoByPreviousData = new()
                {
                    BillId = inputDto.BillId,
                    PreviousDateJalali = inputDto.PreviousDateJalali,
                    PreviousNumber = inputDto.PreviousMeterNumber,
                    CurrentDateJalali = inputDto.CurrentDateJalali,
                    CurrentMeterNumber = inputDto.CurrentMeterNumber,
                    CounterStateCode = inputDto.CounterStateCode
                };
                abBahaCalcResult = await _tariffEngine.Handle(tariffMeterInfoByPreviousData, cancellationToken);
            }
            return abBahaCalcResult;
        }
        private async Task<AbBahaCalculationDetails> GetChangeCounterStateData(FreeGenerateBillInputDto inputDto, CustomerInfoGetDto customerInfo, CancellationToken cancellationToken)
        {
            if (inputDto.CounterStateCode == _changeCounterState && string.IsNullOrWhiteSpace(customerInfo.TavizInfo?.TavizDateJalali ?? string.Empty))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }
            else if (inputDto.CounterStateCode == _changeCounterState && customerInfo.TavizInfo.TavizDateJalali.CompareTo(inputDto.CurrentDateJalali) > 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }
            else if (inputDto.CounterStateCode == _changeCounterState && customerInfo.TavizInfo.TavizDateJalali.CompareTo(customerInfo.BedBesInfo.LastMeterDateJalali) < 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }
            //else if (inputDto.CounterStateCode == _changeCounterState) //taviz
            //{

            MeterInfoByPreviousDataInputDto meterByPreviousData = new()
            {
                BillId = inputDto.BillId,
                PreviousDateJalali = customerInfo.TavizInfo.TavizDateJalali,
                PreviousNumber = 0,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                CurrentMeterNumber = inputDto.CurrentMeterNumber,
                CounterStateCode = 0,
            };
            AbBahaCalculationDetails abBahaCalcTmp = await _tariffEngine.Handle(meterByPreviousData, cancellationToken);

            MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto()
            {
                BillId = inputDto.BillId,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                MonthlyAverageConsumption = abBahaCalcTmp.MonthlyConsumption,
                PreviousDateJalali = inputDto.PreviousDateJalali,
            };
            AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterInfo, cancellationToken);
            abBahaCalc.MeterInfo.PreviousNumber = inputDto.PreviousMeterNumber;
            abBahaCalc.MeterInfo.CurrentNumber = inputDto.CurrentMeterNumber;
            return abBahaCalc;
        }
        private AbBahaCalculationDetails GetAbBahaCalcWithZeroValues(FreeGenerateBillInputDto inputDto, CustomerInfoGetDto customerInfo)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            inputDto.CurrentMeterNumber = 0;
            int finalUnit = GetFinalDomesticUnit(customerInfo, inputDto.CurrentDateJalali);
            ConsumptionInfo consumptionInfo = new(inputDto.PreviousDateJalali, inputDto.CurrentDateJalali, 0, GetDuration(inputDto.PreviousDateJalali, inputDto.CurrentDateJalali), 0, finalUnit);
            MeterInfoOutputDto meterInfo = new(inputDto.PreviousDateJalali, inputDto.CurrentDateJalali, inputDto.PreviousMeterNumber, 0, inputDto.CounterStateCode);

            CustomerDetailInfoInputDto customerDetailInfo = new()
            {
                ZoneId = customerInfo.MembersInfo.ZoneId,
                Radif = customerInfo.MembersInfo.CustomerNumber,
                BranchType = customerInfo.MembersInfo.BranchTypeId,
                UsageId = customerInfo.MembersInfo.UsageId,
                DomesticUnit = customerInfo.MembersInfo.DomesticUnit,
                CommertialUnit = customerInfo.MembersInfo.CommercialUnit,
                OtherUnit = customerInfo.MembersInfo.OtherUnit,
                EmptyUnit = customerInfo.MembersInfo.EmptyUnit,
                WaterInstallationDateJalali = customerInfo.MembersInfo.WaterInstallationDateJalali,
                SewageInstallationDateJalali = customerInfo.MembersInfo.SewageInstallationDateJalali,
                WaterRegisterDate = customerInfo.MembersInfo.WaterRegisterDate,
                SewageRegisterDate = customerInfo.MembersInfo.SewageRegisterDate,
                SewageCalcState = customerInfo.MembersInfo.SewageCalcState,
                ContractualCapacity = customerInfo.MembersInfo.ContractualCapacity,
                HouseholdNumber = customerInfo.MembersInfo.HouseholdNumber,
                HouseholdDate = customerInfo.MembersInfo.HouseholdDate,
                ReadingNumber = customerInfo.MembersInfo.ReadingNumber,
                VillageId = customerInfo.MembersInfo.VillageId,
                IsSpecial = customerInfo.MembersInfo.IsSpecial,
                VirtualCategoryId = customerInfo.MembersInfo.VirtualCategoryId,
                CounterStateCode = inputDto.CounterStateCode,
            };
            MeterInfoByPreviousDataInputDto previousMeterInfo = new()
            {
                BillId = customerInfo.MembersInfo.BillId,
                PreviousDateJalali = inputDto.PreviousDateJalali,
                PreviousNumber = inputDto.PreviousMeterNumber,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                CurrentMeterNumber = inputDto.CurrentMeterNumber,
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
        private int GetFinalDomesticUnit(CustomerInfoGetDto customerInfo, string readingDateJalali)
        {
            if (IsGardenAndResidence(customerInfo.MembersInfo.UsageId))
            {
                return customerInfo.MembersInfo.DomesticUnit < 1 ? 1 : customerInfo.MembersInfo.DomesticUnit;//((/*customerInfo.OtherUnit + */customerInfo.DomesticUnit) == 0 ? 1 : /*customerInfo.OtherUnit + */ customerInfo.DomesticUnit);
            }
            int finalHousehold = GetHouseholdUnit(customerInfo.MembersInfo.HouseholdNumber, customerInfo.MembersInfo.HouseholdDate, readingDateJalali);
            if (finalHousehold > 1)
            {
                return customerInfo.MembersInfo.HouseholdNumber;//customerInfo.DomesticUnit;
            }
            return customerInfo.MembersInfo.DomesticUnit - customerInfo.MembersInfo.EmptyUnit < 1 ? 1 : customerInfo.MembersInfo.DomesticUnit - customerInfo.MembersInfo.EmptyUnit;
        }
        private bool IsGardenAndResidence(int usageId)
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
            if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value)
            {
                return 0;
            }
            return householdUnit;
        }
        private async Task ExecSql(ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, BedBesCreateDto bedBes, KasrHaDto kasrHa, ContorUpdateDto contorUpdate, AbBahaCalculationDetails abBahaCalcResult, IAppUser appUser, int? counterStateCode, string logText)
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
                            await membersCommandService.UpdateBedbes(zoneIdAndCustomerNumber, (long)bedBes.Baha, dbName);
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
        private ContorUpdateDto GetControUpdateDto(CustomerInfoGetDto customerInfo, BedBesCreateDto bedBes, int counterStateCode)
        {
            return new ContorUpdateDto()
            {
                ZoneId = customerInfo.MembersInfo.ZoneId,
                CustomerNumber = customerInfo.MembersInfo.CustomerNumber,
                CurrentDateJalali = bedBes.TodayDate,
                CurrentNumber = (int)bedBes.TodayNo,
                Consumption = (int)bedBes.Masraf,
                ConsumptionAverage = (float)bedBes.Rate,
                MeterChangeDateJalali = customerInfo.TavizInfo?.TavizDateJalali ?? string.Empty,
                MeterChangeNumber = customerInfo.TavizInfo?.TavizNumber ?? 0,
                PreviousCounterState = counterStateCode
            };
        }
        private async Task<BedBesCreateDto> GetBedBes(CustomerInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, FreeGenerateBillInputDto generateBillInfo, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, int? counterSatetCode)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(zoneIdAndCustomerNumber);//checkResult: changeDto
            var (sumItems, jam, pard) = GetAmounts(preDebtAmount, abBahaCalc.SumItems);
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();
            string paymentIdOption = $"1{currentDateJalali.Substring(5, 2)}";
            string paymentId = IsAllowedZeroMeterNumber(counterSatetCode) ?
                string.Empty :
                TransactionIdGenerator.GeneratePaymentId((long)pard, abBahaCalc.Customer.BillId, paymentIdOption);
            if (paymentId.ToString().Length > _payIdMaxChar)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotSupportPaymentIdCharecters(pard));
            }
            decimal barge = await _variabService.GetAndRenew(abBahaCalc.Customer.ZoneId);

            return new BedBesCreateDto()// ToDo :check
            {
                Town = customerInfo.MembersInfo.ZoneId,
                Radif = customerInfo.MembersInfo.CustomerNumber,
                Eshtrak = customerInfo.MembersInfo.ReadingNumber,
                Barge = barge,
                PriNo = generateBillInfo.PreviousMeterNumber,
                TodayNo = generateBillInfo.CurrentMeterNumber,
                PriDate = generateBillInfo.PreviousDateJalali,
                TodayDate = generateBillInfo.CurrentDateJalali,
                AbonFas = (decimal)abBahaCalc.AbonmanFazelabAmount,
                FasBaha = (decimal)abBahaCalc.FazelabAmount + (decimal)abBahaCalc.HotSeasonFazelabAmount,
                AbBaha = (decimal)abBahaCalc.AbBahaAmount,
                Ztadil = 0,
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
                CodEnshab = customerInfo.MembersInfo.UsageId,
                Enshab = customerInfo.MembersInfo.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = (decimal)abBahaCalc.HotSeasonAbBahaAmount,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = customerInfo.MembersInfo.OtherUnit,
                TedKhane = customerInfo.MembersInfo.HouseholdNumber,
                TedadMas = customerInfo.MembersInfo.DomesticUnit,
                TedadTej = customerInfo.MembersInfo.CommercialUnit,
                NoeVa = abBahaCalc.Customer.BranchType,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,//todo
                Rate = (decimal)abBahaCalc.MonthlyConsumption,
                Operator = _operator,
                Mamor = 0,
                TavizDate = customerInfo?.TavizInfo?.TavizDateJalali ?? string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = (decimal)abBahaCalc.JavaniAmount,
                Tafavot = 0,
                KasrHa = (decimal)abBahaCalc.DiscountSum,
                FixMas = customerInfo.MembersInfo.ContractualCapacity,
                ShGhabs1 = customerInfo.MembersInfo.BillId,
                ShPard1 = paymentId,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = (decimal)abBahaCalc.SumBoodje,
                Group1 = customerInfo.MembersInfo.ConsumptionUsageId,
                MasFas = GetSewageConsumption(abBahaCalc.Customer.UsageId, abBahaCalc.Consumption),
                Faz = customerInfo.MembersInfo.SewageCalcState >= 1,
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
                KhaliS = customerInfo.MembersInfo.EmptyUnit,
                EdarehK = customerInfo.MembersInfo.IsSpecial,
                Tafa402 = 0,
                Avarez = (decimal)abBahaCalc.AvarezAmount,
                TrackNumber = 0
            };
        }
        private decimal GetSewageConsumption(int usageId, double consumption) => IsDomestic(usageId) ? (decimal)(consumption * _domesticMaltiplier) : (decimal)consumption;
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
        private KasrHaDto GerKasrHa(CustomerInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, FreeGenerateBillInputDto generateBillInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string paymentId = string.Empty;//TransactionIdGenerator.GeneratePaymentId((long)abBahaCalc.SumItems, abBahaCalc.Customer.BillId);

            return new KasrHaDto()
            {
                Town = customerInfo.MembersInfo.ZoneId,
                IdBedbes = 0,
                Radif = customerInfo.MembersInfo.CustomerNumber,
                CodEnshab = customerInfo.MembersInfo.UsageId,
                Barge = 0,
                PriDate = generateBillInfo.PreviousDateJalali,
                TodayDate = generateBillInfo.CurrentDateJalali,
                PriNo = generateBillInfo.PreviousMeterNumber,
                TodayNo = generateBillInfo.CurrentMeterNumber,
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
                ShGhabs = customerInfo.MembersInfo.BillId,
                ShPard = paymentId,
                DateBed = currentDateJalali,
                TmpDateBed = "",
                TmpTodayDate = "",
                TedVahd = customerInfo.MembersInfo.OtherUnit,
                TedKhane = customerInfo.MembersInfo.HouseholdNumber,
                TedadMas = customerInfo.MembersInfo.DomesticUnit,
                TedadTej = customerInfo.MembersInfo.CommercialUnit,
                ZaribFasl = 0,
                NoeVa = abBahaCalc.Customer.BranchType,
                Bodjeh = (decimal)abBahaCalc.BoodjeDiscount,
            };
        }
        private async Task Validate(FreeGenerateBillInputDto inputDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, CustomerInfoGetDto customerInfo)
        {
            await InputPreviousDataValidate(inputDto, zoneIdAndCustomerNumber);
            await DeletionStateValidation(zoneIdAndCustomerNumber);
            CounterStateValidation(inputDto);
        }
        private async Task InputValidate(FreeGenerateBillInputDto inputDto, CancellationToken cancellationToken)
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
                    var message = string.Join("تاریخ ناصحیح");
                    throw new BaseException(message);
                }
                if (currentDate.Value > DateTime.Now.ToDateOnly())
                {
                    var message = string.Join("تاریخ ناصحیح");
                    throw new BaseException(message);
                }
            }
        }
        private async Task DeletionStateValidation(ZoneIdAndCustomerNumber input)
        {
            MemberInfoGetDto customerInfo = await _commonMemberQueryService.Get(input);
            if (customerInfo.DeletionStateId == _temporaryDeletionStateId)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidTemporaryDeletionState);
            }
            if (customerInfo.DeletionStateId == _collectedDeletionStateId)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidCollectedDeletionState);
            }
        }
        private void CounterStateValidation(FreeGenerateBillInputDto inputDto)
        {
            if ((inputDto.CounterStateCode is null || !IsChangedOrReverse(inputDto.CounterStateCode)) &&
                !IsAllowedZeroMeterNumber(inputDto.CounterStateCode) &&
                (inputDto.CurrentMeterNumber < inputDto.PreviousMeterNumber))
            {
                throw new TariffCalcException(ExceptionLiterals.CurrentNumberLessThanPreviousNumber);
            }
        }

        private async Task InputPreviousDataValidate(FreeGenerateBillInputDto inputDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber)
        {
            BedBesPreviousNumberAndDateOutputDto previousInfo = await _bedBesQueryService.GetPreviousDateAndNumber(zoneIdAndCustomerNumber, inputDto.BillId);
            if (inputDto.PreviousDateJalali.CompareTo(previousInfo.PreviousDateJalali) != 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidPreviousDateJalali);
            }
            if (inputDto.PreviousMeterNumber != previousInfo.PreviousNumber)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidPreviousNumber);
            }
        }

        private bool IsChangedOrReverse(int? counterStateCode) => counterStateCode == _reverseCounterState || counterStateCode == _nextRoundCounterSatate || counterStateCode == _changeCounterState;
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
