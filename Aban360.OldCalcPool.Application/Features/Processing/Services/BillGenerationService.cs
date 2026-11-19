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
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Processing.Services
{
    public interface IBillGenerationService
    {
        Task<NewBillOutputDto> FreeGenerateBillAsync(FreeGenerateBillInputDto inputDto, string detailLogText, IAppUser appUser, CancellationToken cancellationToken);
        Task<NewBillOutputDto> GenerateBillAsync(GenerateBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task<BillIssueRemainedOutputDto> GenerateBillWithRemainedAsync(string billId, IAppUser appUser, CancellationToken cancellationToken);
        Task<NowGenerateBillOutputDto> NowGenerateBillAsync(NowGenerateBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }

    internal sealed class BillGenerationService : AbstractBaseConnection, IBillGenerationService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IValidator<FreeGenerateBillInputDto> _validator;
        private readonly IVariabService _variabService;
        static int[] _FreeGenerateBilldomesticUsage =
        {
            1,
            3
        }; //todo: IsTrue?
        static int[] _FreeGenerateBillinvalidCounterStateCode =
        {
            (int)CounterStateCodeEnum.Close,
            (int)CounterStateCodeEnum.Block,
            (int)CounterStateCodeEnum.NonRead
        };
        static int[] _FreeGenerateBillallowedZeroMeterNumberCounterState =
        {
            (int)CounterStateCodeEnum.Close,
            (int)CounterStateCodeEnum.Block
        };
        private int _FreeGenerateBillpaymentDeadline = 7;
        private float _FreeGenerateBilldomesticMaltiplier = 0.7f;
        private int _FreeGenerateBillcollectedDeletionStateId = 1;
        private int _FreeGenerateBilltemporaryDeletionStateId = 5;
        private int _FreeGenerateBillmalfunctionCounterState = (int)CounterStateCodeEnum.Malfunction;
        private int _FreeGenerateBillchangeCounterState = (int)CounterStateCodeEnum.Change;
        private int _FreeGenerateBillreverseCounterState = (int)CounterStateCodeEnum.Reverse;
        private int _FreeGenerateBillnextRoundCounterSatate = (int)CounterStateCodeEnum.NextRound;
        private int _FreeGenerateBillwithoutConsumptionMeterState = (int)CounterStateCodeEnum.WithoutConsumption;
        private int _FreeGenerateBillpayIdMaxChar = 13;
        private int _FreeGenerateBillfirstMeterNumber = 1;
        private readonly IValidator<GenerateBillInputDto> _billGenerationGenerateBillvalidator;
        static int[] _GenerateBilldomesticUsage =
        {
            (int)UsageEnum.Domestic,
            (int)UsageEnum.DomesticCommercial
        }; //todo: IsTrue?
        static int[] _GenerateBillallowedZeroMeterNumberCounterState =
        {
            (int)CounterStateCodeEnum.Close,
            (int)CounterStateCodeEnum.Block
        };
        const int _GenerateBillpaymentDeadline = 7;
        const float _GenerateBilldomesticMaltiplier = 0.7f;
        const int _GenerateBillpayIdMaxChar = 13;
        private readonly ICommonZoneService _commonZoneService;
        private readonly ISubscriptionEventQueryService _subscriptionEventQueryService;
        private readonly IOpLogCommandService _opLogCommandService;
        private const int _NowGenerateBillMaxConsecutiveSelfClaimPeriods = 3;
        private readonly IValidator<NowGenerateBillInputDto> _billGenerationNowGenerateBillvalidator;
        private readonly INowGenerateBillRequestService _requestService;
        private static string _NowGenerateBillxApiKey = ReportLiterals.XApiKey;
        private static string _NowGenerateBillnowGenerateBillXApiKey = ReportLiterals.NowGenerateBillXApiKey;
        private static float _NowGenerateBillinvalidGTConsumptionPercent = 1.3f;
        private static float _NowGenerateBillinvalidLTConsumptionPercent = 0.3f;
        private static int[] _NowGenerateBillinvalidDeletionStateCodes =
        {
            (int)DeletionStateEnum.JamAvari,
            (int)DeletionStateEnum.JabejaShode,
            (int)DeletionStateEnum.HazfMovaghat
        };
        private static int _NowGenerateBillinvalidGTAmount = 500_000_000;
        private static int _NowGenerateBillinvalidLTConsumption = 1;
        private static int _NowGenerateBillinvalidLTDistanceDay = 10;
        public BillGenerationService(IHttpContextAccessor contextAccessor, ICustomerInfoService customerInfoService, IOldTariffEngine tariffEngine, ICommonMemberQueryService commonMemberQueryService, IBedBesQueryService bedBesQueryService, IConfiguration configuration, IValidator<FreeGenerateBillInputDto> validator, IVariabService variabService, IValidator<GenerateBillInputDto> generateBillvalidator, ICommonZoneService commonZoneService, ISubscriptionEventQueryService subscriptionEventQueryService, IOpLogCommandService opLogCommandService, IValidator<NowGenerateBillInputDto> nowGenerateBillvalidator, INowGenerateBillRequestService requestService) : base(configuration)
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
            _billGenerationGenerateBillvalidator = generateBillvalidator;
            _billGenerationGenerateBillvalidator.NotNull(nameof(generateBillvalidator));
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
            _commonMemberQueryService.NotNull(nameof(bedBesQueryService));
            _subscriptionEventQueryService = subscriptionEventQueryService;
            _subscriptionEventQueryService.NotNull(nameof(subscriptionEventQueryService));
            _opLogCommandService = opLogCommandService;
            _opLogCommandService.NotNull(nameof(opLogCommandService));
            _bedBesQueryService.NotNull(nameof(commonMemberQueryService));
            _billGenerationNowGenerateBillvalidator = nowGenerateBillvalidator;
            _billGenerationNowGenerateBillvalidator.NotNull(nameof(nowGenerateBillvalidator));
            _requestService = requestService;
            _requestService.NotNull(nameof(requestService));
        }

        public async Task<NewBillOutputDto> FreeGenerateBillAsync(FreeGenerateBillInputDto inputDto, string detailLogText, IAppUser appUser, CancellationToken cancellationToken)
        {
            await FreeGenerateBillInputValidate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await FreeGenerateBillGetZoneIdANdCustomerNumber(inputDto.BillId);
            //CustomerInfoGetDto customerInfo = await _customerInfoService.Get(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            CustomerLastInfoGetDto customerInfo = await _commonMemberQueryService.GetMembersBedBesTavizFlatInfo(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, true);
            BedBesPreviousNumberAndDateOutputDto? previousInfo = await _bedBesQueryService.GetPreviousDateAndNumber(zoneIdAndCustomerNumber, inputDto.BillId, true);
            await FreeGenerateBillValidate(inputDto, zoneIdAndCustomerNumber, customerInfo, previousInfo);
            AbBahaCalculationDetails abBahaCalcResult = await FreeGenerateBillGetAbBahaCalc(inputDto, customerInfo, cancellationToken);
            abBahaCalcResult.MeterInfo.CounterStateCode = inputDto.CounterStateCode ?? 0;
            NewBillOutputDto result = new()
            {
                AbBahaCalculationDetail = abBahaCalcResult,
                PreviousBillsInfo = await _bedBesQueryService.GetPreviousBillsInfo(zoneIdAndCustomerNumber),
                PreviousMeterChangeDateJalali = customerInfo?.ChangeDateJalali ?? string.Empty,
                PreviousMeterNumber = inputDto.PreviousMeterNumber,
                PreviousReadingDateJalali = inputDto.PreviousDateJalali
            };
            if (!inputDto.IsConfirm)
            {
                return result;
            }

            BedBesCreateDto bedBes = await FreeGenerateBillGetBedBes(customerInfo, abBahaCalcResult, inputDto, zoneIdAndCustomerNumber, inputDto.CounterStateCode);
            KasrHaDto kasrHa = FreeGenerateBillGerKasrHa(customerInfo, abBahaCalcResult, inputDto, bedBes);
            ContorUpdateDto contorUpdate = FreeGenerateBillGetControUpdateDto(customerInfo, bedBes, inputDto.CounterStateCode ?? 0, previousInfo);
            string logtext = string.IsNullOrWhiteSpace(detailLogText) ? string.Format(OpLogLiterals.GenerateFreeBillOpLog, bedBes.ShGhabs1, bedBes.ShPard1, bedBes.Pard) : string.Join(" - ", detailLogText, string.Format(OpLogLiterals.GenerateFreeBillOpLog, bedBes.ShGhabs1, bedBes.ShPard1, bedBes.Pard));
            await FreeGenerateBillExecSql(zoneIdAndCustomerNumber, bedBes, kasrHa, contorUpdate, abBahaCalcResult, appUser, inputDto.CounterStateCode, logtext);
            return result;
        }

        private async Task<AbBahaCalculationDetails> FreeGenerateBillGetAbBahaCalc(FreeGenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails abBahaCalcResult;
            if (FreeGenerateBillIsAllowedZeroMeterNumber(inputDto.CounterStateCode))
            {
                abBahaCalcResult = FreeGenerateBillGetAbBahaCalcWithZeroValues(inputDto, customerInfo);
            }
            else if (inputDto.CounterStateCode == _FreeGenerateBillchangeCounterState)
            {
                abBahaCalcResult = await FreeGenerateBillGetChangeCounterStateData(inputDto, customerInfo, cancellationToken);
            }
            else if (inputDto.CounterStateCode == _FreeGenerateBillmalfunctionCounterState && inputDto.ConsumptionAverage.HasValue)
            {
                MeterDateInfoWithMonthlyConsumptionOutputDto tariffMeterInfoByConsumptionAverage = new()
                {
                    BillId = inputDto.BillId,
                    PreviousDateJalali = inputDto.PreviousDateJalali, //customerInfo.BedBesInfo?.LastMeterDateJalali ?? customerInfo.MembersInfo.WaterInstallationDateJalali,
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

            abBahaCalcResult.MonthlyConsumption = Math.Round(abBahaCalcResult.MonthlyConsumption, 2);
            abBahaCalcResult.DailyConsumption = Math.Round(abBahaCalcResult.DailyConsumption, 3);
            return abBahaCalcResult;
        }

        private async Task<AbBahaCalculationDetails> FreeGenerateBillGetChangeCounterStateData(FreeGenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, CancellationToken cancellationToken)
        {
            if (inputDto.CounterStateCode == _FreeGenerateBillchangeCounterState && string.IsNullOrWhiteSpace(customerInfo?.ChangeDateJalali ?? string.Empty))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }
            else if (inputDto.CounterStateCode == _FreeGenerateBillchangeCounterState && customerInfo.ChangeDateJalali.CompareTo(inputDto.CurrentDateJalali) > 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }
            else if (inputDto.CounterStateCode == _FreeGenerateBillchangeCounterState && customerInfo.ChangeDateJalali.CompareTo(customerInfo.LastMeterDateJalali) < 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidChangeDate);
            }

            //else if (inputDto.CounterStateCode == _changeCounterState) //taviz
            //{
            MeterInfoByPreviousDataInputDto meterByPreviousData = new()
            {
                BillId = inputDto.BillId,
                PreviousDateJalali = customerInfo.ChangeDateJalali,
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

        private AbBahaCalculationDetails FreeGenerateBillGetAbBahaCalcWithZeroValues(FreeGenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            inputDto.CurrentMeterNumber = 0;
            int finalUnit = FreeGenerateBillGetFinalDomesticUnit(customerInfo, inputDto.CurrentDateJalali);
            ConsumptionInfo consumptionInfo = new(inputDto.PreviousDateJalali, inputDto.CurrentDateJalali, 0, FreeGenerateBillGetDuration(inputDto.PreviousDateJalali, inputDto.CurrentDateJalali), 0, finalUnit);
            MeterInfoOutputDto meterInfo = new(inputDto.PreviousDateJalali, inputDto.CurrentDateJalali, inputDto.PreviousMeterNumber, 0, inputDto.CounterStateCode);
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
                PreviousDateJalali = inputDto.PreviousDateJalali,
                PreviousNumber = inputDto.PreviousMeterNumber,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                CurrentMeterNumber = inputDto.CurrentMeterNumber,
                CounterStateCode = inputDto.CounterStateCode,
            };
            MeterImaginaryInputDto meterImaginaryDto = new()
            {
                CustomerInfo = customerDetailInfo,
                MeterPreviousData = previousMeterInfo
            };
            CustomerInfoOutputDto customerInfoOutputDto = new(meterImaginaryDto);
            stopWatch.Stop();
            AbBahaCalculationDetails abBahaCalcResult = new AbBahaCalculationDetails(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<NerkhGetDto>(), new List<AbAzadFormulaDto>(), new List<ZaribGetDto>(), consumptionInfo, meterInfo, customerInfoOutputDto, stopWatch.ElapsedMilliseconds, 0);
            return abBahaCalcResult;
        }

        private int FreeGenerateBillGetFinalDomesticUnit(CustomerLastInfoGetDto customerInfo, string readingDateJalali)
        {
            if (FreeGenerateBillIsGardenAndResidence(customerInfo.UsageId))
            {
                return customerInfo.DomesticUnit < 1 ? 1 : customerInfo.DomesticUnit; //((/*customerInfo.OtherUnit + */customerInfo.DomesticUnit) == 0 ? 1 : /*customerInfo.OtherUnit + */ customerInfo.DomesticUnit);
            }

            int finalHousehold = FreeGenerateBillGetHouseholdUnit(customerInfo.HouseholdNumber, customerInfo.HouseholdDateJalali, readingDateJalali);
            if (finalHousehold > 1)
            {
                return customerInfo.HouseholdNumber; //customerInfo.DomesticUnit;
            }

            return customerInfo.DomesticUnit - customerInfo.EmptyUnit < 1 ? 1 : customerInfo.DomesticUnit - customerInfo.EmptyUnit;
        }

        private bool FreeGenerateBillIsGardenAndResidence(int usageId)
        {
            int[] s = [25, 34];
            return s.Contains(usageId);
        }

        private int FreeGenerateBillGetHouseholdUnit(int householdUnit, string? householdDate, string readingDateJalali)
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

            if (readingDateGregorian.Value < expireHouseHoldGregorian.Value) //تاریخ قرائت قبل از تاریخ ثبت خانوار
            {
                return 0;
            }

            if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value) // تاریخ قرائت بعد از تاریخ انقضای خانوار
            {
                return 0;
            }

            return householdUnit;
        }

        private async Task FreeGenerateBillExecSql(ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, BedBesCreateDto bedBes, KasrHaDto kasrHa, ContorUpdateDto contorUpdate, AbBahaCalculationDetails abBahaCalcResult, IAppUser appUser, int? counterStateCode, string logText)
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
                        BillByBedBedIdInsertDto billInsertByBedBesIdDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, FreeGenerateBillGetTypeId(counterStateCode), bedBesRecordId);
                        await billCommandService.InsertByBedBesId(billInsertByBedBesIdDto, dbName);
                        await opLogcommandService.Insert(logText, appUser);
                        if (abBahaCalcResult.DiscountSum > 0)
                        {
                            await kasrHasCommandService.Insert(kasrHa, dbName);
                        }

                        if (!FreeGenerateBillIsAllowedZeroMeterNumber(counterStateCode))
                        {
                            await waterDebtCommandService.UpdateAmount(bedBes.ShGhabs1, (long)bedBes.Baha);
                            await membersCommandService.UpdateBedbes(zoneIdAndCustomerNumber, (long)bedBes.Baha, bedBes.TodayDate, dbName);
                            await controCommandService.Update(contorUpdate, dbName, true); //update contro
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

        private async Task<ZoneIdAndCustomerNumber> FreeGenerateBillGetZoneIdANdCustomerNumber(string billId)
        {
            ZoneIdAndCustomerNumber result = await _commonMemberQueryService.Get(billId);
            if (result.DeletionStateId == 1)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidDeletionState);
            }

            return result;
        }

        private ContorUpdateDto FreeGenerateBillGetControUpdateDto(CustomerLastInfoGetDto customerInfo, BedBesCreateDto bedBes, int counterStateCode, BedBesPreviousNumberAndDateOutputDto? previousInfo)
        {
            ContorUpdateDto contorDto;
            if (previousInfo is null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidPreviousBillsDataToGenerateContro(customerInfo.BillId));
            }
            else
            {
                contorDto = new()
                {
                    ZoneId = customerInfo.ZoneId,
                    CustomerNumber = customerInfo.CustomerNumber,
                    CurrentDateJalali = IsInvalidCounterStateCode((int)bedBes.CodVas) ? previousInfo.PreviousDateJalali : bedBes.TodayDate,
                    CurrentNumber = IsInvalidCounterStateCode((int)bedBes.CodVas) ? previousInfo.PreviousNumber : (int)bedBes.TodayNo,
                    Consumption = IsInvalidCounterStateCode((int)bedBes.CodVas) ? previousInfo.Consumption : (int)bedBes.Masraf,
                    ConsumptionAverage = IsInvalidCounterStateCode((int)bedBes.CodVas) ? previousInfo.ConsumptionAverage : (float)bedBes.Rate,
                    MeterChangeDateJalali = customerInfo?.ChangeDateJalali ?? string.Empty,
                    MeterChangeNumber = customerInfo?.ChangeNumber ?? 0,
                    PreviousCounterState = counterStateCode
                };
                return contorDto;
            }

            bool IsInvalidCounterStateCode(int curretnCounterStateCode) => _FreeGenerateBillinvalidCounterStateCode.Contains(curretnCounterStateCode);
        }

        private async Task<BedBesCreateDto> FreeGenerateBillGetBedBes(CustomerLastInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, FreeGenerateBillInputDto generateBillInfo, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, int? counterSatetCode)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(zoneIdAndCustomerNumber); //checkResult: changeDto
            var(sumItems, jam, pard) = TransactionIdGenerator.GetAmounts(preDebtAmount, abBahaCalc.SumItems);
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_FreeGenerateBillpaymentDeadline).ToShortPersianDateString();
            string paymentIdOption = $"{CommonLiterals.WaterPayIdUniqueCode}{currentDateJalali.Substring(5, 2)}";
            string paymentId = FreeGenerateBillIsAllowedZeroMeterNumber(counterSatetCode) || jam < CommonLiterals.BedBesConditionPayableAmount ? string.Empty : TransactionIdGenerator.GeneratePaymentId((long)pard, abBahaCalc.Customer.BillId, paymentIdOption);
            if (paymentId.ToString().Length > _FreeGenerateBillpayIdMaxChar)
            {
                paymentId = string.Empty;
            }

            decimal barge = await _variabService.GetAndRenew(abBahaCalc.Customer.ZoneId);
            return new BedBesCreateDto() // ToDo :check
            {
                Town = customerInfo.ZoneId,
                Radif = customerInfo.CustomerNumber,
                Eshtrak = customerInfo.ReadingNumber,
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
                CodEnshab = customerInfo.UsageId,
                Enshab = customerInfo.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = (decimal)abBahaCalc.HotSeasonAbBahaAmount,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = customerInfo.OtherUnit,
                TedKhane = customerInfo.HouseholdNumber,
                TedadMas = customerInfo.DomesticUnit,
                TedadTej = customerInfo.CommercialUnit,
                NoeVa = abBahaCalc.Customer.BranchType,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1, //todo
                Rate = (decimal)abBahaCalc.MonthlyConsumption,
                Operator = ReportLiterals.RayabOperator,
                Mamor = (decimal)generateBillInfo.OperatorCode,
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
                MasFas = FreeGenerateBillGetSewageConsumption(abBahaCalc.Customer.UsageId, abBahaCalc.Consumption),
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

        private decimal FreeGenerateBillGetSewageConsumption(int usageId, double consumption) => FreeGenerateBillIsDomestic(usageId) ? (decimal)(consumption * _FreeGenerateBilldomesticMaltiplier) : (decimal)consumption;
        private KasrHaDto FreeGenerateBillGerKasrHa(CustomerLastInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, FreeGenerateBillInputDto generateBillInfo, BedBesCreateDto bedBes)
        {
            return new KasrHaDto()
            {
                Town = customerInfo.ZoneId,
                IdBedbes = 0,
                Radif = customerInfo.CustomerNumber,
                CodEnshab = customerInfo.UsageId,
                Barge = bedBes.Barge,
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
                ShGhabs = customerInfo.BillId,
                ShPard = bedBes.ShPard1,
                DateBed = bedBes.DateBed,
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

        private async Task FreeGenerateBillValidate(FreeGenerateBillInputDto inputDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, CustomerLastInfoGetDto customerInfo, BedBesPreviousNumberAndDateOutputDto? previousInfo)
        {
            await FreeGenerateBillInputPreviousDataValidate(inputDto, customerInfo, previousInfo);
            await FreeGenerateBillDeletionStateValidation(zoneIdAndCustomerNumber);
            FreeGenerateBillCounterStateValidation(inputDto);
        }

        private async Task FreeGenerateBillInputValidate(FreeGenerateBillInputDto inputDto, CancellationToken cancellationToken)
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
                    var message = string.Join(",", "تاریخ ناصحیح");
                    throw new InvalidBillCommandException(message);
                }

                if (currentDate.Value > DateTime.Now.ToDateOnly())
                {
                    var message = string.Join(",", "تاریخ ناصحیح");
                    throw new InvalidBillCommandException(message);
                }

                if (currentDate.Value < DateTime.Now.AddMonths(-5).ToDateOnly())
                {
                    var message = string.Join(",", " تاریخ قرائت نباید بیش از 5 روز پیش باشد.");
                    throw new InvalidBillCommandException(message);
                }
            }
        }

        private async Task FreeGenerateBillDeletionStateValidation(ZoneIdAndCustomerNumber input)
        {
            MemberInfoGetDto customerInfo = await _commonMemberQueryService.Get(input);
            if (customerInfo.DeletionStateId == _FreeGenerateBilltemporaryDeletionStateId)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidTemporaryDeletionState);
            }

            if (customerInfo.DeletionStateId == _FreeGenerateBillcollectedDeletionStateId)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidCollectedDeletionState);
            }
        }

        private void FreeGenerateBillCounterStateValidation(FreeGenerateBillInputDto inputDto)
        {
            if ((inputDto.CounterStateCode is null || !FreeGenerateBillIsChangedOrReverse(inputDto.CounterStateCode)) && !FreeGenerateBillIsAllowedZeroMeterNumber(inputDto.CounterStateCode) && (inputDto.CurrentMeterNumber < inputDto.PreviousMeterNumber))
            {
                throw new TariffCalcException(ExceptionLiterals.CurrentNumberLessThanPreviousNumber);
            }

            if (inputDto.CounterStateCode == _FreeGenerateBillmalfunctionCounterState && inputDto.ConsumptionAverage is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidMonthlyAverageWithMalfunctionState);
            }

            if (inputDto.CounterStateCode == _FreeGenerateBillwithoutConsumptionMeterState && inputDto.PreviousMeterNumber != inputDto.CurrentMeterNumber)
            {
                throw new ReadingException(ExceptionLiterals.InvalidNotEqualMeterNumberInWithoutConsumption);
            }
        }

        private async Task FreeGenerateBillInputPreviousDataValidate(FreeGenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, BedBesPreviousNumberAndDateOutputDto? previousInfo)
        {
            string previousDateExpected = previousInfo is not null ? previousInfo.PreviousDateJalali : customerInfo.MeterInstallationDateJalali;
            int previousMeterNumberExpected = previousInfo is not null ? previousInfo.PreviousNumber : _FreeGenerateBillfirstMeterNumber;
            if (inputDto.PreviousDateJalali.CompareTo(previousDateExpected) != 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidPreviousDateJalali);
            }

            if (inputDto.PreviousMeterNumber != previousMeterNumberExpected)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidPreviousNumber);
            }
        }

        private bool FreeGenerateBillIsChangedOrReverse(int? counterStateCode) => counterStateCode == _FreeGenerateBillreverseCounterState || counterStateCode == _FreeGenerateBillnextRoundCounterSatate || counterStateCode == _FreeGenerateBillchangeCounterState;
        private bool FreeGenerateBillIsDomestic(int usageId) => _FreeGenerateBilldomesticUsage.Contains(usageId);
        private bool FreeGenerateBillIsAllowedZeroMeterNumber(int? counterStateCode) => _FreeGenerateBillallowedZeroMeterNumberCounterState.Contains(counterStateCode ?? 0);
        private int FreeGenerateBillGetDuration(string previousDate, string currentDate)
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

        private int FreeGenerateBillGetTypeId(int? counterStateCode)
        {
            if (_FreeGenerateBillallowedZeroMeterNumberCounterState.Contains(counterStateCode ?? 0))
            {
                return 8;
            }

            return 1; //todo: set other 
        }

        public async Task<NewBillOutputDto> GenerateBillAsync(GenerateBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await GenerateBillInputValidate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await GenerateBillGetZoneIdANdCustomerNumber(inputDto.BillId);
            CustomerLastInfoGetDto customerInfo = await _commonMemberQueryService.GetMembersBedBesTavizFlatInfo(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, true);
            await GenerateBillValidate(inputDto, zoneIdAndCustomerNumber, customerInfo);
            AbBahaCalculationDetails abBahaCalcResult = await GenerateBillGetAbBahaCalc(inputDto, customerInfo, cancellationToken);
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

            BedBesCreateDto bedBes = await GenerateBillGetBedBes(customerInfo, abBahaCalcResult, inputDto, zoneIdAndCustomerNumber, inputDto.CounterStateCode);
            KasrHaDto kasrHa = GenerateBillGerKasrHa(customerInfo, abBahaCalcResult, inputDto);
            ContorUpdateDto contorUpdate = GenerateBillGetControUpdateDto(customerInfo, bedBes, inputDto.CounterStateCode ?? 0);
            string logtext = string.Format(OpLogLiterals.GenerateBillOpLog, bedBes.ShGhabs1, bedBes.ShPard1, bedBes.Pard);
            await GenerateBillSqlCommands(zoneIdAndCustomerNumber, bedBes, kasrHa, contorUpdate, abBahaCalcResult, appUser, inputDto.CounterStateCode, logtext);
            return result;
        }

        private async Task<AbBahaCalculationDetails> GenerateBillGetAbBahaCalc(GenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails abBahaCalcResult;
            if (GenerateBillIsAllowedZeroMeterNumber(inputDto.CounterStateCode))
            {
                abBahaCalcResult = GenerateBillGetAbBahaCalcWithZeroValues(inputDto, customerInfo);
            }
            else if (inputDto.CounterStateCode == (int)CounterStateCodeEnum.Change)
            {
                abBahaCalcResult = await GenerateBillGetChangeCounterStateData(inputDto, customerInfo, cancellationToken);
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
                MeterInfoByPreviousDataInputDto tariffMeterInfoByPreviousData = GenerateBillGetMeterInfoByPreviousData(customerInfo, inputDto);
                abBahaCalcResult = await _tariffEngine.Handle(tariffMeterInfoByPreviousData, cancellationToken);
            }

            return abBahaCalcResult;
        }

        private async Task<AbBahaCalculationDetails> GenerateBillGetChangeCounterStateData(GenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, CancellationToken cancellationToken)
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

        private AbBahaCalculationDetails GenerateBillGetAbBahaCalcWithZeroValues(GenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            inputDto.MeterNumber = 0;
            string previousDate = customerInfo?.LastMeterDateJalali ?? customerInfo.MeterInstallationDateJalali;
            int previousNumber = customerInfo?.LastMeterNumber ?? 0;
            int finalUnit = GenerateBillGetFinalDomesticUnit(customerInfo, inputDto.CurrentDateJalali);
            ConsumptionInfo consumptionInfo = new(previousDate, inputDto.CurrentDateJalali, 0, GenerateBillGetDuration(previousDate, inputDto.CurrentDateJalali), 0, finalUnit);
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
            MeterImaginaryInputDto meterImaginaryDto = new()
            {
                CustomerInfo = customerDetailInfo,
                MeterPreviousData = previousMeterInfo
            };
            CustomerInfoOutputDto customerInfoOutputDto = new(meterImaginaryDto);
            stopWatch.Stop();
            AbBahaCalculationDetails abBahaCalcResult = new AbBahaCalculationDetails(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<NerkhGetDto>(), new List<AbAzadFormulaDto>(), new List<ZaribGetDto>(), consumptionInfo, meterInfo, customerInfoOutputDto, stopWatch.ElapsedMilliseconds, 0);
            return abBahaCalcResult;
        }

        private int GenerateBillGetFinalDomesticUnit(CustomerLastInfoGetDto customerInfo, string readingDateJalali)
        {
            if (GenerateBillIsGardenAndResidence(customerInfo.UsageId))
            {
                return customerInfo.DomesticUnit < 1 ? 1 : customerInfo.DomesticUnit; //((/*customerInfo.OtherUnit + */customerInfo.DomesticUnit) == 0 ? 1 : /*customerInfo.OtherUnit + */ customerInfo.DomesticUnit);
            }

            int finalHousehold = GenerateBillGetHouseholdUnit(customerInfo.HouseholdNumber, customerInfo.HouseholdDateJalali, readingDateJalali);
            if (finalHousehold > 1)
            {
                return customerInfo.HouseholdNumber; //customerInfo.DomesticUnit;
            }

            return customerInfo.DomesticUnit - customerInfo.EmptyUnit < 1 ? 1 : customerInfo.DomesticUnit - customerInfo.EmptyUnit;
        }

        internal static bool GenerateBillIsGardenAndResidence(int usageId)
        {
            int[] s = [25, 34];
            return s.Contains(usageId);
        }

        private int GenerateBillGetHouseholdUnit(int householdUnit, string? householdDate, string readingDateJalali)
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

            if (readingDateGregorian.Value < expireHouseHoldGregorian.Value) //تاریخ قرائت قبل از تاریخ ثبت خانوار
            {
                return 0;
            }

            if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value) // تاریخ قرائت بعد از تاریخ انقضای خانوار
            {
                return 0;
            }

            return householdUnit;
        }

        private async Task GenerateBillSqlCommands(ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, BedBesCreateDto bedBes, KasrHaDto kasrHa, ContorUpdateDto contorUpdate, AbBahaCalculationDetails abBahaCalcResult, IAppUser appUser, int? counterStateCode, string logText)
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
                        BillByBedBedIdInsertDto billInsertByBedBesIdDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, GenerateBillGetTypeId(counterStateCode), bedBesRecordId);
                        await billCommandService.InsertByBedBesId(billInsertByBedBesIdDto, dbName);
                        await opLogcommandService.Insert(logText, appUser);
                        if (abBahaCalcResult.DiscountSum > 0)
                        {
                            await kasrHasCommandService.Insert(kasrHa, dbName);
                        }

                        if (!GenerateBillIsAllowedZeroMeterNumber(counterStateCode))
                        {
                            await waterDebtCommandService.UpdateAmount(bedBes.ShGhabs1, (long)bedBes.Baha);
                            await membersCommandService.UpdateBedbes(zoneIdAndCustomerNumber, (long)bedBes.Baha, bedBes.TodayDate, dbName);
                            await controCommandService.Update(contorUpdate, dbName, true); //update contro
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

        private async Task<ZoneIdAndCustomerNumber> GenerateBillGetZoneIdANdCustomerNumber(string billId)
        {
            ZoneIdAndCustomerNumber result = await _commonMemberQueryService.Get(billId);
            if (result.DeletionStateId == 1)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidDeletionState);
            }

            return result;
        }

        private ContorUpdateDto GenerateBillGetControUpdateDto(CustomerLastInfoGetDto customerInfo, BedBesCreateDto bedBes, int counterStateCode)
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

        private MeterInfoByPreviousDataInputDto GenerateBillGetMeterInfoByPreviousData(CustomerLastInfoGetDto customerInfo, GenerateBillInputDto generateBillInfo)
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

        private async Task<BedBesCreateDto> GenerateBillGetBedBes(CustomerLastInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, GenerateBillInputDto generateBillInfo, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, int? counterSatetCode)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(zoneIdAndCustomerNumber); //checkResult: changeDto
            var(sumItems, jam, pard) = TransactionIdGenerator.GetAmounts(preDebtAmount, abBahaCalc.SumItems);
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_GenerateBillpaymentDeadline).ToShortPersianDateString();
            string paymentIdOption = $"{CommonLiterals.WaterPayIdUniqueCode}{currentDateJalali.Substring(5, 2)}";
            string paymentId = GenerateBillIsAllowedZeroMeterNumber(counterSatetCode) || jam < CommonLiterals.BedBesConditionPayableAmount ? string.Empty : TransactionIdGenerator.GeneratePaymentId((long)pard, abBahaCalc.Customer.BillId, paymentIdOption);
            if (paymentId.ToString().Length > _GenerateBillpayIdMaxChar)
            {
                paymentId = string.Empty;
            }

            decimal barge = await _variabService.GetAndRenew(abBahaCalc.Customer.ZoneId);
            return new BedBesCreateDto() // ToDo :check
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
                Ztadil = 0, //
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
                ZaribFasl = (decimal)abBahaCalc.HotSeasonAbBahaAmount, //
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = customerInfo.OtherUnit,
                TedKhane = customerInfo.HouseholdNumber,
                TedadMas = customerInfo.DomesticUnit,
                TedadTej = customerInfo.CommercialUnit,
                NoeVa = abBahaCalc.Customer.BranchType,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1, //todo
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
                MasFas = GenerateBillGetSewageConsumption(abBahaCalc.Customer.UsageId, abBahaCalc.Consumption),
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

        private decimal GenerateBillGetSewageConsumption(int usageId, double consumption) => GenerateBillIsDomestic(usageId) ? (decimal)(consumption * _GenerateBilldomesticMaltiplier) : (decimal)consumption;
        private KasrHaDto GenerateBillGerKasrHa(CustomerLastInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, GenerateBillInputDto generateBillInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string paymentId = string.Empty; //TransactionIdGenerator.GeneratePaymentId((long)abBahaCalc.SumItems, abBahaCalc.Customer.BillId);
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

        private async Task GenerateBillValidate(GenerateBillInputDto inputDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, CustomerLastInfoGetDto customerInfo)
        {
            await GenerateBillDeletionStateValidation(zoneIdAndCustomerNumber);
            GenerateBillCounterStateValidation(inputDto.CounterStateCode, inputDto.MeterNumber, customerInfo.LastMeterNumber);
        }

        private async Task GenerateBillInputValidate(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _billGenerationGenerateBillvalidator.ValidateAsync(inputDto, cancellationToken);
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

        private async Task GenerateBillDeletionStateValidation(ZoneIdAndCustomerNumber input)
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

        private void GenerateBillCounterStateValidation(int? counterStateCode, int currentNumber, int? previousNumber)
        {
            if ((counterStateCode is null || !GenerateBillIsChangedOrReverse(counterStateCode)) && (previousNumber.HasValue) && !GenerateBillIsAllowedZeroMeterNumber(counterStateCode) && (currentNumber < previousNumber))
            {
                throw new TariffCalcException(ExceptionLiterals.CurrentNumberLessThanPreviousNumber);
            }
        }

        private bool GenerateBillIsChangedOrReverse(int? counterStateCode) => counterStateCode == (int)CounterStateCodeEnum.Reverse || counterStateCode == (int)CounterStateCodeEnum.NextRound || counterStateCode == (int)CounterStateCodeEnum.Change;
        private bool GenerateBillIsDomestic(int usageId) => _GenerateBilldomesticUsage.Contains(usageId);
        private bool GenerateBillIsAllowedZeroMeterNumber(int? counterStateCode) => _GenerateBillallowedZeroMeterNumberCounterState.Contains(counterStateCode ?? 0);
        private int GenerateBillGetDuration(string previousDate, string currentDate)
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

        private int GenerateBillGetTypeId(int? counterStateCode)
        {
            if (_GenerateBillallowedZeroMeterNumberCounterState.Contains(counterStateCode ?? 0))
            {
                return 8;
            }

            return 1; //todo: set other 
        }

        public async Task<BillIssueRemainedOutputDto> GenerateBillWithRemainedAsync(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            BillIssueRemainedOutputDto result = await GenerateBillWithRemainedGetResult(memberInfo, billId);
            result.FirstName = memberInfo.FirstName;
            result.SurName = memberInfo.Surname;
            result.FullName = memberInfo.FullName;
            string opLogText = string.Format(OpLogLiterals.GenerateBillIssueRemianedOpLog, result.BillId, result.PaymentId, result.Amount);
            await _opLogCommandService.Insert(opLogText, appUser);
            return result;
        }

        private async Task<BillIssueRemainedOutputDto> GenerateBillWithRemainedGetResult(MemberInfoGetDto memberInfo, string billId)
        {
            SubscriptionCardexInput inputDto = new(billId, string.Empty, true);
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> subscripitonInfo = await _subscriptionEventQueryService.GetEventsSummaryDtos(inputDto);
            long amount = subscripitonInfo.ReportHeader.Remained;
            string currentMonth = DateTime.Now.ToShortPersianDateString().Substring(5, 2);
            string paymentIdOption = $"{CommonLiterals.WaterPayIdUniqueCode}{currentMonth}";
            return new BillIssueRemainedOutputDto()
            {
                CustomerNumber = memberInfo.CustomerNumber,
                ZoneId = memberInfo.ZoneId,
                ZoneTitle = memberInfo.ZoneTitle,
                BillId = memberInfo.BillId,
                ReadingNumber = memberInfo.ReadingNumber,
                UsageId = memberInfo.UsageId,
                UsageTitle = memberInfo.UsageTitle,
                BranchTypeId = memberInfo.UseStateId,
                BranchTypeTitle = memberInfo.UseStateTitle,
                ContractualCapacity = memberInfo.ContractualCapacity,
                Amount = amount,
                PaymentId = TransactionIdGenerator.GeneratePaymentId(amount, billId, paymentIdOption),
            };
        }

        public async Task<NowGenerateBillOutputDto> NowGenerateBillAsync(NowGenerateBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            var limitResult = await _requestService.Register(inputDto, cancellationToken);
            if (limitResult == NowGenerateBillErrorCodeEnum.MobileDailyLimitReached)
            {
                return new NowGenerateBillOutputDto("به حداکثر تعداد درخواست در روز رسیده‌اید.", (int)limitResult);
            }

            if (limitResult == NowGenerateBillErrorCodeEnum.BillDailyLimitReached)
            {
                return new NowGenerateBillOutputDto("امکان صدور قبض برای این شناسه قبض در امروز وجود ندارد چون به حداکثر دفعات مجاز اعلام رقم کنتور رسیده است.", (int)limitResult);
            }

            await NowGenerateBillValidateInput(inputDto, cancellationToken);
            var(zoneIdAndCustomerNumber, customerInfo, customerValidateResult) = await NowGenerateBillCustomerValidate(inputDto);
            if (customerValidateResult is not null)
            {
                return customerValidateResult;
            }

            NowGenerateBillOutputDto? previousInfoValidate = NowGenerateBillPreviousInfoValidate(inputDto, customerInfo);
            if (previousInfoValidate is not null)
            {
                return previousInfoValidate;
            }

            bool limitReached = await _bedBesQueryService.HasConsecutiveSelfClaimBills(customerInfo.ZoneId, customerInfo.CustomerNumber, _NowGenerateBillMaxConsecutiveSelfClaimPeriods);
            if (limitReached)
            {
                return new NowGenerateBillOutputDto($"به سقف {_NowGenerateBillMaxConsecutiveSelfClaimPeriods} دوره متوالی صدور قبض خوداظهاری رسیده‌اید.", (int)NowGenerateBillErrorCodeEnum.NotAvailableNow);
            }

            NewBillOutputDto freeBillResultTemp = await NowGenerateBillCalcFreeBill(inputDto, customerInfo, false, appUser, cancellationToken);
            NowGenerateBillOutputDto? result = NowGenerateBillBillValidate(freeBillResultTemp, customerInfo);
            if (result is null)
            {
                NewBillOutputDto freeBillResult = await NowGenerateBillCalcFreeBill(inputDto, customerInfo, true, appUser, cancellationToken);
                return new NowGenerateBillOutputDto("موفق", (int)NowGenerateBillErrorCodeEnum.Success);
            }

            return result;
        }

        private async Task NowGenerateBillValidateInput(NowGenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _billGenerationNowGenerateBillvalidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

        private async Task<(ZoneIdAndCustomerNumber? , CustomerLastInfoGetDto? , NowGenerateBillOutputDto? )> NowGenerateBillCustomerValidate(NowGenerateBillInputDto inputDto)
        {
            string returnMessage = string.Empty;
            IHeaderDictionary requestHeader = _contextAccessor.HttpContext.Request.Headers;
            if (!requestHeader.Where(r => r.Key == _NowGenerateBillxApiKey && r.Value == _NowGenerateBillnowGenerateBillXApiKey).Any())
            {
                returnMessage = "احراز هویت با خطا روبرو شد.";
                NowGenerateBillOutputDto result = new(returnMessage, (int)NowGenerateBillErrorCodeEnum.BadRequest)
                {
                    SmsCategory = NowGenerateBillSmsCategoryEnum.System
                };
                return (null, null, result);
            }

            ZoneIdAndCustomerNumber? zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            if (zoneIdAndCustomerNumber == null)
            {
                returnMessage = "شناسه قبض معتبر نیست.";
                NowGenerateBillOutputDto result = new(returnMessage, (int)NowGenerateBillErrorCodeEnum.BadRequest);
                return (null, null, result);
            }

            CustomerLastInfoGetDto? customerInfo = await _commonMemberQueryService.GetMembersBedBesTavizFlatInfo(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, false);
            if (customerInfo is null)
            {
                returnMessage = "اطلاعات قبض قبلی به درستی یافت نشد.";
                NowGenerateBillOutputDto result = new(returnMessage, (int)NowGenerateBillErrorCodeEnum.BadRequest);
                return (null, null, result);
            }

            return (zoneIdAndCustomerNumber, customerInfo, null);
        }

        private NowGenerateBillOutputDto? NowGenerateBillPreviousInfoValidate(NowGenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo)
        {
            string returnMessage = string.Empty;
            if (_NowGenerateBillinvalidDeletionStateCodes.Contains(customerInfo.DeletionStateId))
            {
                returnMessage = "انشعاب جمع‌آوری شده است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.InvalidDeletionState);
            }

            if (customerInfo.LastMeterNumber > inputDto.CurrentMeterNumber)
            {
                returnMessage = "رقم فعلی کمتر از رقم قبلی است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.PreNumberHigherThanTodayNumber);
            }

            if (customerInfo.LastMeterNumber == inputDto.CurrentMeterNumber)
            {
                returnMessage = "رقم فعلی برابر با رقم قبلی است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.PreNumberEqualTodayNumber);
            }

            if (customerInfo.LastMeterDateJalali.CompareTo(DateTime.Now.AddDays(-1 * _NowGenerateBillinvalidLTDistanceDay).ToShortPersianDateString()) >= 0)
            {
                returnMessage = "برای مشترک در 10 روز گذشته قبض دیگری صادر شده.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.Low)
                {
                    SmsCategory = NowGenerateBillSmsCategoryEnum.RecentBill
                };
            }

            return null;
        }

        private NowGenerateBillOutputDto? NowGenerateBillBillValidate(NewBillOutputDto freeBillResult, CustomerLastInfoGetDto customerInfo)
        {
            string returnMessage = string.Empty;
            if (freeBillResult.AbBahaCalculationDetail.MonthlyConsumption >= customerInfo.LastMonthlyConsumption * _NowGenerateBillinvalidGTConsumptionPercent)
            {
                returnMessage = "میانگین مصرف دوره جاری بیش از 30درصد دوره قبلی است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.High);
            }

            if (freeBillResult.AbBahaCalculationDetail.MonthlyConsumption <= customerInfo.LastMonthlyConsumption * (1 - _NowGenerateBillinvalidLTConsumptionPercent))
            {
                returnMessage = "میانگین مصرف دوره جاری کمتر از 30درصد دوره قبلی است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.Low);
            }

            if (freeBillResult.AbBahaCalculationDetail.SumItems >= _NowGenerateBillinvalidGTAmount)
            {
                returnMessage = "مبلغ دوره جاری بیش از 50 میلیون تومان است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.High);
            }

            if (freeBillResult.AbBahaCalculationDetail.Consumption <= _NowGenerateBillinvalidLTConsumption)
            {
                returnMessage = "مصرف دوره جاری نمی‌تواند کمتر از 2مترمکعب باشد.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.Low);
            }

            return null;
        }

        private async Task<NewBillOutputDto> NowGenerateBillCalcFreeBill(NowGenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, bool isConfirm, IAppUser appUser, CancellationToken cancellationToken)
        {
            string detailLogText = OpLogLiterals.NowGenerateBillOpLog;
            FreeGenerateBillInputDto freeGenerateBillDto = new()
            {
                BillId = inputDto.BillId,
                CurrentMeterNumber = inputDto.CurrentMeterNumber,
                PreviousMeterNumber = customerInfo.LastMeterNumber ?? 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                PreviousDateJalali = customerInfo.LastMeterDateJalali,
                ConsumptionAverage = null,
                CounterStateCode = (int)CounterStateCodeEnum.Common,
                OperatorCode = FreeGenerateBillOperatorCodeEnum.SelfClaimRemote,
                IsConfirm = isConfirm,
            };
            NewBillOutputDto freeResult = await this.FreeGenerateBillAsync(freeGenerateBillDto, detailLogText, appUser, cancellationToken);
            return freeResult;
        }
    }
}
